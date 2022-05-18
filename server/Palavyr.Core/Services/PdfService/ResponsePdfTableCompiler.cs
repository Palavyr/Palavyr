using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.PdfService
{
    public class ResponsePdfTableCompiler : IResponsePdfTableCompiler
    {
        private readonly IStaticTableCompiler staticTableCompiler;
        private readonly IDynamicTableCompilerOrchestrator dynamicTablesCompiler;

        public ResponsePdfTableCompiler(IStaticTableCompiler staticTableCompiler, IDynamicTableCompilerOrchestrator dynamicTablesCompiler)
        {
            this.staticTableCompiler = staticTableCompiler;
            this.dynamicTablesCompiler = dynamicTablesCompiler;
        }

        public async Task<List<Table>> CompileResponseTables(string intentId, EmailRequest emailRequest, CultureInfo culture, bool includeDynamicTableTotals)
        {
            var tables = new List<Table>(); // order matters here
            var staticTables = await staticTableCompiler.CollectStaticTables(intentId, culture, emailRequest.NumIndividuals); // ui always sends a number - 1 or greater.
            var dynamicTables = await dynamicTablesCompiler.CompileTablesToPdfRows(emailRequest.DynamicResponses, culture, includeDynamicTableTotals);

            tables.AddRange(dynamicTables);
            tables.AddRange(staticTables);
            return tables;
        }
    }
}