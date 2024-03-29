﻿
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models;
using Palavyr.Core.Requests;
using Palavyr.Core.Resources;
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
        Task<SendLiveEmailResultResource> SendWidgetResponse(string intentId, EmailRequest emailRequest, bool isDemo);
        Task<SendLiveEmailResultResource> SendFallbackResponse(string intentId, EmailRequest emailRequest, bool isDemo);
    }

    public class ResponseEmailSender : IResponseEmailSender
    {
        private readonly IMapToNew<FileAsset, FileAssetResource> mapper;
        private readonly IEntityStore<Intent> intentStore;
        private readonly ILogger<ResponseEmailSender> logger;
        private readonly ICriticalResponses criticalResponses;
        private readonly IAttachmentRetriever attachmentRetriever;
        private readonly IEntityStore<Account> accountStore;
        private readonly ITemporaryPath temporaryPath;
        private readonly IResponsePdfGenerator responsePdfGenerator;
        private readonly ICompileSenderDetails compileSenderDetails;
        private readonly ISesEmail client;

        public ResponseEmailSender(
            IMapToNew<FileAsset, FileAssetResource> mapper,
            IEntityStore<Intent> intentStore,
            ILogger<ResponseEmailSender> logger,
            ICriticalResponses criticalResponses,
            IAttachmentRetriever attachmentRetriever,
            IEntityStore<Account> accountStore,
            ITemporaryPath temporaryPath,
            IResponsePdfGenerator responsePdfGenerator,
            ICompileSenderDetails compileSenderDetails,
            ISesEmail client)
        {
            this.mapper = mapper;
            this.intentStore = intentStore;
            this.logger = logger;
            this.criticalResponses = criticalResponses;
            this.attachmentRetriever = attachmentRetriever;
            this.accountStore = accountStore;
            this.temporaryPath = temporaryPath;
            this.responsePdfGenerator = responsePdfGenerator;
            this.compileSenderDetails = compileSenderDetails;
            this.client = client;
        }

        public async Task<SendLiveEmailResultResource> SendWidgetResponse(string intentId, EmailRequest emailRequest, bool isDemo)
        {
            var responses = criticalResponses.Compile(emailRequest.KeyValues);
            var culture = await accountStore.GetCulture();
            var intent = await intentStore.GetIntentOnly(intentId);

            var additionalFiles = new List<CloudFileDownloadRequest>();

            FileAssetResource fileAssetResource = null; // used to provide direct link in the chat
            if (intent.SendPdfResponse)
            {
                logger.LogDebug("Generating PDF Response from Send Email");
                var fileAsset = await responsePdfGenerator.GeneratePdfResponse(
                    responses,
                    emailRequest,
                    culture,
                    intentId,
                    isDemo
                );
                additionalFiles.Add(fileAsset.ToCloudFileDownloadRequest());
                fileAssetResource = await mapper.Map(fileAsset, default);
            }

            var senderDetails = await compileSenderDetails.Compile(intentId, emailRequest);
            var attachments = await attachmentRetriever.GatherAttachments(intentId, additionalFiles);

            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray(), fileAssetResource); // file asset goes to widget for show

            CleanUpLocalFiles(attachments);

            return responseResult;
        }

        public async Task<SendLiveEmailResultResource> SendFallbackResponse(string intentId, EmailRequest emailRequest, bool isDemo)
        {
            var sendAttachmentsOnFallback = await SendAttachmentsWhenFallback(intentId);

            IHaveBeenDownloadedFromCloudToLocal[] attachments;
            if (sendAttachmentsOnFallback)
            {
                attachments = await attachmentRetriever.GatherAttachments(intentId);
            }
            else
            {
                attachments = new IHaveBeenDownloadedFromCloudToLocal[] { };
            }

            var senderDetails = await compileSenderDetails.Compile(intentId, emailRequest);
            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray());

            CleanUpLocalFiles(attachments);

            return responseResult;
        }

        private void CleanUpLocalFiles(IHaveBeenDownloadedFromCloudToLocal[] attachments)
        {
            foreach (var attachment in attachments)
            {
                temporaryPath.DeleteLocalTempFile(attachment.FileNameWithExtension);
            }
        }

        private async Task<SendLiveEmailResultResource> Send(CompileSenderDetails.CompiledSenderDetails details, string[] attachments, FileAssetResource? fileAssetResource = null)
        {
            bool ok;
            if (attachments.Length == 0)
                ok = await client.SendEmail(
                    details.FromAddress,
                    details.ToAddress,
                    details.Subject,
                    details.BodyAsHtml,
                    details.BodyAsText,
                    notifyIntentOwner: true);
            else
                ok = await client.SendEmailWithAttachments(
                    details.FromAddress,
                    details.ToAddress,
                    details.Subject,
                    details.BodyAsHtml,
                    details.BodyAsText,
                    attachments.ToList(),
                    notifyIntentOwner: true); // Attachments here should be local file paths that are temporary

            return ok
                ? SendLiveEmailResultResource.CreateSuccess(EndingSequenceAttacher.EmailSuccessfulNodeId, fileAssetResource)
                : SendLiveEmailResultResource.CreateFailure(EndingSequenceAttacher.EmailFailedNodeId);
        }

        private async Task<bool> SendAttachmentsWhenFallback(string intentId)
        {
            var intent = await intentStore.GetIntentOnly(intentId);
            return intent.SendAttachmentsOnFallback;
        }
    }
}