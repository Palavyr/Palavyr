#nullable enable
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.EmailService.EmailResponse
{
    public interface IResponseEmailSender
    {
        Task<SendEmailResultResponse> SendEmail(string areaId, EmailRequest emailRequest, CancellationToken cancellationToken);
        Task<SendEmailResultResponse> SendFallbackEmail(string areaId, EmailRequest emailRequest, CancellationToken cancellationToken);
    }

    public class ResponseEmailSender : IResponseEmailSender
    {
        private readonly ILogger<ResponseEmailSender> logger;
        private readonly ICriticalResponses criticalResponses;
        private readonly IAttachmentRetriever attachmentRetriever;
        private readonly IAccountRepository accountRepository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly ITemporaryPath temporaryPath;
        private readonly IPdfResponseGenerator pdfResponseGenerator;
        private readonly ICompileSenderDetails compileSenderDetails;
        private readonly ISesEmail client;
        private readonly IConfiguration configuration;
        private readonly ILocaleDefinitions localeDefinitions;
        private readonly ILinkCreator linkCreator;
        private readonly IConvoHistoryRepository convoHistoryRepository;
        private readonly IHoldAnAccountId accountIdHolder;

        public ResponseEmailSender(
            ILogger<ResponseEmailSender> logger,
            ICriticalResponses criticalResponses,
            IAttachmentRetriever attachmentRetriever,
            IAccountRepository accountRepository,
            IConfigurationRepository configurationRepository,
            ITemporaryPath temporaryPath,
            IPdfResponseGenerator pdfResponseGenerator,
            ICompileSenderDetails compileSenderDetails,
            ISesEmail client,
            IConfiguration configuration,
            ILocaleDefinitions localeDefinitions,
            ILinkCreator linkCreator,
            IConvoHistoryRepository convoHistoryRepository,
            IHoldAnAccountId accountIdHolder
        )
        {
            this.logger = logger;
            this.criticalResponses = criticalResponses;
            this.attachmentRetriever = attachmentRetriever;
            this.accountRepository = accountRepository;
            this.configurationRepository = configurationRepository;
            this.temporaryPath = temporaryPath;
            this.pdfResponseGenerator = pdfResponseGenerator;
            this.compileSenderDetails = compileSenderDetails;
            this.client = client;
            this.configuration = configuration;
            this.localeDefinitions = localeDefinitions;
            this.linkCreator = linkCreator;
            this.convoHistoryRepository = convoHistoryRepository;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task<SendEmailResultResponse> SendEmail(string areaId, EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            var responses = criticalResponses.Compile(emailRequest.KeyValues);
            logger.LogDebug("Compiled successfully");

            var culture = await GetCulture();
            var localTempPath = temporaryPath.CreateLocalTempSafeFile(emailRequest.ConversationId);
            logger.LogDebug("culture and local temp path gotten");

            var area = await configurationRepository.GetAreaById(areaId);
            logger.LogDebug($"{area.AreaIdentifier} found");

            var additionalFiles = new List<S3SDownloadRequestMeta>();

            string? pdfLink = null;
            if (area.SendPdfResponse)
            {
                logger.LogDebug("Generating PDF Response from Send Email");
                var pdfServerResponse = await pdfResponseGenerator.GeneratePdfResponseAsync(
                    responses,
                    emailRequest,
                    culture,
                    localTempPath.FileStem,
                    areaId
                );
                pdfLink = linkCreator.GenericCreatePreSignedUrl(pdfServerResponse.S3Key, configuration.GetUserDataBucket());
                additionalFiles.Add(pdfServerResponse.ToS3DownloadRequestMeta());

                var currentConvoRecord = await convoHistoryRepository.GetConversationRecordById(emailRequest.ConversationId);
                currentConvoRecord.ResponsePdfId = emailRequest.ConversationId;
                await convoHistoryRepository.UpdateConversationRecord(currentConvoRecord);

            }

            var senderDetails = await compileSenderDetails.Compile(areaId, emailRequest);

            var s3DownloadRequestMetas = await attachmentRetriever.RetrievePdfUris(areaId, cancellationToken);
            
            if (additionalFiles != null)
            {
                s3DownloadRequestMetas.AddRange(additionalFiles);
            }
            
            var attachments = await attachmentRetriever.RetrieveAttachmentFiles(areaId, s3DownloadRequestMetas, cancellationToken);
            
            
            logger.LogDebug("Sending Email...");
            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray(), pdfLink);
            
            foreach (var attachment in attachments)
            {
                logger.LogDebug($"Deleting locale temp file: {attachment.FileNameWithExtension}");
                temporaryPath.DeleteLocalTempFile(attachment.FileNameWithExtension);
            }

            await convoHistoryRepository.CommitChangesAsync(cancellationToken);
            return responseResult;
        }

        public async Task<SendEmailResultResponse> SendFallbackEmail(string areaId, EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            var sendAttachmentsOnFallback = await SendAttachmentsWhenFallback(areaId);

            IHaveBeenDownloadedFromS3[] attachments;
            if (sendAttachmentsOnFallback)
            {
                var metas = await attachmentRetriever.RetrievePdfUris(areaId, cancellationToken);
                attachments = await attachmentRetriever.RetrieveAttachmentFiles(areaId,  metas, cancellationToken);
            }
            else
            {
                attachments = new IHaveBeenDownloadedFromS3[] { };
            }

            var senderDetails = await compileSenderDetails.Compile(areaId, emailRequest);
            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray());
            foreach (var attachment in attachments)
            {
                temporaryPath.DeleteLocalTempFile(attachment.FileNameWithExtension);
            }

            return responseResult;
        }

        private async Task<SendEmailResultResponse> Send(CompileSenderDetails.CompiledSenderDetails details, string[] attachments, string? pdfUri = null, CancellationToken cancellationToken = default)
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
            var account = await accountRepository.GetAccount();
            var locale = account.Locale ?? localeDefinitions.DefaultLocale.Name;
            return new CultureInfo(locale);
        }

        private async Task<bool> SendAttachmentsWhenFallback(string areaId)
        {
            var account = await configurationRepository.GetAreaById(areaId);
            return account.SendAttachmentsOnFallback;
        }
    }
}