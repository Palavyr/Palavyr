#nullable enable
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.EmailService.EmailResponse
{
    public interface IResponseEmailSender
    {
        Task<SendEmailResultResponse> SendEmail(string intentId, EmailRequest emailRequest);
        Task<SendEmailResultResponse> SendFallbackEmail(string intentId, EmailRequest emailRequest);
    }

    public class ResponseEmailSender : IResponseEmailSender
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly IEntityStore<ConversationRecord> convoRecordStore;
        private readonly ILogger<ResponseEmailSender> logger;
        private readonly ICriticalResponses criticalResponses;
        private readonly IAttachmentRetriever attachmentRetriever;
        private readonly IEntityStore<Account> accountStore;
        private readonly ITemporaryPath temporaryPath;
        private readonly IResponsePdfGenerator responsePdfGenerator;
        private readonly ICompileSenderDetails compileSenderDetails;
        private readonly ISesEmail client;
        private readonly IConfiguration configuration;
        private readonly ILocaleDefinitions localeDefinitions;
        private readonly ILinkCreator linkCreator;

        public ResponseEmailSender(
            IEntityStore<Area> intentStore,
            IEntityStore<ConversationRecord> convoRecordStore,
            ILogger<ResponseEmailSender> logger,
            ICriticalResponses criticalResponses,
            IAttachmentRetriever attachmentRetriever,
            IEntityStore<Account> accountStore,
            ITemporaryPath temporaryPath,
            IResponsePdfGenerator responsePdfGenerator,
            ICompileSenderDetails compileSenderDetails,
            ISesEmail client,
            IConfiguration configuration,
            ILocaleDefinitions localeDefinitions,
            ILinkCreator linkCreator)
        {
            this.intentStore = intentStore;
            this.convoRecordStore = convoRecordStore;
            this.logger = logger;
            this.criticalResponses = criticalResponses;
            this.attachmentRetriever = attachmentRetriever;
            this.accountStore = accountStore;
            this.temporaryPath = temporaryPath;
            this.responsePdfGenerator = responsePdfGenerator;
            this.compileSenderDetails = compileSenderDetails;
            this.client = client;
            this.configuration = configuration;
            this.localeDefinitions = localeDefinitions;
            this.linkCreator = linkCreator;
        }

        public async Task<SendEmailResultResponse> SendEmail(string intentId, EmailRequest emailRequest)
        {
            var responses = criticalResponses.Compile(emailRequest.KeyValues);
            logger.LogDebug("Compiled successfully");

            var culture = await GetCulture();
            var localTempPath = temporaryPath.CreateLocalTempSafeFile(emailRequest.ConversationId);
            logger.LogDebug("culture and local temp path gotten");

            var intent = await intentStore.GetIntentOnly(intentId);
            logger.LogDebug($"{intent.AreaIdentifier} found");

            var additionalFiles = new List<CloudFileDownloadRequest>();
            string? pdfLink = null; // used to provide direct link in the chat

            if (intent.SendPdfResponse)
            {
                logger.LogDebug("Generating PDF Response from Send Email");
                var pdfFileAsset = await responsePdfGenerator.GeneratePdfResponse(
                    responses,
                    emailRequest,
                    culture,
                    emailRequest.ConversationId,
                    intentId
                );
                pdfLink = await linkCreator.CreateLink(pdfFileAsset.FileId);
                additionalFiles.Add(pdfFileAsset.ToCloudFileDownloadRequest());
            }

            var senderDetails = await compileSenderDetails.Compile(intentId, emailRequest);

            var attachments = await attachmentRetriever.GatherAttachments(intentId, additionalFiles);

            logger.LogDebug("Sending Email...");
            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray(), pdfLink);

            foreach (var attachment in attachments)
            {
                logger.LogDebug($"Deleting locale temp file: {attachment.FileNameWithExtension}");
                temporaryPath.DeleteLocalTempFile(attachment.FileNameWithExtension);
            }

            return responseResult;
        }

        public async Task<SendEmailResultResponse> SendFallbackEmail(string intentId, EmailRequest emailRequest)
        {
            var sendAttachmentsOnFallback = await SendAttachmentsWhenFallback(intentId);

            IHaveBeenDownloadedFromCloudToLocal[] attachments;
            if (sendAttachmentsOnFallback)
            {
                attachments = await attachmentRetriever.GatherAttachments(intentId);
                // var metas = await attachmentRetriever.RetrievePdfUris(areaId, cancellationToken);
                // attachments = await attachmentRetriever.DownloadForAttachmentToEmail(areaId,  metas, cancellationToken);
            }
            else
            {
                attachments = new IHaveBeenDownloadedFromCloudToLocal[] { };
            }

            var senderDetails = await compileSenderDetails.Compile(intentId, emailRequest);
            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray());
            foreach (var attachment in attachments)
            {
                temporaryPath.DeleteLocalTempFile(attachment.FileNameWithExtension);
            }

            return responseResult;
        }

        private async Task<SendEmailResultResponse> Send(CompileSenderDetails.CompiledSenderDetails details, string[] attachments, string? pdfUri = null)
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
                    attachments.ToList()); // Attachments here should be local file paths that are temporary

            return ok
                ? SendEmailResultResponse.CreateSuccess(EndingSequenceAttacher.EmailSuccessfulNodeId, pdfUri)
                : SendEmailResultResponse.CreateFailure(EndingSequenceAttacher.EmailFailedNodeId);
        }

        private async Task<CultureInfo> GetCulture()
        {
            var account = await accountStore.GetAccount();
            var locale = account.Locale ?? localeDefinitions.DefaultLocale.Name;
            return new CultureInfo(locale);
        }

        private async Task<bool> SendAttachmentsWhenFallback(string intentId)
        {
            var intent = await intentStore.GetIntentOnly(intentId);
            return intent.SendAttachmentsOnFallback;
        }
    }
}