using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.DynamicTableService.Thresholds;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public class BasicThresholdCompiler : BaseCompiler<BasicThreshold>, IDynamicTablesCompiler
    {
        private readonly IGenericDynamicTableRepository<BasicThreshold> repository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IThresholdEvaluator thresholdEvaluator;

        public BasicThresholdCompiler(
            IGenericDynamicTableRepository<BasicThreshold> repository,
            IConfigurationRepository configurationRepository,
            IThresholdEvaluator thresholdEvaluator
        ) : base(repository)
        {
            this.repository = repository;
            this.configurationRepository = configurationRepository;
            this.thresholdEvaluator = thresholdEvaluator;
        }

        public async Task UpdateConversationNode(DashContext dashContext, DynamicTable table, string tableId)
        {
            await Task.CompletedTask;
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
                NodeTypeCode.II,
                resolveOrder: 0,
                dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
            return Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponseParts dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await repository.GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);

            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(allRows.First().TableId);

            var itemsToCreateRowsFor = allRows.Select(row => row.ItemName).Distinct();

            var tableRows = new List<TableRow>();

            foreach (var itemName in itemsToCreateRowsFor)
            {
                var itemRows = allRows.Where(row => row.ItemName == itemName).ToList();
                if (itemRows.Count > 0)
                {
                    var thresholdResult = thresholdEvaluator.Evaluate(responseValueAsDouble, itemRows);
                    tableRows.Add(
                        new TableRow(
                            dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : itemName,
                            thresholdResult.ValueMin,
                            thresholdResult.ValueMax,
                            false,
                            culture,
                            thresholdResult.Range
                        )
                    );
                }
            }

            return tableRows;
        }

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents _)
        {
            var thresholds = await Repository.GetAllRowsMatchingDynamicResponseId(node.DynamicType);
            var currentResponseAsDouble = double.Parse(response);
            var isTooComplicated = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, thresholds);
            return isTooComplicated;
        }
    }
}