using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            var categories = rows.Select(row => row.Category).Distinct().ToList();
            return categories;
        }

        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId)
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

            var orderedThresholds = records
                .Where(rec => rec.Category == category)
                .OrderBy(x => x.Threshold);
            var responseAmountAsDouble = double.Parse(amount);

            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(records.First().TableId);
            var thresholdResult = thresholdEvaluator.Evaluate(responseAmountAsDouble, orderedThresholds);
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
            if (node.ResolveOrder == 0) throw new Exception("Shouldn't be doing a check on the first node");
            var categoryResponse = dynamicResponseComponents.Responses.Single().Values.Single();

            var records = await Repository.GetAllRowsMatchingDynamicResponseId(node.DynamicType);

            var orderedThresholds = records
                .Where(rec => rec.Category == categoryResponse)
                .OrderBy(x => x.Threshold);
            var currentResponseAsDouble = double.Parse(response);
            var isTooComplicated = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, orderedThresholds);
            return isTooComplicated;
        }
    }
}