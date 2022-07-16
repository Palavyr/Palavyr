﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PricingStrategyTableServices.Thresholds;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices.Compilers
{
    public interface IPercentOfThresholdCompiler : IPricingStrategyTableCompiler
    {
    }

    public class PercentOfThresholdCompiler : BaseCompiler<PercentOfThresholdTableRow>, IPercentOfThresholdCompiler
    {
        private readonly IEntityStore<PercentOfThresholdTableRow> psStore;
        private readonly IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore;
        private readonly IThresholdEvaluator thresholdEvaluator;
        private readonly IResponseRetriever<PercentOfThresholdTableRow> responseRetriever;

        public PercentOfThresholdCompiler(
            // IPricingStrategyEntityStore<PercentOfThreshold> repository,
            IEntityStore<PercentOfThresholdTableRow> psStore,
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<PricingStrategyTableMeta> dynamicTableMetaStore,
            IThresholdEvaluator thresholdEvaluator,
            IResponseRetriever<PercentOfThresholdTableRow> responseRetriever
        ) : base(psStore, convoNodeStore)
        {
            this.psStore = psStore;
            this.dynamicTableMetaStore = dynamicTableMetaStore;
            this.thresholdEvaluator = thresholdEvaluator;
            this.responseRetriever = responseRetriever;
        }

        public async Task UpdateConversationNode<T>(List<T> table, string tableId, string intentId)
        {
            await Task.CompletedTask;
        }

        public async Task CompileToConfigurationNodes(PricingStrategyTableMeta pricingStrategyTableMeta, List<NodeTypeOptionResource> nodes)
        {
            nodes.AddAdditionalNode(
                NodeTypeOptionResource.Create(
                    pricingStrategyTableMeta.MakeUniqueIdentifier(),
                    pricingStrategyTableMeta.ConvertToPrettyName(),
                    new List<string>() { "Continue" },
                    new List<string>() { "Continue" },
                    true,
                    false,
                    false,
                    NodeTypeOptionResource.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber, // this is for the tree, so okay, but it should be what the dynamic table item type is. We don't have access to that here, so we just say its a number.
                    NodeTypeCode.II,
                    dynamicType: pricingStrategyTableMeta.MakeUniqueIdentifier(),
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
                var thresholdResult = (PercentOfThresholdTableRow)thresholdEvaluator.Evaluate(responseValueAsDouble, itemThresholds);

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

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents _)
        {
            var currentResponseAsDouble = double.Parse(response);
            
            var records = await psStore
                .RawReadonlyQuery()
                .Where(x => node.DynamicType.EndsWith(x.TableId))
                .ToListAsync(psStore.CancellationToken);
            
            // var records = await repository.GetAllRowsMatchingDynamicResponseId(node.DynamicType);
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

        // private PricingStrategyValidationResult ValidationLogic(List<PercentOfThreshold> table, string tableTag)
        // {
        //     var reasons = new List<string>();
        //     var valid = true;
        //
        //     var itemIds = table.Select(x => x.ItemId).Distinct();
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
        //     var table = pricingStrategyTable.TableData as List<PercentOfThreshold>;
        //     var tableTag = pricingStrategyTable.TableTag;
        //     return ValidationLogic(table, tableTag);
        // }

        // public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(DynamicTableMeta dynamicTableMeta)
        // {
        //     var tableId = dynamicTableMeta.TableId;
        //     // var areaId = dynamicTableMeta.AreaIdentifier;
        //
        //     var table = await psStore.GetMany(tableId, s => s.TableId);
        //     // var table = await repository.GetAllRows(areaId, tableId);
        //     return ValidationLogic(table.ToList(), dynamicTableMeta.TableTag);
        // }

        public async Task<List<TableRow>> CreatePreviewData(PricingStrategyTableMeta tableTableMeta, Intent intent, CultureInfo culture)
        {
            var availablePercentOfThreshold = await responseRetriever.RetrieveAllAvailableResponses(tableTableMeta.TableId);
            var responseParts = PricingStrategyResponsePartJoiner.CreateDynamicResponseParts(availablePercentOfThreshold.First().TableId, availablePercentOfThreshold.First().Threshold.ToString());
            var currentRows = await CompileToPdfTableRow(responseParts, new List<string>() { tableTableMeta.TableId }, culture);
            return currentRows;
        }

        public async Task<List<PercentOfThresholdTableRow>> RetrieveAllAvailableResponses(string dynamicResponseId)
        {
            return await GetAllRowsMatchingResponseId(dynamicResponseId);
        }
    }
}