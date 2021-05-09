using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.GlobalConstants;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

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
        private readonly IS3Saver s3Saver;
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly ITempPathCreator tempPathCreator;
        private readonly ILocalFileDeleter localFileDeleter;
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
            IS3Saver s3Saver,
            IS3KeyResolver s3KeyResolver,
            ITempPathCreator tempPathCreator,
            ILocalFileDeleter localFileDeleter,
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
            this.s3Saver = s3Saver;
            this.s3KeyResolver = s3KeyResolver;
            this.tempPathCreator = tempPathCreator;
            this.localFileDeleter = localFileDeleter;
            this.criticalResponses = criticalResponses;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(string accountId, string areaId, CultureInfo culture, CancellationToken cancellationToken)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

            var areaData = await configurationRepository.GetAreaComplete(accountId, areaId);
            var userAccount = await accountRepository.GetAccount(accountId, cancellationToken);

            var fakeResponses = this.criticalResponses.Compile(
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
                fakeResponses,
                staticTables,
                dynamicTables);

            var safeFileNameStem = GuidUtils.CreateNewId();
            var localTempPath = tempPathCreator.Create(string.Join(".", safeFileNameStem, "pdf"));

            var tempLocalFilePdfPath = await htmlToPdfClient.GeneratePdfFromHtmlOrNull(html, localTempPath, safeFileNameStem);
            if (tempLocalFilePdfPath == null)
            {
                localFileDeleter.Delete(localTempPath);
                throw new Exception("Unable to generate PDF from html");
            }
            
            var s3Key = s3KeyResolver.ResolvePreviewKey(accountId, safeFileNameStem);
            var success = await s3Saver.SaveObjectToS3(previewBucket, tempLocalFilePdfPath, s3Key);
            if (!success)
            {
                throw new Exception("Unable to save object to s3.");
            }
            else
            {
                localFileDeleter.Delete(tempLocalFilePdfPath);
            }
            
            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3Key, previewBucket);
            var fileLink = FileLink.CreateLink("Preview", preSignedUrl, safeFileNameStem);
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