using System;
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
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.EmailService.EmailResponse
{
    public interface IResponseEmailSender
    {
        Task<SendEmailResultResponse> SendEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken);
        Task<SendEmailResultResponse> SendFallbackEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken);
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
        private readonly IS3Saver s3Saver;
        private readonly IConfiguration configuration;
        private readonly IS3KeyResolver s3KeyResolver;

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
            IS3Saver s3Saver,
            IConfiguration configuration,
            IS3KeyResolver s3KeyResolver
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
            this.s3Saver = s3Saver;
            this.configuration = configuration;
            this.s3KeyResolver = s3KeyResolver;
        }

        public async Task<SendEmailResultResponse> SendEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            var responses = criticalResponses.Compile(emailRequest.KeyValues);

            var culture = await GetCulture(accountId, cancellationToken);
            var localTempPath = temporaryPath.CreateLocalTempSafeFile(emailRequest.ConversationId);

            logger.LogDebug("Generating PDF Response from Send Email");
            var pdfServerResponse = await pdfResponseGenerator.GeneratePdfResponseAsync(
                responses,
                emailRequest,
                culture,
                localTempPath.FileStem, 
                accountId,
                areaId,
                cancellationToken
            );
            var senderDetails = await compileSenderDetails.Compile(accountId, areaId, emailRequest, cancellationToken);
            var attachments = await attachmentRetriever.RetrieveAttachmentFiles(accountId, areaId, new[] {pdfServerResponse.ToS3DownloadRequestMeta()}, cancellationToken);

            logger.LogDebug("Sending Email...");
            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray());
            foreach (var attachment in attachments)
            {
                logger.LogDebug($"Deleting locale tempfile: {attachment.FileNameWithExtension}");
                temporaryPath.DeleteLocalTempFile(attachment.FileNameWithExtension);
            }

            return responseResult;
        }

        public async Task<SendEmailResultResponse> SendFallbackEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            var sendAttachmentsOnFallback = await SendAttachmentsWhenFallback(accountId, areaId);

            IHaveBeenDownloadedFromS3[] attachments;
            if (sendAttachmentsOnFallback)
            {
                attachments = await attachmentRetriever.RetrieveAttachmentFiles(accountId, areaId, null, cancellationToken);
            }
            else
            {
                attachments = new IHaveBeenDownloadedFromS3[] { };
            }

            var senderDetails = await compileSenderDetails.Compile(accountId, areaId, emailRequest, cancellationToken);
            var responseResult = await Send(senderDetails, attachments.Select(x => x.TempFilePath).ToArray());
            foreach (var attachment in attachments)
            {
                temporaryPath.DeleteLocalTempFile(attachment.FileNameWithExtension);
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
                    attachments.ToList());// Attachments here should be local file paths that are temporary

            return ok
                ? SendEmailResultResponse.CreateSuccess(EndingSequence.EmailSuccessfulNodeId)
                : SendEmailResultResponse.CreateFailure(EndingSequence.EmailFailedNodeId);
        }

        private async Task<CultureInfo> GetCulture(string accountId, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount(accountId, cancellationToken);
            var locale = account.Locale ?? LocaleDefinition.DefaultCountryId;
            return new CultureInfo(locale);
        }

        private async Task<bool> SendAttachmentsWhenFallback(string accountId, string areaId)
        {
            var account = await configurationRepository.GetAreaById(accountId, areaId);
            return account.SendAttachmentsOnFallback;
        }

        private async Task SaveResponsePdfToS3(string localFilePath, string accountId, string safeFileNameStem)
        {
            var userDataBucket = configuration.GetUserDataSection();
            var success = await s3Saver.SaveObjectToS3(userDataBucket, localFilePath, s3KeyResolver.ResolveResponsePdfKey(accountId, safeFileNameStem));
            if (!success)
            {
                throw new Exception("Could not save pdf response to S3");
            }
        }
    }
}