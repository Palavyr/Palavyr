using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.CloudKeyResolvers;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Services.TemporaryPaths;
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
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IDynamicTableCompilerOrchestrator dynamicTablesCompiler;
        private readonly IConfiguration configuration;

        public ResponsePdfGenerator(
            IEntityStore<Area> intentStore,
            IPdfResponseKeyResolver pdfResponseKeyResolver,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IResponsePdfTableCompiler responsePdfTableCompiler,
            IConfiguration configuration
        )
        {
            this.intentStore = intentStore;
            this.pdfResponseKeyResolver = pdfResponseKeyResolver;
            this.htmlToPdfClient = htmlToPdfClient;
            this.responseHtmlBuilder = responseHtmlBuilder;
            this.responsePdfTableCompiler = responsePdfTableCompiler;
            this.configuration = configuration;
        }

        public async Task<PdfServerResponse> GeneratePdfResponse(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string uniqueId,
            string intentId)
        {
            if (emailRequest.NumIndividuals <= 0) throw new Exception("Num individuals must be 1 or more.");

            var intent = await intentStore.GetIntentOnly(intentId);

            var responseTables = await responsePdfTableCompiler.CompileResponseTables(intentId, emailRequest, culture, intent.IncludeDynamicTableTotals);
            var html = await responseHtmlBuilder.BuildResponseHtml(intentId, criticalResponses, responseTables, emailRequest);

            var userDataBucket = configuration.GetUserDataBucket();
            var s3Key = pdfResponseKeyResolver.Resolve(
                new FileName
                {
                    Extension = ExtensionTypes.Pdf,
                    FileId = emailRequest.ConversationId,
                    FileStem = emailRequest.ConversationId
                });

            var pdfServerResponse = await htmlToPdfClient.GeneratePdfFromHtml(html, userDataBucket, s3Key, uniqueId, Paper.CreateDefault(uniqueId)); // TODO: Make this configurable via the DBs
            return pdfServerResponse;
        }
    }
    
    public interface IResponsePdfTableCompiler
    {
        Task<List<Table>> CompileResponseTables(string intentId, EmailRequest emailRequest, CultureInfo culture, bool includeDynamicTableTotals);
    }

    public class ResponsePdfTableCompiler : IResponsePdfTableCompiler
    {
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IDynamicTableCompilerOrchestrator dynamicTablesCompiler;

        public ResponsePdfTableCompiler(IStaticTableCompiler staticTableCompiler, IDynamicTableCompilerOrchestrator dynamicTablesCompiler)
        {
            this.staticTableCompiler = staticTableCompiler;
            this.dynamicTablesCompiler = dynamicTablesCompiler;
        }

        public async Task<List<Table>> CompileResponseTables(string intentId, EmailRequest emailRequest, CultureInfo culture, bool includeDynamicTableTotals)
        {
            var tables = new List<Table>();
            var staticTables = await staticTableCompiler.CollectStaticTables(intentId, culture, emailRequest.NumIndividuals); // ui always sends a number - 1 or greater.
            var dynamicTables = await dynamicTablesCompiler.CompileTablesToPdfRows(emailRequest.DynamicResponses, culture, includeDynamicTableTotals);

            tables.AddRange(dynamicTables);
            tables.AddRange(staticTables);
            return tables;
        }
    }



    public class ResponsePdfGeneratorUpdateConversationRecordDecorator : IResponsePdfGenerator
    {
        private readonly IResponsePdfGenerator inner;
        private readonly IEntityStore<ConversationRecord> convoRecordStore;

        public ResponsePdfGeneratorUpdateConversationRecordDecorator(IResponsePdfGenerator inner, IEntityStore<ConversationRecord> convoRecordStore)
        {
            this.inner = inner;
            this.convoRecordStore = convoRecordStore;
        }

        public async Task<PdfServerResponse> GeneratePdfResponse(CriticalResponses criticalResponses, EmailRequest emailRequest, CultureInfo culture, string uniqueId, string intentId)
        {
            var response = await inner.GeneratePdfResponse(criticalResponses, emailRequest, culture, uniqueId, intentId);

            var record = await convoRecordStore.GetSingleRecord(emailRequest.ConversationId);
            record.ResponsePdfId = emailRequest.ConversationId;
            await convoRecordStore.Update(record);

            return response;
        }
    }
}