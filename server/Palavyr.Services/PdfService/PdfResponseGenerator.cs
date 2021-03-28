using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Palavyr.Services.DatabaseService;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Services.PdfService.PdfSections.Util;
using Palavyr.Services.DynamicTableService.Compilers;

namespace Palavyr.Services.PdfService
{
    public class PdfResponseGenerator : IPdfResponseGenerator
    {
        private readonly IAccountsConnector accountsConnector;
        private readonly IDashConnector dashConnector;
        private readonly ILogger<PdfResponseGenerator> logger;
        private readonly IHtmlToPdfClient htmlToPdfClient;
        private readonly IResponseHtmlBuilder responseHtmlBuilder;
        private readonly IResponseCustomizer responseCustomizer;
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly SelectOneFlatCompiler selectOneFlatCompiler;
        private readonly PercentOfThresholdCompiler percentOfThresholdCompiler;
        private readonly BasicThresholdCompiler basicThresholdCompiler;

        public PdfResponseGenerator(
            IAccountsConnector accountsConnector,
            IDashConnector dashConnector,
            ILogger<PdfResponseGenerator> logger,
            IHtmlToPdfClient htmlToPdfClient,
            IResponseHtmlBuilder responseHtmlBuilder,
            IResponseCustomizer responseCustomizer,
            IStaticTableCompiler staticTableCompiler,
            SelectOneFlatCompiler selectOneFlatCompiler,
            PercentOfThresholdCompiler percentOfThresholdCompiler,
            BasicThresholdCompiler basicThresholdCompiler
        )
        {
            this.accountsConnector = accountsConnector;
            this.dashConnector = dashConnector;
            this.logger = logger;
            this.htmlToPdfClient = htmlToPdfClient;
            this.responseHtmlBuilder = responseHtmlBuilder;
            this.responseCustomizer = responseCustomizer;
            this.staticTableCompiler = staticTableCompiler;
            this.selectOneFlatCompiler = selectOneFlatCompiler;
            this.percentOfThresholdCompiler = percentOfThresholdCompiler;
            this.basicThresholdCompiler = basicThresholdCompiler;
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
            var areaData = await dashConnector.GetAreaComplete(accountId, areaId);
            var account = await accountsConnector.GetAccount(accountId);

            if (emailRequest.NumIndividuals <= 0) throw new Exception("Num individuals must be 1 or more.");
            var staticTables = staticTableCompiler.CollectStaticTables(areaData, culture, emailRequest.NumIndividuals); // ui always sends a number - 1 or greater.
            var dynamicTables = await CollectRealDynamicTables(accountId, emailRequest.DynamicResponses, culture);

            var html = responseHtmlBuilder.BuildResponseHtml(account, areaData, criticalResponses, staticTables, dynamicTables);

            html = responseCustomizer.Customize(html, emailRequest, account);

            var fileName = await htmlToPdfClient.GeneratePdfFromHtml(html, localWriteToPath, identifier);
            return fileName;
        }

        private async Task<List<Table>> CollectRealDynamicTables(
            string accountId,
            List<Dictionary<string, DynamicResponse>> dynamicResponses,
            CultureInfo culture
        )
        {
            var tableRows = new List<TableRow>();

            /*
             * Dynamic responses
             * 
             *  // [
                //     {
                //         "DynamicTableKey?": [
                //             {[node.nodeType]: "Response Value"},
                //             {[node.nodeType]: "Response Value"},
                //             {[node.nodeType]: "Response Value"}
                //         ]
                //     },
                //     {
                //         "SelectOneFlat-1231": [
                //             {"SelectOneFlat-1231": "Ruby"}
                //         ]
                //     }
                // ]
             */

            foreach (var dynamicResponse in dynamicResponses)
            {
                /*
                 * Dynamic response
                //     {
                //         "DynamicTableKey?": [
                //             {[node.nodeType]: "Response Value"},
                //             {[node.nodeType]: "Response Value"},
                //             {[node.nodeType]: "Response Value"}
                //         ]
                //     }
                 */


                // var dynamicResponseId = dynamicResponse.First().Key;
                // var dynamicResponseValue = dynamicResponse.First().Value;

                // I'll need to map now an array of dynamic responses. Could check lengths here
                // if its length 1, then just grab the first, but if its more than one...
                // The response ID is currently like SelectOneFlat-325324
                // the future will be ["SelectOneFlat-32234", "SelectOneFlat-65656"]. Need to generalize to more than one.


                // DynamicTableKey

                var dynamicTableKeys = dynamicResponse.Keys.ToList(); // in the future, there could be multiple key values
                // for now, we are only expecting one key. Not sure yet how we can combine multiple tables or if there is even a point to doing this.
                if (dynamicTableKeys.Count > 1) throw new Exception("Multiple dynamic table keys specified. This is a configuration error");
                var dynamicTableKey = dynamicTableKeys[0];
                var responses = dynamicResponse[dynamicTableKey];

                List<TableRow> rows;
                if (dynamicTableKey.StartsWith(DynamicTableTypes.CreateSelectOneFlat().TableType))
                {
                    rows = await selectOneFlatCompiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
                }
                else if (dynamicTableKey.StartsWith(DynamicTableTypes.CreatePercentOfThreshold().TableType))
                {
                    rows = await percentOfThresholdCompiler.CompileToPdfTableRow(accountId, responses,dynamicTableKeys, culture);
                }
                else if (dynamicTableKey.StartsWith(DynamicTableTypes.CreateBasicThreshold().TableType))
                {
                    rows = await basicThresholdCompiler.CompileToPdfTableRow(accountId, responses,  dynamicTableKeys, culture);
                }
                else
                {
                    throw new Exception("Computing dynamic fee type not yet implemented");
                }

                tableRows.AddRange(rows);
            }

            var table = new Table("Variable estimates determined by your responses", tableRows, culture);
            return new List<Table>() {table};
        }
    }
}