using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponsePdfTableCompiler : IResponsePdfTableCompiler
    {
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IPricingStrategyTableCompilerOrchestrator pricingStrategyTablesCompiler;

        public ResponsePdfTableCompiler(IStaticTableCompiler staticTableCompiler, IPricingStrategyTableCompilerOrchestrator pricingStrategyTablesCompiler)
        {
            this.staticTableCompiler = staticTableCompiler;
            this.pricingStrategyTablesCompiler = pricingStrategyTablesCompiler;
        }

        public async Task<List<Table>> CompileResponseTables(string intentId, EmailRequest emailRequest, CultureInfo culture, bool includeDynamicTableTotals)
        {
            var tables = new List<Table>(); // order matters here
            var staticTables = await staticTableCompiler.CollectStaticTables(intentId, culture, emailRequest.NumIndividuals); // ui always sends a number - 1 or greater.
            var dynamicTables = await pricingStrategyTablesCompiler.CompileTablesToPdfRows(emailRequest.DynamicResponses, culture, includeDynamicTableTotals);

            tables.AddRange(dynamicTables);
            tables.AddRange(staticTables);
            return tables;
        }
    }
}