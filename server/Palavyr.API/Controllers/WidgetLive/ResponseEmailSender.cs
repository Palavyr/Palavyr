using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.API.Controllers.WidgetLive
{
    public interface IResponseEmailSender
    {
        Task<SendEmailResultResponse> SendEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken);
        Task<SendEmailResultResponse> SendFallbackEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken);
    }

    public class ResponseEmailSender : IResponseEmailSender
    {
        private readonly ICriticalResponses criticalResponses;
        private readonly IAttachmentRetriever attachmentRetriever;
        private readonly IAccountRepository accountRepository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly ITempPathCreator tempPathCreator;
        private readonly ILocalFileDeleter localFileDeleter;
        private readonly IPdfResponseGenerator pdfResponseGenerator;
        private readonly ICompileSenderDetails compileSenderDetails;
        private readonly ISesEmail client;

        public ResponseEmailSender(
            ICriticalResponses criticalResponses,
            IAttachmentRetriever attachmentRetriever,
            IAccountRepository accountRepository,
            IConfigurationRepository configurationRepository,
            ITempPathCreator tempPathCreator,
            ILocalFileDeleter localFileDeleter,
            IPdfResponseGenerator pdfResponseGenerator,
            ICompileSenderDetails compileSenderDetails,
            ISesEmail client
        )
        {
            this.criticalResponses = criticalResponses;
            this.attachmentRetriever = attachmentRetriever;
            this.accountRepository = accountRepository;
            this.configurationRepository = configurationRepository;
            this.tempPathCreator = tempPathCreator;
            this.localFileDeleter = localFileDeleter;
            this.pdfResponseGenerator = pdfResponseGenerator;
            this.compileSenderDetails = compileSenderDetails;
            this.client = client;
        }

        public async Task<SendEmailResultResponse> SendEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            var responses = criticalResponses.Compile(emailRequest.KeyValues);

            // TODO: This may need to work a little differently. Previously we sent emails from local files. That works.
            // Here we might try to attach files directly from S3 but I think we need to transfer them first to the server.
            var attachments = await attachmentRetriever.RetrieveAttachmentFiles(accountId, areaId, cancellationToken);

            var culture = await GetCulture(accountId);
            var safeFileNameStem = emailRequest.ConversationId;
            var localFilePath = await pdfResponseGenerator.GeneratePdfResponseAsync(
                responses,
                emailRequest,
                culture,
                tempPathCreator.Create(safeFileNameStem + ".pdf"),
                safeFileNameStem,
                accountId,
                areaId
            );

            if (localFilePath == null)
            {
                throw new Exception("Could not generate PDF Response");
            }

            var senderDetails = await compileSenderDetails.Compile(accountId, areaId, emailRequest);
            var responseResult = await Send(senderDetails, attachments);
            localFileDeleter.Delete(localFilePath);
            foreach (var attachment in attachments)
            {
                localFileDeleter.Delete(attachment);
            }

            return responseResult;
        }

        public async Task<SendEmailResultResponse> SendFallbackEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            var sendAttachmentsOnFallback = await SendAttachmentsWhenFallback(accountId, areaId);

            string[] attachments;
            if (sendAttachmentsOnFallback)
            {
                attachments = await attachmentRetriever.RetrieveAttachmentFiles(accountId, areaId, cancellationToken);
            }
            else
            {
                attachments = new string[] { };
            }

            var senderDetails = await compileSenderDetails.Compile(accountId, areaId, emailRequest);
            var responseResult = await Send(senderDetails, attachments);
            foreach (var attachment in attachments)
            {
                localFileDeleter.Delete(attachment);
            }

            return responseResult;
        }

        private async Task<SendEmailResultResponse> Send(CompileSenderDetails.CompiledSenderDetails details, string[] attachments)
        {
            bool ok;
            if (attachments.Length == 0)
                ok = await client.SendEmail(
                    details.FromAddress,
                    details.ToAddress,
                    details.Subject,
                    details.BodyAsHtml,
                    details.BodyAsText);
            else
                ok = await client.SendEmailWithAttachments(
                    details.FromAddress,
                    details.ToAddress,
                    details.Subject,
                    details.BodyAsHtml,
                    details.BodyAsText,
                    attachments.ToList());

            return ok
                ? SendEmailResultResponse.CreateSuccess(EndingSequence.EmailSuccessfulNodeId)
                : SendEmailResultResponse.CreateFailure(EndingSequence.EmailFailedNodeId);
        }

        private async Task<CultureInfo> GetCulture(string accountId)
        {
            var account = await accountRepository.GetAccount(accountId);
            var locale = account.Locale ?? LocaleDefinition.DefaultCountryId;
            return new CultureInfo(locale);
        }

        private async Task<bool> SendAttachmentsWhenFallback(string accountId, string areaId)
        {
            var account = await configurationRepository.GetAreaById(accountId, areaId);
            return account.SendAttachmentsOnFallback;
        }
    }
}