using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.CloudKeyResolvers;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.PdfService
{
    public class PreviewResponseGenerator : IPreviewResponseGenerator
    {
        private readonly IEntityStore<Intent> intentStore;
        private readonly ILogger<PreviewResponseGenerator> logger;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IResponseHtmlBuilder responseHtmlBuilder;
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IResponsePdfPreviewKeyResolver previewKeyResolver;
        private readonly ITemporaryPath temporaryPath;
        private readonly ICriticalResponses criticalResponses;
        private readonly IPricingStrategyTableCompilerRetriever compilerRetriever;
        private readonly IGuidUtils guidUtils;

        public PreviewResponseGenerator(
            IEntityStore<Intent> intentStore,
            ILogger<PreviewResponseGenerator> logger,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IStaticTableCompiler staticTableCompiler,
            IResponsePdfPreviewKeyResolver previewKeyResolver,
            ITemporaryPath temporaryPath,
            ICriticalResponses criticalResponses,
            IPricingStrategyTableCompilerRetriever compilerRetriever,
            IGuidUtils guidUtils
        )
        {
            this.intentStore = intentStore;
            this.logger = logger;
            this.htmlToPdfClient = htmlToPdfClient;
            this.responseHtmlBuilder = responseHtmlBuilder;
            this.staticTableCompiler = staticTableCompiler;
            this.previewKeyResolver = previewKeyResolver;
            this.temporaryPath = temporaryPath;
            this.criticalResponses = criticalResponses;
            this.compilerRetriever = compilerRetriever;
            this.guidUtils = guidUtils;
        }

        public async Task<FileAsset> CreatePdfResponsePreviewAsync(string intentId, CultureInfo culture)
        {
            var fakeResponses = CreateFakeResponses();

            var tables = await CreatePreviewTables(intentId, culture);
            var uniqueId = $"{ResponsePrefix.Preview}{guidUtils.CreateNewId()}";

            var html = await responseHtmlBuilder.BuildResponseHtml(
                intentId,
                fakeResponses,
                tables,
                new EmailRequest()
                {
                    Name = "John Doe",
                    EmailAddress = "john.doe@example.com",
                    Phone = "555-555-555",
                    ConversationId = uniqueId
                });

            var localTempSafeFile = temporaryPath.CreateLocalTempSafeFile(uniqueId, ExtensionTypes.Pdf);
            var s3Key = previewKeyResolver.Resolve(
                new FileName
                {
                    Extension = ExtensionTypes.Pdf,
                    FileId = uniqueId,
                    FileStem = uniqueId
                });

            var fileAsset = await htmlToPdfClient.GeneratePdfFromHtml(html, s3Key, localTempSafeFile.FileStem, Paper.DefaultOptions(localTempSafeFile.FileStem));
            return fileAsset;
        }

        private CriticalResponses CreateFakeResponses()
        {
            return criticalResponses.Compile(
                new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>() { { "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam" } },
                    new Dictionary<string, string>() { { "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam" } },
                    new Dictionary<string, string>() { { "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam" } },
                });
        }

        private async Task<List<Table>> CreatePreviewTables(string intentId, CultureInfo culture)
        {
            var tables = new List<Table>();
            var staticTables = await staticTableCompiler.CollectStaticTables(intentId, culture, 2); // ui always sends a number - 1 or greater.
            var pricingStrategyTables = await CollectPreviewPricingStrategyTables(intentId, culture);

            tables.AddRange(staticTables);
            tables.AddRange(pricingStrategyTables);
            return tables;
        }

        private async Task<List<Table>> CollectPreviewPricingStrategyTables(string intentId, CultureInfo culture)
        {
            var intent = await intentStore.GetIntentComplete(intentId);
            var pricingStrategyTableMetas = intent.PricingStrategyTableMetas;

            var rows = new List<TableRow>();
            foreach (var tableMeta in pricingStrategyTableMetas)
            {
                var pricingStrategyCompiler = compilerRetriever.RetrieveCompiler(tableMeta.TableType);
                var newRows = await pricingStrategyCompiler.CreatePreviewData(tableMeta, intent, culture);
                rows.AddRange(newRows);
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture, intent.IncludePricingStrategyTableTotals);
            return new List<Table>() { table };
        }
    }
}