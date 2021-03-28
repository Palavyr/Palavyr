using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystemTools.FormPaths;
using Palavyr.Common.FileSystemTools.ListPaths;
using Palavyr.Domain;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.AuthenticationServices;
using Palavyr.Services.DatabaseService;
using Palavyr.Services.EmailService.ResponseEmailTools;
using Palavyr.Services.PdfService;
using Palavyr.Services.PdfService.PdfSections.Util;

namespace Palavyr.API.Controllers.WidgetLive
{

    public class SendWidgetResponseEmailController : PalavyrBaseController
    {
        private readonly IDashConnector dashConnector;
        private readonly IAccountsConnector accountsConnector;
        private readonly IResponseCustomizer responseCustomizer;
        private readonly IConfiguration config;
        private readonly IPdfResponseGenerator pdfResponseGenerator;
        private readonly ISesEmail client;
        private ILogger logger;


        public SendWidgetResponseEmailController(
            IDashConnector dashConnector,
            IAccountsConnector accountsConnector,
            IResponseCustomizer responseCustomizer,
            ILogger<SendWidgetResponseEmailController> logger,
            ISesEmail client,
            IConfiguration config,
            IPdfResponseGenerator pdfResponseGenerator
        )
        {
            this.dashConnector = dashConnector;
            this.accountsConnector = accountsConnector;
            this.responseCustomizer = responseCustomizer;
            this.config = config;
            this.pdfResponseGenerator = pdfResponseGenerator;
            this.client = client;
            this.logger = logger;

        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost("widget/area/{areaId}/email/send")]
        public async Task<SendEmailResultResponse> SendEmail(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EmailRequest emailRequest
        )
        {
            logger.LogDebug("Attempting to send email from widget");

            var criticalResponses = new CriticalResponses(emailRequest.KeyValues);
            var attachmentFiles = AttachmentPaths.ListAttachmentsAsDiskPaths(accountId, areaId);

            var account = await accountsConnector.GetAccount(accountId);
            var locale = account.Locale;

            logger.LogDebug($"Locale being used: {locale}");
            var culture = new CultureInfo(locale);

            var safeFileNameStem = emailRequest.ConversationId;
            var safeFilePath = FormFilePath.FormResponsePDFFilePath(accountId, safeFileNameStem);

            await pdfResponseGenerator.GeneratePdfResponseAsync(
                criticalResponses, 
                emailRequest, 
                culture, 
                safeFilePath,
                safeFileNameStem,
                accountId,
                areaId
            );
            var fullPdfResponsePath = FormFilePath.FormResponsePDFFilePath(accountId, safeFilePath);
            if (DiskUtils.ValidatePathExists(fullPdfResponsePath))
            {
                attachmentFiles.Add(fullPdfResponsePath);
            }

            var fromAddress = account.EmailAddress;
            var toAddress = emailRequest.EmailAddress;

            var area = await dashConnector.GetAreaById(accountId, areaId);               

            var subject = area.UseAreaFallbackEmail ? account.GeneralFallbackSubject : area.Subject;
            var htmlBody = area.UseAreaFallbackEmail ? account.GeneralFallbackEmailTemplate : area.EmailTemplate;
            
            var textBody = ""; // This can be another upload. People can decide one or both. Html is prioritized.
            if (string.IsNullOrWhiteSpace(htmlBody))
            {
                htmlBody = "";
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                subject = "";
            }
            htmlBody = responseCustomizer.Customize(htmlBody, emailRequest, account);

            bool ok;
            if (attachmentFiles.Count == 0)
                ok = await client.SendEmail(fromAddress, toAddress, subject, htmlBody, textBody);
            else
                ok = await client.SendEmailWithAttachments(
                    fromAddress, toAddress, subject, htmlBody, textBody,
                    attachmentFiles);

            return ok 
                ? SendEmailResultResponse.Create(EndingSequence.EmailSuccessfulNodeId, true) 
                : SendEmailResultResponse.Create(EndingSequence.EmailFailedNodeId, false);
        }
    }
}