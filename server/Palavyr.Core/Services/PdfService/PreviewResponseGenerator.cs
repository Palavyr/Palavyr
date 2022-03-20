using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;
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
    public class PreviewResponseGenerator : IPreviewResponseGenerator
    {
        private readonly IEntityStore<Area> intentStore;
        private readonly IConfiguration configuration;
        private readonly ILogger<PreviewResponseGenerator> logger;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IResponseHtmlBuilder responseHtmlBuilder;
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IResponsePdfPreviewKeyResolver previewKeyResolver;
        private readonly ITemporaryPath temporaryPath;
        private readonly ICriticalResponses criticalResponses;
        private readonly IDynamicTableCompilerRetriever compilerRetriever;
        private readonly IGuidUtils guidUtils;

        public PreviewResponseGenerator(
            IEntityStore<Area> intentStore,
            IConfiguration configuration,
            ILogger<PreviewResponseGenerator> logger,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IStaticTableCompiler staticTableCompiler,
            IResponsePdfPreviewKeyResolver previewKeyResolver,
            ITemporaryPath temporaryPath,
            ICriticalResponses criticalResponses,
            IDynamicTableCompilerRetriever compilerRetriever,
            IGuidUtils guidUtils
        )
        {
            this.intentStore = intentStore;
            this.configuration = configuration;
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
            var uniqueId = $"Preview-{guidUtils.CreateNewId()}";

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
            var dynamicTables = await CollectPreviewDynamicTables(intentId, culture);

            tables.AddRange(staticTables);
            tables.AddRange(dynamicTables);
            return tables;
        }

        private async Task<List<Table>> CollectPreviewDynamicTables(string intentId, CultureInfo culture)
        {
            var intent = await intentStore.GetIntentComplete(intentId);
            var dynamicTableMetas = intent.DynamicTableMetas;

            var rows = new List<TableRow>();
            foreach (var tableMeta in dynamicTableMetas)
            {
                // TODO: These need to retrieve the INTERFACE Yo
                var dynamicCompiler = compilerRetriever.RetrieveCompiler(tableMeta.TableType);
                var newRows = await dynamicCompiler.CreatePreviewData(tableMeta, intent, culture);
                rows.AddRange(newRows);
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture, intent.IncludeDynamicTableTotals);
            return new List<Table>() { table };
        }
    }
}