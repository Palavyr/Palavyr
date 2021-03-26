using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystemTools.ListPaths;
using Palavyr.Domain;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.AuthenticationServices;
using Palavyr.Services.DatabaseService;
using Palavyr.Services.EmailService.ResponseEmailTools;

namespace Palavyr.API.Controllers.WidgetLive
{

    public class SendWidgetResponseFallbackEmailController : PalavyrBaseController
    {
        private readonly IAccountsConnector accountsConnector;
        private readonly IDashConnector dashConnector;
        private readonly ISesEmail client;
        private ILogger logger;

        public SendWidgetResponseFallbackEmailController(
            IAccountsConnector accountsConnector,
            IDashConnector dashConnector,
            ILogger<SendWidgetResponseFallbackEmailController> logger,
            ISesEmail client
        )
        {
            this.accountsConnector = accountsConnector;
            this.dashConnector = dashConnector;
            this.client = client;
            this.logger = logger;
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
            var account = await accountsConnector.GetAccount(accountId);

            var fromAddress = account.EmailAddress;
            var toAddress = emailRequest.EmailAddress;

            var area = await dashConnector.GetAreaById(accountId, areaId);

            var fallbackSubject = area.UseAreaFallbackEmail
                ? area.FallbackSubject
                : account.GeneralFallbackSubject;

            var fallbackHtmlBody = area.UseAreaFallbackEmail
                ? area.FallbackEmailTemplate
                : account.GeneralFallbackEmailTemplate;


            var fallbackTextBody = area.FallbackEmailTemplate; // This can be another upload. People can decide one or both. Html is prioritized.
            if (string.IsNullOrWhiteSpace(fallbackHtmlBody))
            {
                fallbackHtmlBody = "";
            }

            if (string.IsNullOrWhiteSpace(fallbackSubject))
            {
                fallbackSubject = "";
            }
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
                ? SendEmailResultResponse.Create(EndingSequence.EmailSuccessfulNodeId, true)
                : SendEmailResultResponse.Create(EndingSequence.FallbackEmailFailedNodeId, false);
        }
    }
}