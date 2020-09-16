using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.API.chatUtils;
using DashboardServer.API.pathUtils;
using DashboardServer.API.receiverTypes;
using DashboardServer.API.ResponseTypes;
using DashboardServer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PDFService;
using Server.Domain;
using Server.Domain.AccountDB;
using Microsoft.Extensions.Logging;
using Palavyr.FileSystem;

namespace DashboardServer.API.GeneratePdf
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

        private const string PdfServiceUrl = "http://localhost:5600/create-pdf"; //

        public PdfResponseGenerator(DashContext dashContext, AccountsContext accountContext,
            ConvoContext dynamicTableContext, string accountId, string areaId, HttpRequest request)
        {
            DashContext = dashContext;
            AccountContext = accountContext;
            DynamicTablesContext = dynamicTableContext;
            AccountId = accountId;
            AreaId = areaId;
            Request = request;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(CultureInfo culture)
        {
            var areaData = GetDeepAreaData(); // This was fucking stupid. Impossible to test.
            var userAccount = GetUserAccount();
            var accountId = userAccount.AccountId;
            
            var criticalResponses = new CriticalResponses(new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>() {{"Very important info", "Crucial response"}},
                new Dictionary<string, string>() {{"An Important Question", "An insightful response"}},
            });
            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables = CollectPreviewDynamicTables(areaData, AccountId, culture);

            var html = PdfGenerator.GenerateNewPDF(userAccount, areaData, criticalResponses, staticTables,
                dynamicTables);

            var randomFileName = Guid.NewGuid().ToString();
            var localWriteToPath_PDFPreview = PathFormUtils.FormFullResponsePreviewLocalPath(accountId, randomFileName, "pdf");
            var fileId = await GeneratePdfFromHtml(html, PdfServiceUrl, localWriteToPath_PDFPreview, randomFileName);

            var link = CreatePreviewUrlLink(AccountId, fileId);
            var fileLink = FileLink.CreateLink("Preview", link, fileId);
            return fileLink;
        }

        /// <summary>
        /// Overload: Used to create a response pdf and save a temp copy to S3 for creating a presigned URL. Same strat for our pdfs.
        /// </summary>
        /// <param name="s3Client"></param>
        /// <returns></returns>
        public async Task<FileLink> CreatePdfResponsePreviewAsync(
            IAmazonS3 s3Client,
            ILogger _logger,
            CultureInfo culture)
        {
            var areaData = GetDeepAreaData(); // This was fucking stupid. Impossible to test.
            var userAccount = GetUserAccount();
            var accountId = userAccount.AccountId;
            
            var criticalResponses = new CriticalResponses(new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>() {{"Example info", "Crucial response"}},
                new Dictionary<string, string>() {{"Selected to Include", "An insightful response"}},
            });
            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables = CollectPreviewDynamicTables(areaData, AccountId, culture);

            var html = PdfGenerator.GenerateNewPDF(
                userAccount,
                areaData,
                criticalResponses,
                staticTables,
                dynamicTables);
            
            var randomFileName = Guid.NewGuid().ToString();
            var localWriteToPath_PDFPreview = PathFormUtils.FormFullResponsePreviewLocalPath(accountId, randomFileName, "pdf");
            await GeneratePdfFromHtml(html, PdfServiceUrl, localWriteToPath_PDFPreview, randomFileName);

            var link = await UriUtils.CreatePreSignedPreviewUrlLink(_logger, AccountId, localWriteToPath_PDFPreview, s3Client);
            var fileLink = FileLink.CreateLink("Preview", link, localWriteToPath_PDFPreview);

            if (File.Exists(localWriteToPath_PDFPreview))
            {
                File.Delete(localWriteToPath_PDFPreview);
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
            
            var fileName = await GeneratePdfFromHtml(html, PdfServiceUrl, localWriteToPath, identifier);
            return fileName;
        }

        private async Task<string> GeneratePdfFromHtml(string htmlString, string serviceUrl, string localWriteToPath, string identifier)
        {
            var values = new Dictionary<string, string>
            {
                {"html", htmlString.Trim()},
                {"path", localWriteToPath},
                {"Id", identifier}
            };
            
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(Path.Combine(serviceUrl, AccountId), content);
            var fileName = await response.Content.ReadAsStringAsync();
            return fileName;
        }

        private static string CreatePreviewUrlLink(string accountId, string fileId)
        {
            // var host = Request.Host.Value; //TODO:  Get this from config (request.host)
            var builder =
                new UriBuilder()
                {
                    // TODO: take these values from the environment
                    Scheme = "https", Host = "localhost", Port = 5001,
                    Path = Path.Combine(accountId, MagicString.PreviewPDF, fileId)
                };
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
                            .Where(row => row.AccountId == accountId && row.AreaId == data.AreaIdentifier).ToList();
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
                            .Where(row => row.AccountId == accountId && row.AreaId == data.AreaIdentifier)
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