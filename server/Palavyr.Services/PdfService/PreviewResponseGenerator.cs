using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystemTools.FormPaths;
using Palavyr.Common.FileSystemTools.LocalServices;
using Palavyr.Common.GlobalConstants;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Configuration.Schemas.DynamicTables;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.AmazonServices;
using Palavyr.Services.PdfService.PdfSections.Util;
using Palavyr.Services.Repositories;

namespace Palavyr.Services.PdfService
{
    public class PreviewResponseGenerator : IPreviewResponseGenerator
    {
        private readonly IAmazonS3 s3Client;
        private readonly IConfiguration configuration;
        private readonly ILogger<PreviewResponseGenerator> logger;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IResponseHtmlBuilder responseHtmlBuilder;
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IGenericDynamicTableRepository<SelectOneFlat> genericDynamicTableRepository;

        public PreviewResponseGenerator(
            IAmazonS3 s3Client,
            IConfiguration configuration,
            ILogger<PreviewResponseGenerator> logger,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IStaticTableCompiler staticTableCompiler,
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            IGenericDynamicTableRepository<SelectOneFlat> genericDynamicTableRepository
        )
        {
            this.s3Client = s3Client;
            this.configuration = configuration;
            this.logger = logger;
            this.htmlToPdfClient = htmlToPdfClient;
            this.responseHtmlBuilder = responseHtmlBuilder;
            this.staticTableCompiler = staticTableCompiler;
            this.configurationRepository = configurationRepository;
            this.accountRepository = accountRepository;
            this.genericDynamicTableRepository = genericDynamicTableRepository;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(string accountId, string areaId, CultureInfo culture)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

            var areaData = await configurationRepository.GetAreaComplete(accountId, areaId);
            var userAccount = await accountRepository.GetAccount(accountId);

            var criticalResponses = new CriticalResponses(
                new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>() {{"Example info", "Crucial response"}},
                    new Dictionary<string, string>() {{"Selected to Include", "An insightful response"}},
                });

            logger.LogDebug("Attempting to collect table data....");

            var staticTables = staticTableCompiler.CollectStaticTables(areaData, culture, 2); // ui always sends a number - 1 or greater.
            var dynamicTables = await CollectPreviewDynamicTables(areaData, accountId, culture);

            logger.LogDebug($"Generating PDF Html string to send to express server...");
            var html = responseHtmlBuilder.BuildResponseHtml(
                userAccount,
                areaData,
                criticalResponses,
                staticTables,
                dynamicTables);

            var safeFileNameStem = Guid.NewGuid().ToString();
            var safeFileNamePath = FormFilePath.FormResponsePreviewLocalFilePath(accountId, safeFileNameStem);

            logger.LogDebug(
                $"Attempting to create pdf file from html at {safeFileNamePath} using URL {LocalServices.PdfServiceUrl}");

            try
            {
                await htmlToPdfClient.GeneratePdfFromHtml(html, safeFileNamePath, safeFileNameStem);
                logger.LogDebug($"Successfully wrote the pdf file to disk at {safeFileNamePath}!");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Failed to convert and write the HTML to PDF using the express server.");
                logger.LogCritical($"Attempted to use url: {LocalServices.PdfServiceUrl}");
                logger.LogCritical($"Encountered Error: {ex.Message}");
                throw new Exception();
            }

            string link;
            try
            {
                link = await UriUtils.CreatePreSignedPreviewUrlLink(
                    logger, accountId, safeFileNameStem,
                    safeFileNamePath, s3Client, previewBucket);
                logger.LogDebug("Successfully created a pre-signed link to the pdf!");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Failed to create pre-signed preview Url Link from S3.");
                logger.LogCritical($"Encountered Error: {ex.Message}");
                throw new Exception();
            }

            var fileLink = FileLink.CreateLink("Preview", link, safeFileNamePath);

            if (File.Exists(safeFileNamePath))
            {
                File.SetAttributes(safeFileNamePath, FileAttributes.Normal);
                File.Delete(safeFileNamePath);
                logger.LogDebug($"Deleted local path (currently on S3). Path {safeFileNamePath}");
            }

            return fileLink;
        }


        /// Only use for the preview (will generate a sensible row result given the type of dynamic table logged in the area table
        private async Task<List<Table>> CollectPreviewDynamicTables(Area area, string accountId, CultureInfo culture)
        {
            // compute dynamic table. Probably have to get the correct table 
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