using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.PdfService
{
    public class PreviewResponseGenerator : IPreviewResponseGenerator
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PreviewResponseGenerator> logger;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IResponseHtmlBuilder responseHtmlBuilder;
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;
        private readonly ILinkCreator linkCreator;
        private readonly IGenericDynamicTableRepository<SelectOneFlat> genericDynamicTableRepository;
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly ITemporaryPath temporaryPath;
        private readonly ICriticalResponses criticalResponses;

        public PreviewResponseGenerator(
            IConfiguration configuration,
            ILogger<PreviewResponseGenerator> logger,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IStaticTableCompiler staticTableCompiler,
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            ILinkCreator linkCreator,
            IGenericDynamicTableRepository<SelectOneFlat> genericDynamicTableRepository,
            IS3KeyResolver s3KeyResolver,
            ITemporaryPath temporaryPath,
            ICriticalResponses criticalResponses
        )
        {
            this.configuration = configuration;
            this.logger = logger;
            this.htmlToPdfClient = htmlToPdfClient;
            this.responseHtmlBuilder = responseHtmlBuilder;
            this.staticTableCompiler = staticTableCompiler;
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
            this.linkCreator = linkCreator;
            this.genericDynamicTableRepository = genericDynamicTableRepository;
            this.s3KeyResolver = s3KeyResolver;
            this.temporaryPath = temporaryPath;
            this.criticalResponses = criticalResponses;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(string accountId, string areaId, CultureInfo culture, CancellationToken cancellationToken)
        {
            var previewBucket = configuration.GetPreviewBucket();

            var areaData = await configurationRepository.GetAreaComplete(accountId, areaId);
            var userAccount = await accountRepository.GetAccount(accountId, cancellationToken);

            var fakeResponses = criticalResponses.Compile(
                new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>() {{"Example info", "Crucial response"}},
                    new Dictionary<string, string>() {{"Selected to Include", "An insightful response"}},
                });

            logger.LogDebug("Attempting to collect table data....");

            var staticTables = await staticTableCompiler.CollectStaticTables(accountId, areaData, culture, 2, cancellationToken); // ui always sends a number - 1 or greater.
            var dynamicTables = await CollectPreviewDynamicTables(areaData, accountId, culture);

            logger.LogDebug($"Generating PDF Html string to send to express server...");
            var html = responseHtmlBuilder.BuildResponseHtml(
                userAccount,
                areaData,
                fakeResponses,
                staticTables,
                dynamicTables);

            var localTempSafeFile = temporaryPath.CreateLocalTempSafeFile();

            var s3Key = s3KeyResolver.ResolvePreviewKey(accountId, localTempSafeFile.FileStem);
            await htmlToPdfClient.GeneratePdfFromHtml(html, previewBucket, s3Key, localTempSafeFile.FileStem, Paper.CreateDefault(localTempSafeFile.FileStem));

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3Key, previewBucket);
            var fileLink = FileLink.CreateLink("Preview", preSignedUrl, localTempSafeFile.FileStem);
            return fileLink;
        }

        /// Only use for the preview (will generate a sensible row result given the type of dynamic table logged in the area table
        private async Task<List<Table>> CollectPreviewDynamicTables(Area area, string accountId, CultureInfo culture)
        {
            var dynamicTableMetas = area.DynamicTableMetas;

            var rows = new List<TableRow>();
            foreach (var tableMeta in dynamicTableMetas)
            {
                if (tableMeta.TableType == DynamicTableTypes.CreateSelectOneFlat().TableType)
                {
                    var tableRows = await genericDynamicTableRepository.GetAllRows(accountId, area.AreaIdentifier);

                    var randomTableRow = tableRows[0]; // TODO: allow this to be specified via frontend
                    const bool perPerson = false; // TODO: Allow to specify true
                    var row = new TableRow(
                        randomTableRow.Option,
                        randomTableRow.ValueMin,
                        randomTableRow.ValueMax,
                        perPerson,
                        culture,
                        randomTableRow.Range);

                    rows.Add(row);
                }
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture);
            return new List<Table>() {table};
        }
    }
}