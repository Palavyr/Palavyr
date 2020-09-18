using System;
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
using Palavyr.API.GeneratePdf;
using Palavyr.API.pathUtils;
using Palavyr.API.receiverTypes;
using Palavyr.Common.FileSystem;

namespace Palavyr.API.Controllers.Emails
{
    [Route("api/widget")]
    [ApiController]
    public class SendEmailController : BaseController
    {
        private IConfiguration Config { get; set; }
        private SESEmail Client { get; set; } // Startup.cs handles finding credentials from appsettings.json through GetAWSOptions
        private static ILogger<SendEmailController> _logger;

        public SendEmailController(ILogger<SendEmailController> logger, AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext,
            IWebHostEnvironment env, IAmazonSimpleEmailService SES, IConfiguration config) : base(accountContext, convoContext, dashContext, env)
        {
            Config = config;
            Client = new SESEmail(SES);
            _logger = logger;
        }

        private string SetUserDb(string apiKey)
        {
            var account = AccountContext.Accounts.Single(row => row.ApiKey == apiKey);
            return account.AccountId;
        }

       
        [HttpPost("{apiKey}/area/{areaId}/email/send")]
        public async Task<StatusCodeResult> SendEmail(string apiKey, string areaId, [FromBody] EmailRequest userDetails)
        {
            var accountId = SetUserDb(apiKey);
            var pdfGenerator = new PdfResponseGenerator(DashContext, AccountContext, ConvoContext, accountId, areaId, Request);
            var criticalResponses = new CriticalResponses(userDetails.KeyValues);
            var attachmentFiles = AttachmentPaths.ListAttachmentsAsDiskPaths(accountId, areaId);

            var account = AccountContext.Accounts.Single(row => row.AccountId == accountId);
            var locale = account.Locale;
            var culture = new CultureInfo(locale);

            var safeFileNameStem = Guid.NewGuid().ToString();
            var safeFilePath = FormFilePath.FormResponsePDFFilePath(accountId, safeFileNameStem);

            await pdfGenerator.GeneratePdfResponseAsync(criticalResponses, userDetails, culture, safeFilePath, safeFileNameStem);
            var fullPDFResponsePath = FormFilePath.FormResponsePDFFilePath(accountId, safeFilePath);
            if (DiskUtils.ValidatePathExists(fullPDFResponsePath))
                attachmentFiles.Add(fullPDFResponsePath);

            var fromAddress = AccountContext.Accounts.Single(row => row.AccountId == accountId).EmailAddress;
            var toAddress = userDetails.EmailAddress;
            var subject =
                "This subject line will be configured by user per area and default to a default address in the account settings.";
            var htmlBody = DashContext.Areas.Single(row => row.AreaIdentifier == areaId).EmailTemplate;
            var textBody = "";// This can be another upload. People can decide one or both. Html is prioritized.

            bool ok;
            if (attachmentFiles.Count == 0)
                ok = await Client.SendEmail(fromAddress, toAddress, subject, htmlBody, textBody);
            else
                ok = await Client.SendEmailWithAttachments(fromAddress, toAddress, subject, htmlBody, textBody, attachmentFiles);
            
            return ok ? (StatusCodeResult) new OkResult() : new NotFoundResult();
        }
    }
}