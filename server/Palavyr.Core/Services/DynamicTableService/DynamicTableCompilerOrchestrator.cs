using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicTableCompilerOrchestrator : IDynamicTableCompilerOrchestrator
    {
        private readonly DynamicTableCompilerRetriever dynamicTableCompilerRetriever;

        public DynamicTableCompilerOrchestrator(
            DynamicTableCompilerRetriever dynamicTableCompilerRetriever
        )
        {
            this.dynamicTableCompilerRetriever = dynamicTableCompilerRetriever;
        }

        public async Task<List<Table>> CompileTablesToPdfRows(
            string accountId,
            List<Dictionary<string, List<Dictionary<string, string>>>> dynamicResponses,
            CultureInfo culture
        )
        {
            var tableRows = new List<TableRow>();
            foreach (var dynamicResponse in dynamicResponses)
            {
                var dynamicTableKeys = dynamicResponse.Keys.ToList(); // in the future, there could be multiple key values
                // for now, we are only expecting one key. Not sure yet how we can combine multiple tables or if there is even a point to doing this.
                if (dynamicTableKeys.Count > 1) throw new Exception("Multiple dynamic table keys specified. This is a configuration error");
                var dynamicTableKey = dynamicTableKeys[0];
                var responses = dynamicResponse[dynamicTableKey];

                var dynamicTableName = dynamicTableKey.Split("-").First();
                var compiler = dynamicTableCompilerRetriever.RetrieveCompiler(dynamicTableName);

                List<TableRow> rows;
                rows = await compiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
                tableRows.AddRange(rows);
            }

            var table = new Table("Variable estimates determined by your responses", tableRows, culture);
            return new List<Table>() {table};
        }

        public async Task<List<NodeTypeOption>> CompileTablesToConfigurationNodes(
            IEnumerable<DynamicTableMeta> dynamicTableMetas,
            string accountId,
            string areaId
        )
        {
            var nodes = new List<NodeTypeOption>() { };
            foreach (var dynamicTableMeta in dynamicTableMetas)
            {
                var compiler = dynamicTableCompilerRetriever.RetrieveCompiler(dynamicTableMeta.TableType);
                await compiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
            }

            return nodes;
        }
    }
}