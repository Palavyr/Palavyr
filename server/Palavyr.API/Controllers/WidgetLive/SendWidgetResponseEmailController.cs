using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
using SESEmail = EmailService.ResponseEmail.SESEmail;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Route("api")]
    [ApiController]
    public class SendWidgetResponseEmailController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly SESEmail client;
        private ILogger<SendWidgetResponseEmailController> logger;
        private AccountsContext accountsContext;
        private ConvoContext convoContext;
        private DashContext dashContext;

        public SendWidgetResponseEmailController(
            ILogger<SendWidgetResponseEmailController> logger, 
            AccountsContext accountsContext,
            ConvoContext convoContext, 
            DashContext dashContext,
            IWebHostEnvironment env, 
            IAmazonSimpleEmailService SES, 
            IConfiguration config)
        {
            this.config = config;
            this.client = new SESEmail(logger, SES);
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.dashContext = dashContext;
            this.convoContext = convoContext;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.WidgetScheme)]
        [HttpPost("widget/area/{areaId}/email/send")]
        public async Task<IActionResult> SendEmail(
            [FromHeader] string accountId, 
            [FromRoute] string areaId,
            [FromBody] EmailRequest emailRequest)
        {
            logger.LogDebug("Attempting to send email from widget");

            var pdfGenerator = new PdfResponseGenerator(dashContext, accountsContext, convoContext, accountId, areaId,
                Request, logger);
            var criticalResponses = new CriticalResponses(emailRequest.KeyValues);
            var attachmentFiles = AttachmentPaths.ListAttachmentsAsDiskPaths(accountId, areaId);

            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            var locale = account.Locale;
            logger.LogDebug($"Locale being used: {locale}");
            var culture = new CultureInfo(locale);

            var safeFileNameStem = emailRequest.ConversationId;
            var safeFilePath = FormFilePath.FormResponsePDFFilePath(accountId, safeFileNameStem);

            await pdfGenerator.GeneratePdfResponseAsync(criticalResponses, emailRequest, culture, safeFilePath,
                safeFileNameStem);
            var fullPdfResponsePath = FormFilePath.FormResponsePDFFilePath(accountId, safeFilePath);
            if (DiskUtils.ValidatePathExists(fullPdfResponsePath))
            {
                attachmentFiles.Add(fullPdfResponsePath);
            }

            var fromAddress = accountsContext.Accounts.Single(row => row.AccountId == accountId).EmailAddress;
            var toAddress = emailRequest.EmailAddress;
            
            // TODO: Add database entry and frontend component to configure this value per area
            var area = dashContext.Areas.Single(row => row.AreaIdentifier == areaId);
            var subject = area.Subject;
            // var subject = "This subject line will be configured by user per area and default to a default address in the account settings.";
            var htmlBody = area.EmailTemplate;
            var textBody = ""; // This can be another upload. People can decide one or both. Html is prioritized.

            bool ok;
            if (attachmentFiles.Count == 0)
                ok = await client.SendEmail(fromAddress, toAddress, subject, htmlBody, textBody);
            else
                ok = await client.SendEmailWithAttachments(fromAddress, toAddress, subject, htmlBody, textBody,
                    attachmentFiles);

            return Ok(ok);
        }
    }
}