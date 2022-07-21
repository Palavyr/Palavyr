using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PricingStrategyTableServices.Thresholds;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices.Compilers
{
    public interface ISimpleThresholdCompiler : IPricingStrategyTableCompiler
    {
    }

    public class SimpleThresholdCompiler : BaseCompiler<SimpleThresholdTableRow>, ISimpleThresholdCompiler
    {
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyMetaStore;
        private readonly IEntityStore<SimpleThresholdTableRow> basicThresholdStore;
        private readonly IThresholdEvaluator thresholdEvaluator;
        private readonly IResponseRetriever<SimpleThresholdTableRow> responseRetriever;

        public SimpleThresholdCompiler(
            IEntityStore<PricingStrategyTableMeta> pricingStrategyMetaStore,
            IEntityStore<SimpleThresholdTableRow> basicThresholdStore,
            IEntityStore<ConversationNode> convoNodeStore,
            IThresholdEvaluator thresholdEvaluator,
            IResponseRetriever<SimpleThresholdTableRow> responseRetriever
        ) : base(basicThresholdStore, convoNodeStore)
        {
            this.pricingStrategyMetaStore = pricingStrategyMetaStore;
            this.basicThresholdStore = basicThresholdStore;
            this.thresholdEvaluator = thresholdEvaluator;
            this.responseRetriever = responseRetriever;
        }

        public async Task UpdateConversationNode<BasicThreshold>(List<BasicThreshold> table, string tableId, string intentId)
        {
            await Task.CompletedTask;
        }

        public async Task CompileToConfigurationNodes(PricingStrategyTableMeta pricingStrategyTableMeta, List<NodeTypeOptionResource> nodes)
        {
            var nodeTypeOption = NodeTypeOptionResource.Create(
                pricingStrategyTableMeta.MakeUniqueIdentifier(),
                pricingStrategyTableMeta.ConvertToPrettyName(),
                new List<string>() { "Continue" },
                new List<string>() { "Continue" },
                true,
                false,
                false,
                NodeTypeOptionResource.CustomTables,
                DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber,
                NodeTypeCodeEnum.II,
                resolveOrder: 0,
                pricingStrategyType: pricingStrategyTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
            await Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(PricingStrategyResponseParts pricingStrategyResponseParts, List<string> pricingStrategyResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(pricingStrategyResponseIds);
            var responseValue = GetSingleResponseValue(pricingStrategyResponseParts, pricingStrategyResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await RetrieveAllAvailableResponses(responseId);

            var pricingStrategyMeta = await pricingStrategyMetaStore.Get(allRows.First().TableId, s => s.TableId);

            var itemsToCreateRowsFor = allRows.Where(x => !string.IsNullOrWhiteSpace(x.ItemName)).Select(row => row.ItemName).Distinct();

            var tableRows = new List<TableRow>();

            foreach (var itemName in itemsToCreateRowsFor)
            {
                var itemRows = allRows.Where(row => row.ItemName == itemName).ToList();
                if (itemRows.Count > 0)
                {
                    var thresholdResult = thresholdEvaluator.Evaluate(responseValueAsDouble, itemRows);
                    tableRows.Add(
                        new TableRow(
                            pricingStrategyMeta.UseTableTagAsResponseDescription ? pricingStrategyMeta.TableTag : itemName,
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

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents _)
        {
            var thresholds = await basicThresholdStore.RawReadonlyQuery()
                .Where(tableRow => node.PricingStrategyType.EndsWith(tableRow.TableId))
                .ToListAsync(basicThresholdStore.CancellationToken);

            var currentResponseAsDouble = double.Parse(response);
            var isTooComplicated = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, thresholds);
            return isTooComplicated;
        }

        // private PricingStrategyValidationResult ValidationLogic(List<BasicThreshold> table, string tableTag)
        // {
        //     var reasons = new List<string>();
        //     var valid = true;
        //
        //     if (table.Select(x => x.Threshold).Distinct().Count() != table.Count)
        //     {
        //         reasons.Add($"Duplicate threshold values found in {tableTag}");
        //         valid = false;
        //     }
        //
        //     if (table.Any(x => x.Threshold < 0))
        //     {
        //         reasons.Add($"Negative threshold value found in {tableTag}");
        //         valid = false;
        //     }
        //
        //
        //     return valid
        //         ? PricingStrategyValidationResult.CreateValid(tableTag)
        //         : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        // }

        // public PricingStrategyValidationResult ValidatePricingStrategyPreSave<TEntity>(PricingStrategyTable<TEntity> pricingStrategyTable)
        // {
        //     // var table = pricingStrategyTable.BasicThreshold;
        //     var table = pricingStrategyTable.TableData;
        //     var tableTag = pricingStrategyTable.TableTag;
        //     return ValidationLogic(table as List<BasicThreshold>, tableTag);
        // }

        // public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(PricingStrategyTableMeta pricingStrategyTableMeta)
        // {
        //     var tableId = pricingStrategyTableMeta.TableId;
        //     var thresholds = await basicThresholdStore.GetMany(tableId, s => s.TableId);
        //     return ValidationLogic(thresholds.ToList(), pricingStrategyTableMeta.TableTag);
        // }

        public async Task<List<TableRow>> CreatePreviewData(PricingStrategyTableMeta tableTableMeta, Intent intent, CultureInfo culture)
        {
            var availableBasicThreshold = await responseRetriever.RetrieveAllAvailableResponses(tableTableMeta.TableId);
            var responseParts = PricingStrategyResponsePartJoiner.CreatePricingStrategyResponseParts(availableBasicThreshold.First().TableId, availableBasicThreshold.First().Threshold.ToString());
            var currentRows = await CompileToPdfTableRow(responseParts, new List<string>() { tableTableMeta.TableId }, culture);
            return currentRows;
        }

        public async Task<List<SimpleThresholdTableRow>> RetrieveAllAvailableResponses(string responseId)
        {
            return await GetAllRowsMatchingResponseId(responseId);
        }
    }
}