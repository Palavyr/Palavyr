using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.API.chatUtils;
using DashboardServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.receiverTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.Common.FileSystem.FormPaths.LocalServices;
using PDFService;
using Server.Domain.Accounts;
using Server.Domain.Configuration.constants;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.response
{
    public class PdfResponseGenerator
    {
        private DashContext DashContext { get; set; }
        private ConvoContext DynamicTablesContext { get; set; }
        private AccountsContext AccountContext { get; set; }
        private string AccountId { get; set; }
        private string AreaId { get; set; }
        private static readonly HttpClient client = new HttpClient();
        private HttpRequest Request { get; set; }
        private readonly ILogger _logger;

        public PdfResponseGenerator(DashContext dashContext, AccountsContext accountContext,
            ConvoContext dynamicTableContext, string accountId, string areaId, HttpRequest request, ILogger logger)
        {
            DashContext = dashContext;
            AccountContext = accountContext;
            DynamicTablesContext = dynamicTableContext;
            AccountId = accountId;
            AreaId = areaId;
            Request = request;
            _logger = logger;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(CultureInfo culture)
        {
            var areaData = GetDeepAreaData(); // This was fucking stupid. Impossible to test.
            var userAccount = GetUserAccount();
            var accountId = userAccount.AccountId;
            _logger.LogDebug("-------------CreatePdfResponsePreviewAsync-------------------");
            var criticalResponses = new CriticalResponses(new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>() {{"Very important info", "Crucial response"}},
                new Dictionary<string, string>() {{"An Important Question", "An insightful response"}},
            });

            _logger.LogDebug("Attempting to collect table data....");
            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables = CollectPreviewDynamicTables(areaData, AccountId, culture);

            _logger.LogDebug($"Generating PDF Html string to send to express server...");
            var html = PdfGenerator.GenerateNewPDF(userAccount, areaData, criticalResponses, staticTables,
                dynamicTables);

            var randomFileName = Guid.NewGuid().ToString();
            var localWriteToPath_PDFPreview =
                FormFilePath.FormResponsePreviewLocalFilePath(accountId, randomFileName, "pdf");

            _logger.LogDebug(
                $"Local path used to save the pdf from express (being sent to the express server: {localWriteToPath_PDFPreview}");

            string fileId;
            try
            {
                fileId = await GeneratePdfFromHtml(html, LocalServices.PdfServiceUrl, localWriteToPath_PDFPreview,
                    randomFileName);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to convert and write the HTML to PDF using the express server.");
                _logger.LogCritical($"Attempted to use url: {LocalServices.PdfServiceUrl}");
                _logger.LogCritical($"Encountered Error: {ex.Message}");
                throw new Exception();
            }

            var link = CreatePreviewUrlLink(AccountId, fileId);
            var fileLink = FileLink.CreateLink("Preview", link, fileId);
            return fileLink;
        }

        /// <summary>
        /// Overload: Used to create a response pdf and save a temp copy to S3 for creating a presigned URL. Same strat for our pdfs.
        /// </summary>
        /// <param name="s3Client"></param>
        /// <returns></returns>
        public async Task<FileLink> CreatePdfResponsePreviewAsync(IAmazonS3 s3Client, CultureInfo culture)
        {
            _logger.LogDebug("-------------CreatePdfResponsePreviewAsync-------------------");

            var areaData = GetDeepAreaData(); // This was fucking stupid. Impossible to test.
            var userAccount = GetUserAccount();
            var accountId = userAccount.AccountId;

            var criticalResponses = new CriticalResponses(new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>() {{"Example info", "Crucial response"}},
                new Dictionary<string, string>() {{"Selected to Include", "An insightful response"}},
            });

            _logger.LogDebug("Attempting to collect table data....");
            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables = CollectPreviewDynamicTables(areaData, AccountId, culture);

            _logger.LogDebug($"Generating PDF Html string to send to express server...");
            var html = PdfGenerator.GenerateNewPDF(
                userAccount,
                areaData,
                criticalResponses,
                staticTables,
                dynamicTables);

            var safeFileNameStem = Guid.NewGuid().ToString();
            var safeFileNamePath = FormFilePath.FormResponsePreviewLocalFilePath(accountId, safeFileNameStem);

            _logger.LogDebug(
                $"Attempting to create pdf file from html at {safeFileNamePath} using URL {LocalServices.PdfServiceUrl}");

            try
            {
                await GeneratePdfFromHtml(html, LocalServices.PdfServiceUrl, safeFileNamePath, safeFileNameStem);
                _logger.LogDebug($"Successfully wrote the pdf file to disk at {safeFileNamePath}!");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to convert and write the HTML to PDF using the express server.");
                _logger.LogCritical($"Attempted to use url: {LocalServices.PdfServiceUrl}");
                _logger.LogCritical($"Encountered Error: {ex.Message}");
                throw new Exception();
                
            }

            string link;
            try
            {
                link = await UriUtils.CreatePreSignedPreviewUrlLink(_logger, AccountId, safeFileNameStem,
                    safeFileNamePath, s3Client);
                _logger.LogDebug("Successfully created a presigned link to the pdf!");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to create presigned preview Url Link from S3.");
                _logger.LogCritical($"Encountered Error: {ex.Message}");
                throw new Exception();
            }

            var fileLink = FileLink.CreateLink("Preview", link, safeFileNamePath);

            if (File.Exists(safeFileNamePath))
            {
                File.SetAttributes(safeFileNamePath, FileAttributes.Normal);
                File.Delete(safeFileNamePath);
                _logger.LogDebug($"Deleted local path (currently on S3). Path {safeFileNamePath}");
            }

            return fileLink;
        }

        public async Task<string> GeneratePdfResponseAsync(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string localWriteToPath,
            string identifier
        )
        {
            var areaData = GetDeepAreaData();
            var userAccount = GetUserAccount();

            // TODO: Handle Multiple Dynamic Responses
            var dynamicResponse = emailRequest.DynamicResponse.Count > 0
                ? emailRequest.DynamicResponse[0]
                : new Dictionary<string, string>() { };

            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables =
                CollectRealDynamicTables(areaData, AccountId, dynamicResponse, culture); // TODO Support  multiple
            var html = PdfGenerator.GenerateNewPDF(userAccount, areaData, criticalResponses, staticTables,
                dynamicTables);
            
            // Substitute Variables
            var nameElement = emailRequest.KeyValues.SingleOrDefault(dict => dict.ContainsKey("Name"));
            nameElement.TryGetValue("Name", out var clientName);
            var companyName = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == AccountId).CompanyName;
            var logoUri = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == AccountId).AccountLogoUri;

            html = SupportedSubstitutions.MakeVariableSubstitutions(html, companyName, clientName, logoUri);
            
            var fileName = await GeneratePdfFromHtml(html, LocalServices.PdfServiceUrl, localWriteToPath, identifier);
            return fileName;
        }

        private async Task<string> GeneratePdfFromHtml(string htmlString, string serviceUrl, string localWriteToPath,
            string identifier)
        {
            var values = new Dictionary<string, string>
            {
                {LocalServices.html, htmlString.Trim()},
                {LocalServices.path, localWriteToPath},
                {LocalServices.id, identifier}
            };

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(serviceUrl, content);
            var fileName = await response.Content.ReadAsStringAsync();
            return fileName;
        }

        private string CreatePreviewUrlLink(string accountId, string fileId)
        {
            // var host = Request.Host.Value; //TODO:  Get this from config (request.host)
            _logger.LogDebug("Attempting to create new file URI for preview.");
            var builder =
                new UriBuilder()
                {
                    // TODO: take these values from the environment
                    Scheme = "https", Host = "localhost", Port = 5001,
                    Path = Path.Combine(accountId, MagicPathStrings.PreviewPDF, fileId)
                };
            _logger.LogDebug($"URI used for the preview: {builder.Uri.ToString()}");
            return builder.Uri.ToString();
        }

        private Area GetDeepAreaData()
        {
            var areaData = DashContext.Areas
                .Where(row => row.AccountId == AccountId)
                .Include(row => row.ConversationNodes)
                .Include(row => row.DynamicTableMetas)
                .Include(row => row.StaticTablesMetas)
                .ThenInclude(meta => meta.StaticTableRows)
                .ThenInclude(row => row.Fee)
                .Single(row => row.AreaIdentifier == AreaId);
            return areaData;
        }

        private UserAccount GetUserAccount()
        {
            var userAccount = AccountContext.Accounts.Single(row => row.AccountId == AccountId);
            return userAccount;
        }

        private static List<Table> CollectStaticTables(Area areaData, CultureInfo culture)
        {
            var tables = new List<Table>();

            var tableMetas = areaData.StaticTablesMetas;
            foreach (var meta in tableMetas)
            {
                var rows = new List<TableRow>();
                foreach (var dbRow in meta.StaticTableRows)
                {
                    var row = new TableRow(
                        dbRow.Description,
                        dbRow.Fee.Min,
                        dbRow.Fee.Max,
                        dbRow.PerPerson,
                        culture,
                        dbRow.Range);
                    rows.Add(row);
                }

                var table = new Table(meta.Description, rows, culture);
                tables.Add(table);
            }

            return tables;
        }

        /// <summary>
        /// Only use for the preview (will generate a sensible row result given the type of dynamic table logged in the area table
        /// </summary>
        /// <param name="data"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private List<Table> CollectPreviewDynamicTables(Area data, string accountId, CultureInfo culture)
        {
            // compute dynamic table. Probably have to get the correct table 
            var dynamicTableMetas = data.DynamicTableMetas;

            var rows = new List<TableRow>();
            foreach (var tableMeta in dynamicTableMetas)
            {
                switch (tableMeta.TableType)
                {
                    case DynamicTableTypes.SelectOneFlat:
                        var tableRows = DashContext.SelectOneFlats
                            .Where(row => row.AccountId == accountId && row.AreaIdentifier == data.AreaIdentifier)
                            .ToList();
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

                        break;

                    default:
                        continue;
                }
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture);
            return new List<Table>() {table};
        }

        private List<Table> CollectRealDynamicTables(Area data, string accountId,
            Dictionary<string, string> selectedOption, CultureInfo culture)
        {
            var dynamicTables = data.DynamicTableMetas;

            // if (selectedOption.Keys.Single() != DynamicType) throw new Exception();
            // if (selectedOption.Keys.Single() == null && DynamicType != DynamicTableTypes.None) throw new Exception(); // dangerous (depends on widget sending through selection correctly)

            var rows = new List<TableRow>();

            foreach (var dynamicTable in dynamicTables)
            {
                switch (dynamicTable.TableType)
                {
                    case DynamicTableTypes.SelectOneFlat:
                        var dbRow = DashContext
                            .SelectOneFlats
                            .Where(row => row.AccountId == accountId && row.AreaIdentifier == data.AreaIdentifier)
                            .Single(row =>
                                row.Option ==
                                selectedOption.Values
                                    .Single()); // TODO: Pass a result object, bc not all selected options will be strings.

                        var row = new TableRow(
                            dbRow.Option,
                            dbRow.ValueMin,
                            dbRow.ValueMax,
                            false,
                            culture,
                            dbRow.Range);

                        rows.Add(row);
                        break;

                    default:
                        continue;
                }
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture);
            return new List<Table>() {table};
        }
    }
}