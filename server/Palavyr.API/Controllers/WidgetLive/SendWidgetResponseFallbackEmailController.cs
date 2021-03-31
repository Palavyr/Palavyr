using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.FileSystemTools.ListPaths;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;

namespace Palavyr.API.Controllers.WidgetLive
{

    public class SendWidgetResponseFallbackEmailController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IResponseCustomizer responseCustomizer;
        private readonly ISesEmail client;
        private ILogger logger;

        public SendWidgetResponseFallbackEmailController(
            IAccountRepository accountRepository,
            IConfigurationRepository configurationRepository,
            IResponseCustomizer responseCustomizer,
            ILogger<SendWidgetResponseFallbackEmailController> logger,
            ISesEmail client
        )
        {
            this.accountRepository = accountRepository;
            this.configurationRepository = configurationRepository;
            this.responseCustomizer = responseCustomizer;
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
            var account = await accountRepository.GetAccount(accountId);

            var fromAddress = account.EmailAddress;
            var toAddress = emailRequest.EmailAddress;

            var area = await configurationRepository.GetAreaById(accountId, areaId);

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
            fallbackHtmlBody = responseCustomizer.Customize(fallbackHtmlBody, emailRequest, account);

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