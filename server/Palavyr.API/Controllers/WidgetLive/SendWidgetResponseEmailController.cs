using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.API.chatUtils;
using DashboardServer.Data;
using EmailService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.receiverTypes;
using Palavyr.API.response;
using Palavyr.Common.FileSystem.FormPaths;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace Palavyr.API.Controllers.Emails
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
            var fullPDFResponsePath = FormFilePath.FormResponsePDFFilePath(accountId, safeFilePath);
            if (DiskUtils.ValidatePathExists(fullPDFResponsePath))
            {
                attachmentFiles.Add(fullPDFResponsePath);
            }

            var fromAddress = accountsContext.Accounts.Single(row => row.AccountId == accountId).EmailAddress;
            var toAddress = emailRequest.EmailAddress;
            
            // TODO: Add database entry and frontend component to configure this value per area
            var subject = "This subject line will be configured by user per area and default to a default address in the account settings.";
            var htmlBody = dashContext.Areas.Single(row => row.AreaIdentifier == areaId).EmailTemplate;
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