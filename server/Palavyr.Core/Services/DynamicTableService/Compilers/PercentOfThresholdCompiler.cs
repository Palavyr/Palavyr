using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
    public class PercentOfThresholdCompiler : BaseCompiler<PercentOfThreshold>, IDynamicTablesCompiler
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IThresholdEvaluator thresholdEvaluator;

        public PercentOfThresholdCompiler(
            IGenericDynamicTableRepository<PercentOfThreshold> repository,
            IConfigurationRepository configurationRepository,
            IThresholdEvaluator thresholdEvaluator
        ) : base(repository)
        {
            this.configurationRepository = configurationRepository;
            this.thresholdEvaluator = thresholdEvaluator;
        }

        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId)
        {
            await Task.CompletedTask;
        }

        public Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier(),
                    dynamicTableMeta.ConvertToPrettyName(),
                    new List<string>() {"Continue"},
                    new List<string>() {"Continue"},
                    true,
                    false,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber, // this is for the tree, so okay, but it should be what the dynamic table item type is. We don't have access to that here, so we just say its a number.
                    dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
                ));
            return Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponseParts dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await Repository.GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);
            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(allRows[0].TableId);

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


                        return new List<TableRow>()
                        {
                            new TableRow(
                                dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : threshold.ItemName,
                                minBaseAmount,
                                maxBaseAmount,
                                false,
                                culture,
                                threshold.Range
                            )
                        };
                    }
                }
            }

            throw new InvalidOperationException("Provided threshold value was too high. This is a configuration error.");
        }

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents dynamicResponseComponents)
        {
            var records = await Repository.GetAllRowsMatchingDynamicResponseId(node.DynamicType);
            var orderedThresholds = records.OrderBy(x => x.Threshold);
            var currentResponseAsDouble = double.Parse(response);
            var isTooComplicated = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, orderedThresholds);
            return isTooComplicated;
        }
    }
}