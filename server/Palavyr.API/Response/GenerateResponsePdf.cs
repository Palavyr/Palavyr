﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.API.CommonResponseTypes;
using Palavyr.API.RequestTypes;
using Palavyr.API.Services.EntityServices;
using Palavyr.Common.Aws;
using Palavyr.Common.FileSystem;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.Common.FileSystem.LocalServices;
using Palavyr.Common.GlobalConstants;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.PdfService;
using Palavyr.Services.PdfService.PdfSections.Util;


namespace Palavyr.API.Response
{
    public interface IPdfResponseGenerator
    {
        Task<FileLink> CreatePdfResponsePreviewAsync(CultureInfo culture, string accountId, string areaId);
        Task<FileLink> CreatePdfResponsePreviewAsync(IAmazonS3 s3Client, CultureInfo culture, string accountId, string areaId);
        Task<string> GeneratePdfResponseAsync(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string localWriteToPath,
            string identifier,
            string accountId,
            string areaId
        );
    }

    public class PdfResponseGenerator : IPdfResponseGenerator
    {
        private readonly IConfiguration configuration;
        private readonly DashContext dashContext;
        private static readonly HttpClient Client = new HttpClient();
        private readonly ILogger<PdfResponseGenerator> logger;
        private readonly IAccountDataService accountDataService;
        private readonly IAreaDataService areaDataService;

        public PdfResponseGenerator(
            IConfiguration configuration,
            DashContext dashContext,
            ILogger<PdfResponseGenerator> logger,
            IAccountDataService accountDataService,
            IAreaDataService areaDataService
        )
        {
            this.configuration = configuration;
            this.dashContext = dashContext;
            this.logger = logger;
            this.accountDataService = accountDataService;
            this.areaDataService = areaDataService;
        }

        public async Task<FileLink> CreatePdfResponsePreviewAsync(CultureInfo culture, string accountId, string areaId)
        {
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

            var areaData = areaDataService.GetSingleAreaDataRecursive(accountId, areaId);
            var userAccount = accountDataService.GetUserAccount(accountId);

            logger.LogDebug("-------------CreatePdfResponsePreviewAsync-------------------");
            var criticalResponses = new CriticalResponses(new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>() {{"Very important info", "Crucial response"}},
                new Dictionary<string, string>() {{"An Important Question", "An insightful response"}},
            });

            logger.LogDebug("Attempting to collect table data....");
            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables = await CollectPreviewDynamicTables(areaData, accountId, culture);

            logger.LogDebug($"Generating PDF Html string to send to express server...");
            var html = PdfGenerator.GenerateNewPdf(userAccount, areaData, criticalResponses, staticTables,
                dynamicTables);

            var randomFileName = Guid.NewGuid().ToString();
            var localWriteToPath_PDFPreview = FormFilePath.FormResponsePreviewLocalFilePath(accountId, randomFileName, "pdf");

            logger.LogDebug(
                $"Local path used to save the pdf from express (being sent to the express server: {localWriteToPath_PDFPreview}");

            string fileId;
            try
            {
                fileId = await GeneratePdfFromHtml(html, LocalServices.PdfServiceUrl, localWriteToPath_PDFPreview,
                    randomFileName);
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Failed to convert and write the HTML to PDF using the express server.");
                logger.LogCritical($"Attempted to use url: {LocalServices.PdfServiceUrl}");
                logger.LogCritical($"Encountered Error: {ex.Message}");
                throw new Exception();
            }

            var link = CreatePreviewUrlLink(accountId, fileId);
            var fileLink = FileLink.CreateLink("Preview", link, fileId);
            return fileLink;
        }

        /// <summary>
        /// Overload: Used to create a response pdf and save a temp copy to S3 for creating a presigned URL. Same strat for our pdfs.
        /// </summary>
        /// <param name="s3Client"></param>
        /// <param name="culture"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<FileLink> CreatePdfResponsePreviewAsync(IAmazonS3 s3Client, CultureInfo culture, string accountId, string areaId)
        {
            logger.LogDebug("-------------CreatePdfResponsePreviewAsync-------------------");
            var previewBucket = configuration.GetSection(ConfigSections.PreviewSection).Value;

            var areaData = areaDataService.GetSingleAreaDataRecursive(accountId, areaId);
            var userAccount = accountDataService.GetUserAccount(accountId);

            var criticalResponses = new CriticalResponses(new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>() {{"Example info", "Crucial response"}},
                new Dictionary<string, string>() {{"Selected to Include", "An insightful response"}},
            });

            logger.LogDebug("Attempting to collect table data....");
            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables = await CollectPreviewDynamicTables(areaData, accountId, culture);

