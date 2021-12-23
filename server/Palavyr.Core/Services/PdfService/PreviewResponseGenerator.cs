using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Sessions;

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
        private readonly IS3KeyResolver s3KeyResolver;
        private readonly ITemporaryPath temporaryPath;
        private readonly ICriticalResponses criticalResponses;
        private readonly DynamicTableCompilerRetriever compilerRetriever;
        private readonly IHoldAnAccountId accountIdHolder;

        public PreviewResponseGenerator(
            IConfiguration configuration,
            ILogger<PreviewResponseGenerator> logger,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IStaticTableCompiler staticTableCompiler,
            IConfigurationRepository configurationRepository,
            IAccountRepository accountRepository,
            ILinkCreator linkCreator,
            IS3KeyResolver s3KeyResolver,
            ITemporaryPath temporaryPath,
            ICriticalResponses criticalResponses,
            DynamicTableCompilerRetriever compilerRetriever,
            IHoldAnAccountId accountIdHolder
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
            this.s3KeyResolver = s3KeyResolver;
            this.temporaryPath = temporaryPath;
            this.criticalResponses = criticalResponses;
            this.compilerRetriever = compilerRetriever;
            this.accountIdHolder = accountIdHolder;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(string areaId, CultureInfo culture)
        {
            var previewBucket = configuration.GetPreviewBucket();

            var areaData = await configurationRepository.GetAreaComplete(areaId);
            var userAccount = await accountRepository.GetAccount();

            var fakeResponses = criticalResponses.Compile(
                new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>() {{"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam"}},
                    new Dictionary<string, string>() {{"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam"}},
                    new Dictionary<string, string>() {{"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam"}},
                });

            logger.LogDebug("Attempting to collect table data....");

            var staticTables = await staticTableCompiler.CollectStaticTables(areaData, culture, 2); // ui always sends a number - 1 or greater.
            var dynamicTables = await CollectPreviewDynamicTables(areaData, culture);

            logger.LogDebug($"Generating PDF Html string to send to express server...");
            var html = responseHtmlBuilder.BuildResponseHtml(
                userAccount,
                areaData,
                fakeResponses,
                staticTables,
                dynamicTables,
                new EmailRequest()
                {
                    Name = "John Doe",
                    EmailAddress = "john.doe@example.com",
                    Phone = "555-555-555",
                    ConversationId = Guid.NewGuid().ToString()
                });

            var localTempSafeFile = temporaryPath.CreateLocalTempSafeFile();

            var s3Key = s3KeyResolver.ResolvePreviewKey(localTempSafeFile.FileStem);
            await htmlToPdfClient.GeneratePdfFromHtml(html, previewBucket, s3Key, localTempSafeFile.FileStem, Paper.CreateDefault(localTempSafeFile.FileStem));

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3Key, previewBucket);
            var fileLink = FileLink.CreateUrlLink("Preview", preSignedUrl, localTempSafeFile.FileStem);
            return fileLink;
        }

        private async Task<List<Table>> CollectPreviewDynamicTables(Area area, CultureInfo culture
        )
        {
            var dynamicTableMetas = area.DynamicTableMetas;

            var rows = new List<TableRow>();
            foreach (var tableMeta in dynamicTableMetas)
            {
                var dynamicCompiler = compilerRetriever.RetrieveCompiler(tableMeta.TableType);
                var newRows = await dynamicCompiler.CreatePreviewData(tableMeta, area, culture);
                rows.AddRange(newRows);
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture, area.IncludeDynamicTableTotals);
            return new List<Table>() {table};
        }
    }
}