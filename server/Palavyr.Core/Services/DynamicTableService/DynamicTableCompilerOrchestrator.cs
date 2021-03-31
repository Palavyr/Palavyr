using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.DynamicTableService.Compilers;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicTableCompilerOrchestrator : IDynamicTableCompilerOrchestrator
    {
        private readonly SelectOneFlatCompiler selectOneFlatCompiler;
        private readonly PercentOfThresholdCompiler percentOfThresholdCompiler;
        private readonly BasicThresholdCompiler basicThresholdCompiler;
        private readonly TwoNestedCategoryCompiler twoNestedCategoryCompiler;

        public DynamicTableCompilerOrchestrator(
            SelectOneFlatCompiler selectOneFlatCompiler,
            PercentOfThresholdCompiler percentOfThresholdCompiler,
            BasicThresholdCompiler basicThresholdCompiler,
            TwoNestedCategoryCompiler twoNestedCategoryCompiler
        )
        {
            this.selectOneFlatCompiler = selectOneFlatCompiler;
            this.percentOfThresholdCompiler = percentOfThresholdCompiler;
            this.basicThresholdCompiler = basicThresholdCompiler;
            this.twoNestedCategoryCompiler = twoNestedCategoryCompiler;
        }

        public async Task<List<Table>> CompileTablesToPdfRows(
            string accountId,
            List<Dictionary<string, DynamicResponse>> dynamicResponses,
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

                List<TableRow> rows;
                if (dynamicTableKey.StartsWith(DynamicTableTypes.CreateSelectOneFlat().TableType))
                {
                    rows = await selectOneFlatCompiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
                }
                else if (dynamicTableKey.StartsWith(DynamicTableTypes.CreatePercentOfThreshold().TableType))
                {
                    rows = await percentOfThresholdCompiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
                }
                else if (dynamicTableKey.StartsWith(DynamicTableTypes.CreateBasicThreshold().TableType))
                {
                    rows = await basicThresholdCompiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
                }
                else if (dynamicTableKey.StartsWith(DynamicTableTypes.CreateTwoNestedCategory().TableType))
                {
                    rows = await twoNestedCategoryCompiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
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


        public async Task<List<NodeTypeOption>> CompileTablesToConfigurationNodes(
            IEnumerable<DynamicTableMeta> dynamicTableMetas,
            string accountId,
            string areaId
        )
        {
            var nodes = new List<NodeTypeOption>() { };
            foreach (var dynamicTableMeta in dynamicTableMetas)
            {
                switch (dynamicTableMeta.TableType)
                {
                    case nameof(SelectOneFlat):
                        await selectOneFlatCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                        break;
                    case nameof(PercentOfThreshold):
                        await percentOfThresholdCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                        break;
                    case nameof(BasicThreshold):
                        await basicThresholdCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                        break;
                    case nameof(TwoNestedCategory):
                        await twoNestedCategoryCompiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
                        break;

                    // add new node types here

                    default:
                        throw new Exception("Table logic not yet implemented");
                }
            }

            return nodes;
        }
    }
}