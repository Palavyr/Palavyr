using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.ResponseEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Palavyr.API.Response;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.Common.FileSystem.ListPaths;
using PDFService.Sections.Util;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Route("api")]
    [ApiController]
    public class SendWidgetResponseEmailController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IPdfResponseGenerator pdfResponseGenerator;
        private readonly ISesEmail client;
        private ILogger logger;
        private AccountsContext accountsContext;
        private DashContext dashContext;

        public SendWidgetResponseEmailController(
            ILogger<SendWidgetResponseEmailController> logger,
            AccountsContext accountsContext,
            DashContext dashContext,
            ISesEmail client,
            IConfiguration config,
            IPdfResponseGenerator pdfResponseGenerator
        )
        {
            this.config = config;
            this.pdfResponseGenerator = pdfResponseGenerator;
            this.client = client;
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.dashContext = dashContext;
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

            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
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

            var fromAddress = accountsContext.Accounts.Single(row => row.AccountId == accountId).EmailAddress;
            var toAddress = emailRequest.EmailAddress;

            var area = dashContext.Areas.Single(row => row.AreaIdentifier == areaId);
            var subject = area.Subject;
            var htmlBody = area.EmailTemplate;
            var textBody = ""; // This can be another upload. People can decide one or both. Html is prioritized.

            htmlBody = ResponseCustomizer.Customize(htmlBody, emailRequest, account);

            bool ok;
            if (attachmentFiles.Count == 0)
                ok = await client.SendEmail(fromAddress, toAddress, subject, htmlBody, textBody);
            else
                ok = await client.SendEmailWithAttachments(
                    fromAddress, toAddress, subject, htmlBody, textBody,
                    attachmentFiles);

            return ok ? SendEmailResultResponse.Create(EndingSequence.EmailSuccessfulNodeId, ok) : SendEmailResultResponse.Create(EndingSequence.EmailFailedNodeId, ok);
        }
    }
}