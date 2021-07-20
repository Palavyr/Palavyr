using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class CategoryNestedThresholdCompiler : BaseCompiler<CategoryNestedThreshold>, IDynamicTablesCompiler
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConversationOptionSplitter splitter;
        private readonly IThresholdEvaluator thresholdEvaluator;

        public CategoryNestedThresholdCompiler(
            IGenericDynamicTableRepository<CategoryNestedThreshold> repository,
            IConfigurationRepository configurationRepository,
            IConversationOptionSplitter splitter,
            IThresholdEvaluator thresholdEvaluator
        ) : base(repository)
        {
            this.configurationRepository = configurationRepository;
            this.splitter = splitter;
            this.thresholdEvaluator = thresholdEvaluator;
        }

        public List<string> GetCategories(List<CategoryNestedThreshold> rawRows)
        {
            var rows = rawRows.OrderBy(row => row.RowOrder).ToList();
            var categories = rows.Select(row => row.ItemName).Distinct().ToList();
            return categories;
        }

        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId, string areaIdentifier, string accountId)
        {
            var update = table.CategoryNestedThreshold;
            var category = GetCategories(update);

            var nodes = (await context.ConversationNodes.ToListAsync())
                .Where(x => x.IsDynamicTableNode && splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId)
                .OrderBy(x => x.ResolveOrder).ToList();
            if (nodes.Count > 0)
            {
                nodes.Single(x => x.ResolveOrder == 0).ValueOptions = splitter.JoinValueOptions(category);
            }
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var rawRows = await GetTableRows(dynamicTableMeta);
            var categories = GetCategories(rawRows);

            var widgetResponseKey = dynamicTableMeta.MakeUniqueIdentifier();

            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Category"),
                    dynamicTableMeta.ConvertToPrettyName("Category (1)"),
                    new List<string>() {"Continue"},
                    categories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    NodeTypeCode.X,
                    resolveOrder: 0,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey
                )
            );

            // need special threshold node -- like takenumber with conditions - min and max. Auto Add new nodes at 
            // compile time to set nodes for min and max. Hmmmm
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Threshold"),
                    dynamicTableMeta.ConvertToPrettyName("Threshold (2)"),
                    new List<string>() {"Continue"},
                    new List<string>() {"Continue"},
                    true,
                    false,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber,
                    NodeTypeCode.III,
                    resolveOrder: 1,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey // check in widget component perhaps if this is dynamic, and thresholdtype... then we can do a check against the server... bleh this is so gross. But there is no other way right now.
                )
            );
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponseParts dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            // dynamicResponseIds is dynamic table keys
            var responseId = GetSingleResponseId(dynamicResponseIds);
            var records = await Repository.GetAllRowsMatchingDynamicResponseId(accountId, responseId);

            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(dynamicResponse);
            var category = GetResponseByResponseId(orderedResponseIds[0], dynamicResponse);
            var amount = GetResponseByResponseId(orderedResponseIds[1], dynamicResponse);

            var itemRows = records.Where(rec => rec.ItemName == category);

            var responseAmountAsDouble = double.Parse(amount);
            if (responseAmountAsDouble < 0) throw new Exception("Negative values are not allowed");

            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(records.First().TableId);
            var thresholdResult = thresholdEvaluator.Evaluate(responseAmountAsDouble, itemRows);
            return new List<TableRow>()
            {
                new TableRow(
                    dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : string.Join(" @ ", new[] {category, responseAmountAsDouble.ToString("C", culture)}),
                    thresholdResult.ValueMin,
                    thresholdResult.ValueMax,
                    false,
                    culture,
                    thresholdResult.Range
                )
            };
        }

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents dynamicResponseComponents)
        {
            if (node.ResolveOrder == 0)
            {
                return false;
            }

            var convoNodeIds = CollectNodeIds(dynamicResponseComponents);

            var convoNodes = await Repository.GetConversationNodeByIds(convoNodeIds);
            var categoryNode = convoNodes.Single(x => x.ResolveOrder == 0);
            // var thresholdNode = convoNodes.Single(x => x.ResolveOrder == 1);

            var categoryResponse = dynamicResponseComponents.Responses
                .Single(x => x.ContainsKey(categoryNode.NodeId!))
                .Values.Single();

            var records = await Repository.GetAllRowsMatchingDynamicResponseId(node.DynamicType);

            var categoryThresholds = records
                .Where(rec => rec.ItemName == categoryResponse);
            var currentResponseAsDouble = double.Parse(response);
            var isTooComplicated = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, categoryThresholds);
            return isTooComplicated;
        }

        public async Task<PricingStrategyValidationResult> ValidatePricingStrategy(DynamicTableMeta dynamicTableMeta)
        {
            var tableId = dynamicTableMeta.TableId;
            var accountId = dynamicTableMeta.AccountId;
            var areaId = dynamicTableMeta.AreaIdentifier;

            var reasons = new List<string>();
            var valid = true;

            var table = await Repository.GetAllRows(accountId, areaId, tableId);
            var itemIds = table.Select(x => x.ItemId).Distinct().ToList();
            
            var itemNames = table.Select(x => x.ItemName).Distinct().ToList();
            var numCategories = itemIds.Count();
            if (itemNames.Count() != numCategories)
            {
                reasons.Add($"Duplicate threshold values found in {dynamicTableMeta.TableTag}");
                valid = false;
            }

            if (itemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
            {
                reasons.Add($"One or more categories did not contain text");
                valid = false;
            }

            foreach (var itemId in itemIds)
            {
                var thresholds = table.Where(x => x.ItemId == itemId).Select(x => x.Threshold).ToList();
                if (thresholds.Distinct().Count() != thresholds.Count())
                {
                    reasons.Add($"Duplicate threshold values found in {dynamicTableMeta.TableTag}");
                    valid = false;
                }

                if (thresholds.Any(x => x < 0))
                {
                    reasons.Add($"Negative threshold value found in {dynamicTableMeta.TableTag}");
                    valid = false;
                }

                if (!valid)
                {
                    break;
                }
            }

            return valid
                ? PricingStrategyValidationResult.CreateValid(dynamicTableMeta.TableTag)
                : PricingStrategyValidationResult.CreateInvalid(dynamicTableMeta.TableTag, reasons);
        }

        List<string> CollectNodeIds(DynamicResponseComponents dynamicResponseComponents)
        {
            var nodeIds = new List<string>();
            foreach (var response in dynamicResponseComponents.Responses)
            {
                nodeIds.AddRange(response.Keys);
            }

            return nodeIds;
        }
    }
}