using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.CloudKeyResolvers;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponsePdfGenerator : IResponsePdfGenerator
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly IPdfResponseKeyResolver pdfResponseKeyResolver;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IResponseHtmlBuilder responseHtmlBuilder;
        private readonly IResponsePdfTableCompiler responsePdfTableCompiler;

        public ResponsePdfGenerator(
            IEntityStore<Area> intentStore,
            IPdfResponseKeyResolver pdfResponseKeyResolver,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IResponsePdfTableCompiler responsePdfTableCompiler
        )
        {
            this.intentStore = intentStore;
            this.pdfResponseKeyResolver = pdfResponseKeyResolver;
            this.htmlToPdfClient = htmlToPdfClient;
            this.responseHtmlBuilder = responseHtmlBuilder;
            this.responsePdfTableCompiler = responsePdfTableCompiler;
        }

        public async Task<FileAsset> GeneratePdfResponse(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string intentId)
        {
            if (emailRequest.NumIndividuals <= 0) throw new Exception("Num individuals must be 1 or more.");

            var intent = await intentStore.GetIntentOnly(intentId);

            var responseTables = await responsePdfTableCompiler.CompileResponseTables(intentId, emailRequest, culture, intent.IncludeDynamicTableTotals);
            var html = await responseHtmlBuilder.BuildResponseHtml(intentId, criticalResponses, responseTables, emailRequest);

            var fileStem = $"{ResponsePrefix.Palavyr}{emailRequest.ConversationId}";
            var locationKey = pdfResponseKeyResolver.Resolve(
                new FileName
                {
                    Extension = ExtensionTypes.Pdf,
                    FileId = emailRequest.ConversationId,
                    FileStem = fileStem
                });

            var fileAsset = await htmlToPdfClient.GeneratePdfFromHtml(html, locationKey, fileStem, Paper.DefaultOptions(fileStem)); // TODO: Make this configurable via the DBs
            return fileAsset;
        }
    }

    public class ResponsePdfGeneratorUpdateConversationRecordDecorator : IResponsePdfGenerator
    {
        private readonly IResponsePdfGenerator inner;
        private readonly ICancellationTokenTransport cancellationTokenTransport;
        private readonly IUnitOfWorkContextProvider contextProvider;
        private readonly IEntityStore<ConversationRecord> convoRecordStore;
        private readonly IEntityStore<FileAsset> fileAssetStore;
        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;

        public ResponsePdfGeneratorUpdateConversationRecordDecorator(
            IResponsePdfGenerator inner,
            ICancellationTokenTransport cancellationTokenTransport,
            IUnitOfWorkContextProvider contextProvider,
            IEntityStore<ConversationRecord> convoRecordStore,
            IEntityStore<FileAsset> fileAssetStore)
        {
            this.inner = inner;
            this.cancellationTokenTransport = cancellationTokenTransport;
            this.contextProvider = contextProvider;
            this.convoRecordStore = convoRecordStore;
            this.fileAssetStore = fileAssetStore;
        }

        public async Task<FileAsset> GeneratePdfResponse(CriticalResponses criticalResponses, EmailRequest emailRequest, CultureInfo culture, string intentId)
        {
            var fileAsset = await inner.GeneratePdfResponse(criticalResponses, emailRequest, culture, intentId);

            await fileAssetStore.Create(fileAsset);
            await contextProvider.ConfigurationContext().SaveChangesAsync(CancellationToken);

            var record = await convoRecordStore.GetSingleRecord(emailRequest.ConversationId);
            record.ResponsePdfId = fileAsset.FileId;
            await convoRecordStore.Update(record);

            return fileAsset;
        }
    }
}