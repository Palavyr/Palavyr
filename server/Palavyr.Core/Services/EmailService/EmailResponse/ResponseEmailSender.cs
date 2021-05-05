using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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

namespace Palavyr.Core.Services.EmailService.EmailResponse
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
        private readonly IS3Saver s3Saver;
        private readonly IConfiguration configuration;
        private readonly IS3KeyResolver s3KeyResolver;

        public ResponseEmailSender(
            ICriticalResponses criticalResponses,
            IAttachmentRetriever attachmentRetriever,
            IAccountRepository accountRepository,
            IConfigurationRepository configurationRepository,
            ITempPathCreator tempPathCreator,
            ILocalFileDeleter localFileDeleter,
            IPdfResponseGenerator pdfResponseGenerator,
            ICompileSenderDetails compileSenderDetails,
            ISesEmail client,
            IS3Saver s3Saver,
            IConfiguration configuration, 
            IS3KeyResolver s3KeyResolver
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
            this.s3Saver = s3Saver;
            this.configuration = configuration;
            this.s3KeyResolver = s3KeyResolver;
        }

        public async Task<SendEmailResultResponse> SendEmail(string accountId, string areaId, EmailRequest emailRequest, CancellationToken cancellationToken)
        {
            var responses = criticalResponses.Compile(emailRequest.KeyValues);
            var attachments = await attachmentRetriever.RetrieveAttachmentFiles(accountId, areaId, cancellationToken);

            var culture = await GetCulture(accountId);
            var safeFileNameStem = emailRequest.ConversationId;
            var localFilePath = await pdfResponseGenerator.GeneratePdfResponseAsync(
                responses,
                emailRequest,
                culture,
                tempPathCreator.Create(string.Join(".", safeFileNameStem, "pdf")),
                safeFileNameStem,
                accountId,
                areaId
            );

            if (localFilePath == null)
            {
                throw new Exception("Could not generate PDF Response - check the pdf server is active");
            }

            await SaveResponsePdfToS3(localFilePath, accountId, safeFileNameStem);

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