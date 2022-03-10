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
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public interface IPercentOfThresholdCompiler : IDynamicTablesCompiler
    {
    }

    public class PercentOfThresholdCompiler : BaseCompiler<PercentOfThreshold>, IPercentOfThresholdCompiler
    {
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;
        private readonly IThresholdEvaluator thresholdEvaluator;
        private readonly IResponseRetriever responseRetriever;

        public PercentOfThresholdCompiler(
            IPricingStrategyEntityStore<PercentOfThreshold> repository,
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore,
            IThresholdEvaluator thresholdEvaluator,
            IResponseRetriever responseRetriever
        ) : base(repository)
        {
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.thresholdEvaluator = thresholdEvaluator;
            this.responseRetriever = responseRetriever;
        }

        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId, string areaIdentifier)
        {
            await Task.CompletedTask;
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier(),
                    dynamicTableMeta.ConvertToPrettyName(),
                    new List<string>() { "Continue" },
                    new List<string>() { "Continue" },
                    true,
                    false,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber, // this is for the tree, so okay, but it should be what the dynamic table item type is. We don't have access to that here, so we just say its a number.
                    NodeTypeCode.II,
                    dynamicType: dynamicTableMeta.MakeUniqueIdentifier(),
                    shouldRenderChildren: true
                ));
            await Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(DynamicResponseParts dynamicResponseParts, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponseParts, dynamicResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await RetrieveAllAvailableResponses(dynamicResponseId);
            var dynamicMeta = await dynamicTableMetaStore.Get(allRows[0].TableId, s => s.TableId);

            var itemIds = allRows.Select(item => item.ItemId).Distinct().ToArray();

            var tableRows = new List<TableRow>();

            foreach (var itemId in itemIds)
            {
                var itemThresholds = allRows.Where(item => item.ItemId == itemId);
                var thresholdResult = (PercentOfThreshold)thresholdEvaluator.Evaluate(responseValueAsDouble, itemThresholds);

                var minBaseAmount = thresholdResult.ValueMin;
                var maxBaseAmount = thresholdResult.ValueMax;

                if (thresholdResult.PosNeg)
                {
                    minBaseAmount += minBaseAmount * (thresholdResult.Modifier / 100);
                    maxBaseAmount += maxBaseAmount * (thresholdResult.Modifier / 100);
                }
                else
                {
                    minBaseAmount -= minBaseAmount * (thresholdResult.Modifier / 100);
                    maxBaseAmount -= maxBaseAmount * (thresholdResult.Modifier / 100);
                }

                tableRows.Add(
                    new TableRow(
                        dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : thresholdResult.ItemName,
                        minBaseAmount,
                        maxBaseAmount,
                        false,
                        culture,
                        thresholdResult.Range
                    ));
            }

            return tableRows;
        }

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents _)
        {
            var currentResponseAsDouble = double.Parse(response);
            var records = await repository.GetAllRowsMatchingDynamicResponseId(node.DynamicType);
            var uniqueItemIds = records.Select(x => x.ItemId).Distinct();

            var isTooComplicated = false;
            foreach (var itemId in uniqueItemIds)
            {
                var itemRecords = records.Where(x => x.ItemId == itemId);
                var itemComplex = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, itemRecords);
                if (itemComplex)
                {
                    isTooComplicated = true;
                }
            }

            return isTooComplicated;
        }

        private PricingStrategyValidationResult ValidationLogic(List<PercentOfThreshold> table, string tableTag)
        {
            var reasons = new List<string>();
            var valid = true;

            var itemIds = table.Select(x => x.ItemId).Distinct();

            foreach (var itemId in itemIds)
            {
                var thresholds = table.Where(x => x.ItemId == itemId).Select(x => x.Threshold).ToList();
                if (thresholds.Distinct().Count() != thresholds.Count())
                {
                    reasons.Add($"Duplicate threshold values found in {tableTag}");
                    valid = false;
                }

                if (thresholds.Any(x => x < 0))
                {
                    reasons.Add($"Negative threshold value found in {tableTag}");
                    valid = false;
                }

                if (!valid)
                {
                    break;
                }
            }

            return valid
                ? PricingStrategyValidationResult.CreateValid(tableTag)
                : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        }

        public PricingStrategyValidationResult ValidatePricingStrategyPreSave(DynamicTable dynamicTable)
        {
            var table = dynamicTable.PercentOfThreshold;
            var tableTag = dynamicTable.TableTag;
            return ValidationLogic(table, tableTag);
        }

        public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(DynamicTableMeta dynamicTableMeta)
        {
            var tableId = dynamicTableMeta.TableId;
            var areaId = dynamicTableMeta.AreaIdentifier;
            var table = await repository.GetAllRows(areaId, tableId);
            return ValidationLogic(table, dynamicTableMeta.TableTag);
        }

        public async Task<List<TableRow>> CreatePreviewData(DynamicTableMeta tableMeta, Area area, CultureInfo culture)
        {
            var availablePercentOfThreshold = await responseRetriever.RetrieveAllAvailableResponses<PercentOfThreshold>(tableMeta.TableId);
            var responseParts = DynamicTableTypes.CreatePercentOfThreshold().CreateDynamicResponseParts(availablePercentOfThreshold.First().TableId, availablePercentOfThreshold.First().Threshold.ToString());
            var currentRows = await CompileToPdfTableRow(responseParts, new List<string>() { tableMeta.TableId }, culture);
            return currentRows;
        }

        public async Task<List<PercentOfThreshold>> RetrieveAllAvailableResponses(string dynamicResponseId)
        {
            return await GetAllRowsMatchingResponseId(dynamicResponseId);
        }
    }
}