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
using Palavyr.Common.FileSystem.ListPaths;

namespace Palavyr.API.Controllers.WidgetLive
{
    [Route("api")]
    [ApiController]
    public class SendWidgetResponseFallbackEmailController : ControllerBase
    {
        private readonly ISesEmail client;
        private ILogger logger;
        private AccountsContext accountsContext;
        private DashContext dashContext;

        public SendWidgetResponseFallbackEmailController(
            ILogger<SendWidgetResponseFallbackEmailController> logger,
            AccountsContext accountsContext,
            DashContext dashContext,
            ISesEmail client
        )
        {
            this.client = client;
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.dashContext = dashContext;
        }

        [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]
        [HttpPost("widget/area/{areaId}/email/fallback/send")]
        public async Task<SendEmailResultResponse> SendEmail(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromBody] EmailRequest emailRequest
        )
        {
            logger.LogDebug("Attempting to send email from widget");

            var attachmentFiles = AttachmentPaths.ListAttachmentsAsDiskPaths(accountId, areaId);
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);

            var fromAddress = accountsContext.Accounts.Single(row => row.AccountId == accountId).EmailAddress;
            var toAddress = emailRequest.EmailAddress;

            var area = dashContext.Areas.Single(row => row.AreaIdentifier == areaId);
            
            var fallbackSubject = area.UseAreaFallbackEmail 
                ? area.FallbackSubject
                : account.GeneralFallbackSubject;

            var fallbackHtmlBody = area.UseAreaFallbackEmail
                ? area.FallbackEmailTemplate
                : account.GeneralFallbackEmailTemplate;
            
            
            var fallbackTextBody = area.FallbackEmailTemplate; // This can be another upload. People can decide one or both. Html is prioritized.

            fallbackHtmlBody = ResponseCustomizer.Customize(fallbackHtmlBody, emailRequest, account);

            bool ok;
            if (attachmentFiles.Count == 0)
                ok = await client.SendEmail(fromAddress, toAddress, fallbackSubject, fallbackHtmlBody, fallbackTextBody);
            else
                ok = await client.SendEmailWithAttachments(
                    fromAddress, 
                    toAddress, 
                    fallbackSubject, 
                    fallbackHtmlBody, 
                    fallbackTextBody,
                    attachmentFiles);

            return ok
                ? SendEmailResultResponse.Create(EndingSequence.EmailSuccessfulNodeId, ok)
                : SendEmailResultResponse.Create(EndingSequence.FallbackEmailFailedNodeId, ok);
        }
    }
}