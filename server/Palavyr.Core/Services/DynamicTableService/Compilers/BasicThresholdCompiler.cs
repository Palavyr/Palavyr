using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public class BasicThresholdCompiler : BaseCompiler<BasicThreshold>, IDynamicTablesCompiler
    {
        private readonly IGenericDynamicTableRepository<BasicThreshold> repository;

        public BasicThresholdCompiler(IGenericDynamicTableRepository<BasicThreshold> repository) : base(repository)
        {
            this.repository = repository;
        }

        public Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var nodeTypeOption = NodeTypeOption.Create(
                dynamicTableMeta.MakeUniqueIdentifier(),
                dynamicTableMeta.ConvertToPrettyName(),
                new List<string>() {"Continue"},
                new List<string>() {"Continue"},
                true,
                false,
                false,
                NodeTypeOption.CustomTables,
                DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber,
                dynamicType: dynamicTableMeta.MakeUniqueIdentifier()

            );
            nodes.AddAdditionalNode(nodeTypeOption);
            return Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, List<Dictionary<string, string>> dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await repository.GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);

            var itemsToCreateRowsFor = allRows.Select(row => row.ItemName).Distinct();


            var tableRows = new List<TableRow>();
            foreach (var itemName in itemsToCreateRowsFor)
            {
                var itemRows = allRows.Where(row => row.ItemName == itemName).OrderBy(row => row.Threshold);

                foreach (var threshold in itemRows)
                {
                    if (responseValueAsDouble >= threshold.Threshold)
                    {
                        var minBaseAmount = threshold.ValueMin;
                        var maxBaseAmount = threshold.ValueMax;

                        tableRows.Add(
                            new TableRow(
                                threshold.ItemName,
                                minBaseAmount,
                                maxBaseAmount,
                                false,
                                culture,
                                threshold.Range
                            )
                        );
                    }
                }
            }
            return tableRows;
        }
    }
}