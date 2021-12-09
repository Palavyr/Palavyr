using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PdfService.PdfServer;
using Palavyr.Core.Services.TemporaryPaths;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponseRetriever
    {
        private readonly ILifetimeScope lifetimeScope;

        public ResponseRetriever(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public async Task<List<TEntity>> RetrieveAllAvailableResponses<TEntity>(string accountId, string dynamicResponseId) where TEntity : class
        {
            var repository = (IGenericDynamicTableRepository<TEntity>) lifetimeScope.Resolve(typeof(IGenericDynamicTableRepository<TEntity>));
            var rows = await repository.GetAllRowsMatchingDynamicResponseId(dynamicResponseId);
            return rows;
        }
    }


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
        private readonly DynamicTableCompilerRetriever compilerRetriever;
        private readonly ResponseRetriever responseRetriever;

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
            ICriticalResponses criticalResponses,
            DynamicTableCompilerRetriever compilerRetriever,
            ResponseRetriever responseRetriever
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
            this.compilerRetriever = compilerRetriever;
            this.responseRetriever = responseRetriever;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(string accountId, string areaId, CultureInfo culture, CancellationToken cancellationToken)
        {
            var previewBucket = configuration.GetPreviewBucket();

            var areaData = await configurationRepository.GetAreaComplete(accountId, areaId);
            var userAccount = await accountRepository.GetAccount(accountId, cancellationToken);

            var fakeResponses = criticalResponses.Compile(
                new List<Dictionary<string, string>>()
                {
                    new Dictionary<string, string>() {{"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam"}},
                    new Dictionary<string, string>() {{"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam"}},
                    new Dictionary<string, string>() {{"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.?", "Ut enim ad minim veniam"}},
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
                dynamicTables,
                new EmailRequest()
                {
                    Name = "John Doe",
                    EmailAddress = "john.doe@example.com",
                    Phone = "555-555-555",
                    ConversationId = Guid.NewGuid().ToString()
                });

            var localTempSafeFile = temporaryPath.CreateLocalTempSafeFile();

            var s3Key = s3KeyResolver.ResolvePreviewKey(accountId, localTempSafeFile.FileStem);
            await htmlToPdfClient.GeneratePdfFromHtml(html, previewBucket, s3Key, localTempSafeFile.FileStem, Paper.CreateDefault(localTempSafeFile.FileStem));

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3Key, previewBucket);
            var fileLink = FileLink.CreateUrlLink("Preview", preSignedUrl, localTempSafeFile.FileStem);
            return fileLink;
        }

        /// Only use for the preview (will generate a sensible row result given the type of dynamic table logged in the area table
        private async Task<List<Table>> CollectPreviewDynamicTables(Area area, string accountId, CultureInfo culture)
        {
            var dynamicTableMetas = area.DynamicTableMetas;

            var rows = new List<TableRow>();
            foreach (var tableMeta in dynamicTableMetas)
            {
                DynamicResponseParts responseParts;
                List<TableRow> currentRows;
                var dynamicCompiler = compilerRetriever.RetrieveCompiler(tableMeta.TableType);

                switch (tableMeta.TableType)
                {
                    case nameof(DynamicTableTypes.BasicThreshold):

                        var availableBasicThreshold = await responseRetriever.RetrieveAllAvailableResponses<BasicThreshold>(accountId, tableMeta.TableId);
                        responseParts = DynamicTableTypes.CreateBasicThreshold().CreateDynamicResponseParts(availableBasicThreshold.First().TableId, availableBasicThreshold.First().Threshold.ToString());
                        currentRows = await dynamicCompiler.CompileToPdfTableRow(area.AccountId, responseParts, new List<string>() {tableMeta.TableId}, culture);
                        break;

                    case nameof(DynamicTableTypes.SelectOneFlat):

                        var availableOneFlat = await responseRetriever.RetrieveAllAvailableResponses<SelectOneFlat>(accountId, tableMeta.TableId);
                        responseParts = DynamicTableTypes.CreateSelectOneFlat().CreateDynamicResponseParts(availableOneFlat.First().TableId, availableOneFlat.First().Option);
                        currentRows = await dynamicCompiler.CompileToPdfTableRow(area.AccountId, responseParts, new List<string>() {tableMeta.TableId}, culture);

                        break;

                    case nameof(DynamicTableTypes.CategoryNestedThreshold):
                        var availableNestedThreshold = await responseRetriever.RetrieveAllAvailableResponses<CategoryNestedThreshold>(accountId, tableMeta.TableId);
                        currentRows = new List<TableRow>()
                        {
                            new TableRow(
                                tableMeta.UseTableTagAsResponseDescription ? tableMeta.TableTag : availableNestedThreshold.First().ItemName,
                                availableNestedThreshold.First().ValueMin,
                                availableNestedThreshold.First().ValueMax,
                                false,
                                culture,
                                availableNestedThreshold.First().Range)
                        };

                        break;

                    case nameof(DynamicTableTypes.PercentOfThreshold):
                        var availablePercentOfThreshold = await responseRetriever.RetrieveAllAvailableResponses<PercentOfThreshold>(accountId, tableMeta.TableId);
                        responseParts = DynamicTableTypes.CreatePercentOfThreshold().CreateDynamicResponseParts(availablePercentOfThreshold.First().TableId, availablePercentOfThreshold.First().Threshold.ToString());
                        currentRows = await dynamicCompiler.CompileToPdfTableRow(area.AccountId, responseParts, new List<string>() {tableMeta.TableId}, culture);
                        break;

                    case nameof(DynamicTableTypes.TwoNestedCategory):

                        var availableTwoNested = await responseRetriever.RetrieveAllAvailableResponses<TwoNestedCategory>(accountId, tableMeta.TableId);
                        currentRows = new List<TableRow>()
                        {
                            new TableRow(
                                tableMeta.UseTableTagAsResponseDescription ? tableMeta.TableTag : string.Join(" & ", new[] {availableTwoNested.First().ItemName, availableTwoNested.First().InnerItemName}),
                                availableTwoNested.First().ValueMin,
                                availableTwoNested.First().ValueMax,
                                false,
                                culture,
                                availableTwoNested.First().Range
                            )
                        };
                        break;

                    default:
                        throw new DomainException("Dynamic Table Type not identified");
                }

                // var currentRows = await dynamicCompiler.CompileToPdfTableRow(area.AccountId, responseParts, new List<string>() {tableMeta.TableId}, culture);
                rows.AddRange(currentRows);
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture);
            return new List<Table>() {table};
        }
    }
}