            logger.LogDebug($"Generating PDF Html string to send to express server...");
            var html = PdfGenerator.GenerateNewPdf(
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
                await GeneratePdfFromHtml(html, LocalServices.PdfServiceUrl, safeFileNamePath, safeFileNameStem);
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
                link = await UriUtils.CreatePreSignedPreviewUrlLink(logger, accountId, safeFileNameStem,
                    safeFileNamePath, s3Client, previewBucket);
                logger.LogDebug("Successfully created a presigned link to the pdf!");
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Failed to create presigned preview Url Link from S3.");
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

        public async Task<string> GeneratePdfResponseAsync(
            CriticalResponses criticalResponses,
            EmailRequest emailRequest,
            CultureInfo culture,
            string localWriteToPath,
            string identifier,
            string accountId,
            string areaId
        )
        {
            var areaData = areaDataService.GetSingleAreaDataRecursive(accountId, areaId);
            var userAccount = accountDataService.GetUserAccount(accountId);

            var dynamicResponses = emailRequest.DynamicResponses.Count > 0
                ? emailRequest.DynamicResponses
                : new List<Dictionary<string, string>>() {};
            
            var staticTables = CollectStaticTables(areaData, culture);
            var dynamicTables = await CollectRealDynamicTables(accountId, dynamicResponses, culture);

            var html = PdfGenerator.GenerateNewPdf(userAccount, areaData, criticalResponses, staticTables, dynamicTables);

            html = ResponseCustomizer.Customize(html, emailRequest, userAccount);
            
            var fileName = await GeneratePdfFromHtml(html, LocalServices.PdfServiceUrl, localWriteToPath, identifier);
            return fileName;
        }

        private async Task<string> GeneratePdfFromHtml(string htmlString, string serviceUrl, string localWriteToPath, string identifier)
        {
            var values = new Dictionary<string, string>
            {
                {LocalServices.html, htmlString.Trim()},
                {LocalServices.path, localWriteToPath},
                {LocalServices.id, identifier}
            };

            var content = new FormUrlEncodedContent(values);
            var response = await Client.PostAsync(serviceUrl, content);
            var fileName = await response.Content.ReadAsStringAsync();
            return fileName;
        }

        private string CreatePreviewUrlLink(string accountId, string fileId)
        {
            // var host = Request.Host.Value; //TODO:  Get this from config (request.host)
            logger.LogDebug("Attempting to create new file URI for preview.");
            var builder =
                new UriBuilder()
                {
                    // TODO: take these values from the configuration
                    Scheme = "https", Host = "localhost", Port = 5001,
                    Path = Path.Combine(accountId, MagicPathStrings.PreviewPDF, fileId)
                };
            logger.LogDebug($"URI used for the preview: {builder.Uri.ToString()}");
            return builder.Uri.ToString();
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


        /// Only use for the preview (will generate a sensible row result given the type of dynamic table logged in the area table
        private async Task<List<Table>> CollectPreviewDynamicTables(Area data, string accountId, CultureInfo culture)
        {
            // compute dynamic table. Probably have to get the correct table 
            var dynamicTableMetas = data.DynamicTableMetas;

            var rows = new List<TableRow>();
            foreach (var tableMeta in dynamicTableMetas)
            {
                if (tableMeta.TableType == DynamicTableTypes.CreateSelectOneFlat().TableType)
                {
                    var tableRows = await dashContext.SelectOneFlats
                        .Where(row => row.AccountId == accountId && row.AreaIdentifier == data.AreaIdentifier)
                        .ToListAsync();
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
                else
                {
                    continue;
                }
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture);
            return new List<Table>() {table};
        }

        private async Task<List<Table>> CollectRealDynamicTables(
            string accountId,
            List<Dictionary<string, string>> dynamicResponses,
            CultureInfo culture
        )
        {
            var rows = new List<TableRow>();

            foreach (var dynamicResponse in dynamicResponses)
            {
                var dynamicResponseId = dynamicResponse.First().Key;
                var dynamicResponseValue = dynamicResponse.First().Value;

                if (dynamicResponseId.StartsWith(DynamicTableTypes.CreateSelectOneFlat().TableType))
                {
                    var dbRow = await dashContext
                        .SelectOneFlats
                        .Where(tableRow =>
                            tableRow.AccountId == accountId && dynamicResponseId.EndsWith(tableRow.TableId))
                        .SingleOrDefaultAsync(tableRow => tableRow.Option == dynamicResponseValue);

                    var row = new TableRow(
                        dbRow.Option,
                        dbRow.ValueMin,
                        dbRow.ValueMax,
                        false,
                        culture,
                        dbRow.Range);

                    rows.Add(row);
                }
                else if (dynamicResponseId.StartsWith(DynamicTableTypes.CreatePercentOfThreshold().TableType))
                {
                    // need to collect all rows from percent of threshold, then group them by item, then for each item, 
                    // get the base given the threshold, add or subtract the % modifier amount, and associate that with
                    // the item name. So this should potentially add multiple TableRow objects to the rows list.

                    // TODO: extract this into a separate component

                    var responseValueAsDouble = double.Parse(dynamicResponseValue);

                    var allRows =
                        await dashContext
                            .PercentOfThresholds
                            .Where(r => dynamicResponseId.EndsWith(r.TableId))
                            .ToArrayAsync();

                    var itemIds = allRows.Select(item => item.ItemId).Distinct().ToArray();
                    foreach (var itemId in itemIds)
                    {
                        var itemThresholds = allRows.Where(item => item.ItemId == itemId).ToList();
                        itemThresholds.Sort();
                        foreach (var threshold in itemThresholds)
                        {
                            if (responseValueAsDouble >= threshold.Threshold)
                            {
                                var minBaseAmount = threshold.ValueMin;
                                var maxBaseAmount = threshold.ValueMax;

                                if (threshold.PosNeg)
                                {
                                    minBaseAmount += minBaseAmount * (threshold.Modifier / 100);
                                    maxBaseAmount += maxBaseAmount * (threshold.Modifier / 100);
                                }
                                else
                                {
                                    minBaseAmount -= minBaseAmount * (threshold.Modifier / 100);
                                    maxBaseAmount -= maxBaseAmount * (threshold.Modifier / 100);
                                }

                                var tableRow = new TableRow(
                                    threshold.ItemName,
                                    minBaseAmount,
                                    maxBaseAmount,
                                    false,
                                    culture,
                                    threshold.Range
                                );
                                rows.Add(tableRow);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Computing dynamic fee type not yet implemented");
                }
            }

            var table = new Table("Variable estimates determined by your responses", rows, culture);
            return new List<Table>() {table};
        }
    }
}