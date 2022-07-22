using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PricingStrategyTableServices.Thresholds;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices.Compilers
{
    public interface ICategoryNestedThresholdCompiler : IPricingStrategyTableCompiler
    {
    }

    public class CategoryNestedThresholdCompiler : BaseCompiler<CategoryNestedThresholdTableRow>, ICategoryNestedThresholdCompiler
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;
        private readonly IEntityStore<CategoryNestedThresholdTableRow> psStore;
        private readonly IConversationOptionSplitter splitter;
        private readonly IThresholdEvaluator thresholdEvaluator;
        private readonly IResponseRetriever<CategoryNestedThresholdTableRow> responseRetriever;
        private readonly ICancellationTokenTransport cancellationTokenTransport;

        private CancellationToken CancellationToken => cancellationTokenTransport.CancellationToken;

        public CategoryNestedThresholdCompiler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore,
            IEntityStore<CategoryNestedThresholdTableRow> psStore,
            IConversationOptionSplitter splitter,
            IThresholdEvaluator thresholdEvaluator,
            IResponseRetriever<CategoryNestedThresholdTableRow> responseRetriever,
            ICancellationTokenTransport cancellationTokenTransport
        ) : base(psStore, convoNodeStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
            this.psStore = psStore;
            this.splitter = splitter;
            this.thresholdEvaluator = thresholdEvaluator;
            this.responseRetriever = responseRetriever;
            this.cancellationTokenTransport = cancellationTokenTransport;
        }

        public List<string> GetCategories<TEntity>(List<TEntity> rawRows)
        {
            var rows = rawRows.OrderBy(row => ((IOrderedRow)row).RowOrder).ToList();
            var categories = rows.Select(row => ((IMultiItem)row).Category).Distinct().ToList();
            return categories;
        }

        public async Task UpdateConversationNode<TEntity>(List<TEntity> table, string tableId, string intentId)
        {
            var category = GetCategories(table);
            var allNodes = await convoNodeStore.GetMany(intentId, s => s.IntentId);
            var nodes = allNodes
                .Where(n => n.IsPricingStrategyTableNode)
                .Where(n => splitter.GetTableIdFromPricingStrategyNodeType(n.NodeType) == tableId)
                .OrderBy(x => x.ResolveOrder)
                .ToList();

            if (nodes.Count > 0)
            {
                nodes.Single(x => x.ResolveOrder == 0).ValueOptions = splitter.JoinValueOptions(category);
            }
        }

        public async Task CompileToConfigurationNodes(PricingStrategyTableMeta pricingStrategyTableMeta, List<NodeTypeOptionResource> nodes)
        {
            var rawRows = await GetTableRows(pricingStrategyTableMeta);
            var categories = GetCategories(rawRows);

            var widgetResponseKey = pricingStrategyTableMeta.MakeUniqueIdentifier();

            nodes.AddAdditionalNode(
                NodeTypeOptionResource.Create(
                    pricingStrategyTableMeta.MakeUniqueIdentifier("Category"),
                    pricingStrategyTableMeta.ConvertToPrettyName("Category (1)"),
                    new List<string> { "Continue" },
                    categories,
                    true,
                    true,
                    false,
                    NodeTypeOptionResource.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    NodeTypeCodeEnum.X,
                    resolveOrder: 0,
                    isMultiOptionEditable: false,
                    pricingStrategyType: widgetResponseKey
                )
            );

            // need special threshold node -- like takenumber with conditions - min and max. Auto Add new nodes at 
            // compile time to set nodes for min and max. Hmmmm
            nodes.AddAdditionalNode(
                NodeTypeOptionResource.Create(
                    pricingStrategyTableMeta.MakeUniqueIdentifier("Threshold"),
                    pricingStrategyTableMeta.ConvertToPrettyName("Threshold (2)"),
                    new List<string> { "Continue" },
                    new List<string> { "Continue" },
                    true,
                    false,
                    false,
                    NodeTypeOptionResource.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber,
                    NodeTypeCodeEnum.III,
                    resolveOrder: 1,
                    isMultiOptionEditable: false,
                    pricingStrategyType: widgetResponseKey // check in widget component perhaps if this is PricingStrategy, and threshold type... then we can do a check against the server... bleh this is so gross. But there is no other way right now.
                )
            );
        }

        private async Task<List<CategoryNestedThresholdTableRow>> GetAllRowsMatchingPricingStrategyResponseId(string id)
        {
            return await psStore.RawReadonlyQuery()
                .Where(tableRow => id.EndsWith(tableRow.TableId))
                .ToListAsync(psStore.CancellationToken);
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(PricingStrategyResponseParts pricingStrategyResponseParts, List<string> pricingStrategyResponseIds, CultureInfo culture)
        {
            // pricingStrategyResponseIds is PricingStrategy table keys
            var responseId = GetSingleResponseId(pricingStrategyResponseIds);
            var records = await GetAllRowsMatchingPricingStrategyResponseId(responseId);

            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(pricingStrategyResponseParts);
            var category = GetResponseByResponseId(orderedResponseIds[0], pricingStrategyResponseParts);
            var amount = GetResponseByResponseId(orderedResponseIds[1], pricingStrategyResponseParts);

            var itemRows = records.Where(rec => rec.Category == category);

            var responseAmountAsDouble = double.Parse(amount);
            if (responseAmountAsDouble < 0) throw new Exception("Negative values are not allowed");

            var pricingStrategyMeta = await pricingStrategyTableMetaStore.Get(records.First().TableId, s => s.TableId);
            var thresholdResult = thresholdEvaluator.Evaluate(responseAmountAsDouble, itemRows);
            return new List<TableRow>
            {
                new TableRow(
                    pricingStrategyMeta.UseTableTagAsResponseDescription ? pricingStrategyMeta.TableTag : string.Join(" @ ", category, responseAmountAsDouble.ToString("C", culture)),
                    thresholdResult.ValueMin,
                    thresholdResult.ValueMax,
                    false,
                    culture,
                    thresholdResult.Range
                )
            };
        }

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents pricingStrategyResponseComponents)
        {
            if (node.ResolveOrder == 0)
            {
                return false;
            }

            var convoNodeIds = CollectNodeIds(pricingStrategyResponseComponents);

            var convoNodes = await convoNodeStore.GetMany(convoNodeIds.ToArray(), c => c.NodeId);
            var categoryNode = convoNodes.Single(x => x.ResolveOrder == 0);

            var categoryResponse = pricingStrategyResponseComponents.Responses
                .Single(x => x.ContainsKey(categoryNode.NodeId!))
                .Values.Single();

            var records = await GetAllRowsMatchingPricingStrategyResponseId(node.PricingStrategyType);

            var categoryThresholds = records
                .Where(rec => rec.Category == categoryResponse);
            var currentResponseAsDouble = double.Parse(response);
            var isTooComplicated = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, categoryThresholds);
            return isTooComplicated;
        }

        //
        // private PricingStrategyValidationResult ValidationLogic(List<CategoryNestedThreshold> table, string tableTag)
        // {
        //     var reasons = new List<string>();
        //     var valid = true;
        //
        //     var itemIds = table.Select(x => x.ItemId).Distinct().ToList();
        //
        //     var itemNames = table.Select(x => x.ItemName).Distinct().ToList();
        //     var numCategories = itemIds.Count();
        //     if (itemNames.Count() != numCategories)
        //     {
        //         reasons.Add($"Duplicate categories found in {tableTag}");
        //         valid = false;
        //     }
        //
        //     if (itemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
        //     {
        //         reasons.Add("One or more categories did not contain text");
        //         valid = false;
        //     }
        //
        //     foreach (var itemId in itemIds)
        //     {
        //         var thresholds = table.Where(x => x.ItemId == itemId).Select(x => x.Threshold).ToList();
        //         if (thresholds.Distinct().Count() != thresholds.Count())
        //         {
        //             reasons.Add($"Duplicate threshold values found in {tableTag}");
        //             valid = false;
        //         }
        //
        //         if (thresholds.Any(x => x < 0))
        //         {
        //             reasons.Add($"Negative threshold value found in {tableTag}");
        //             valid = false;
        //         }
        //
        //         if (!valid)
        //         {
        //             break;
        //         }
        //     }
        //
        //     return valid
        //         ? PricingStrategyValidationResult.CreateValid(tableTag)
        //         : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        // }

        // public PricingStrategyValidationResult ValidatePricingStrategyPreSave<TEntity>(PricingStrategyTable<TEntity> pricingStrategyTable)
        // {
        //     var table = pricingStrategyTable.TableData!;
        //     var tableTag = pricingStrategyTable.TableTag!;
        //     return ValidationLogic(table as List<CategoryNestedThreshold>, tableTag);
        // }

        // public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(PricingStrategyTableMeta pricingStrategyTableMeta)
        // {
        //     var tableId = pricingStrategyTableMeta.TableId;
        //     var table = await psStore.GetMany(tableId, s => s.TableId);
        //     return ValidationLogic(table.ToList(), pricingStrategyTableMeta.TableTag);
        // }

        public async Task<List<TableRow>> CreatePreviewData(PricingStrategyTableMeta tableTableMeta, Intent intent, CultureInfo culture)
        {
            var availableNestedThreshold = await responseRetriever.RetrieveAllAvailableResponses(tableTableMeta.TableId);
            var currentRows = new List<TableRow>
            {
                new TableRow(
                    tableTableMeta.UseTableTagAsResponseDescription ? tableTableMeta.TableTag : availableNestedThreshold.First().Category,
                    availableNestedThreshold.First().ValueMin,
                    availableNestedThreshold.First().ValueMax,
                    false,
                    culture,
                    availableNestedThreshold.First().Range)
            };

            return currentRows;
        }

        public async Task<List<CategoryNestedThresholdTableRow>> RetrieveAllAvailableResponses(string accountId, string responseId, CancellationToken cancellationToken)
        {
            return await GetAllRowsMatchingResponseId(responseId);
        }

        List<string> CollectNodeIds(PricingStrategyResponseComponents pricingStrategyResponseComponents)
        {
            var nodeIds = new List<string>();
            foreach (var response in pricingStrategyResponseComponents.Responses)
            {
                nodeIds.AddRange(response.Keys);
            }

            return nodeIds;
        }
    }
}