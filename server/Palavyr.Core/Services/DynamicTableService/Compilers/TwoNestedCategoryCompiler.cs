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
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public class TwoNestedCategoryCompiler : BaseCompiler<TwoNestedCategory>, IDynamicTablesCompiler
    {
        private readonly IGenericDynamicTableRepository<TwoNestedCategory> repository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConversationOptionSplitter splitter;

        public TwoNestedCategoryCompiler(
            IGenericDynamicTableRepository<TwoNestedCategory> repository,
            IConfigurationRepository configurationRepository,
            IConversationOptionSplitter splitter
        ) : base(repository)
        {
            this.repository = repository;
            this.configurationRepository = configurationRepository;
            this.splitter = splitter;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponseParts dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(dynamicResponseIds);
            var records = await repository.GetAllRowsMatchingDynamicResponseId(accountId, responseId);

            // itemName
            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(dynamicResponse);
            var outerCategory = GetResponseByResponseId(orderedResponseIds[0], dynamicResponse);
            var innerCategory = GetResponseByResponseId(orderedResponseIds[1], dynamicResponse);

            var result = records.Single(rec => rec.ItemName == outerCategory && rec.InnerItemName == innerCategory);
            var dynamicTableMeta = await configurationRepository.GetDynamicTableMetaByTableId(result.TableId);

            return new List<TableRow>()
            {
                new TableRow(
                    dynamicTableMeta.UseTableTagAsResponseDescription ? dynamicTableMeta.TableTag : string.Join(" & ", new[] {result.ItemName, result.InnerItemName}),
                    result.ValueMin,
                    result.ValueMax,
                    false,
                    culture,
                    result.Range
                )
            };
        }


        public Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents dynamicResponseComponents)
        {
            return Task.FromResult(false);
        }

        public PricingStrategyValidationResult ValidationLogic(List<TwoNestedCategory> table, string tableTag)
        {
            var reasons = new List<string>();
            var valid = true;

            var itemIds = table.Select(x => x.ItemId).Distinct().ToList();
            var itemNames = table.Select(x => x.ItemName).Distinct().ToList();
            var numCategories = itemIds.Count();
            if (itemNames.Count() != numCategories)
            {
                reasons.Add($"Duplicate outer category values found in {tableTag}");
                valid = false;
            }

            if (itemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
            {
                reasons.Add($"One or more outer categories did not contain text in {tableTag}");
                valid = false;
            }

            foreach (var itemId in itemIds)
            {
                var innerItemNames = table.Where(x => x.ItemId == itemId).Select(x => x.InnerItemName).ToList();
                if (innerItemNames.Count() != numCategories)
                {
                    reasons.Add($"Duplicate inner category values found in {tableTag}");
                    valid = false;
                }

                if (innerItemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
                {
                    reasons.Add($"One or more inner categories did not contain text in {tableTag}");
                    valid = false;
                }

                break; // all inner categories are copied across outer categories
            }

            return valid
                ? PricingStrategyValidationResult.CreateValid(tableTag)
                : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        }

        public PricingStrategyValidationResult ValidatePricingStrategyPreSave(DynamicTable dynamicTable)
        {
            var table = dynamicTable.TwoNestedCategory;
            var tableTag = dynamicTable.TableTag;
            return ValidationLogic(table, tableTag);
        }

        public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(DynamicTableMeta dynamicTableMeta)
        {
            var tableId = dynamicTableMeta.TableId;
            var accountId = dynamicTableMeta.AccountId;
            var areaId = dynamicTableMeta.AreaIdentifier;
            var table = await Repository.GetAllRows(accountId, areaId, tableId);
            return ValidationLogic(table, dynamicTableMeta.TableTag);
        }

        private CategoryRetriever GetInnerAndOuterCategories(List<TwoNestedCategory> rawRows)
        {
            // This table type does not facilitate multiple branches. I.e. the inner categories are all the same for all of the outer categories.
            var rows = rawRows.OrderBy(row => row.RowOrder).ToList();

            var outerCategories = rows.Select(row => row.ItemName).Distinct().ToList();

            var itemId = rows.Select(row => row.ItemId).Distinct().First();
            var innerCategories = rows.Where(row => row.ItemId == itemId).Select(row => row.InnerItemName).ToList();

            return new CategoryRetriever
            {
                InnerCategories = innerCategories,
                OuterCategories = outerCategories
            };
        }

        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId, string areaIdentifier, string accountId)
        {
            var update = table.TwoNestedCategory;

            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(update);
            var nodes = (await context.ConversationNodes.ToListAsync())
                .Where(x => x.IsDynamicTableNode && splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId)
                .OrderBy(x => x.ResolveOrder).ToList();
            if (nodes.Count > 0)
            {
                nodes.Single(x => x.ResolveOrder == 0).ValueOptions = splitter.JoinValueOptions(outerCategories);
                nodes.Single(x => x.ResolveOrder == 1).ValueOptions = splitter.JoinValueOptions(innerCategories);
            }
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var rawRows = await GetTableRows(dynamicTableMeta);
            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(rawRows);
            var widgetResponseKey = dynamicTableMeta.MakeUniqueIdentifier();

            // Outer-category
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Outer-Categories"),
                    dynamicTableMeta.ConvertToPrettyName("Outer"),
                    new List<string>() {"Continue"},
                    outerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    NodeTypeCode.X,
                    resolveOrder: 0,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey,
                    shouldRenderChildren: true
                ));

            // inner-categories
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Inner-Categories"),
                    dynamicTableMeta.ConvertToPrettyName("Inner"),
                    new List<string>() {"Continue"},
                    innerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    NodeTypeCode.X,
                    resolveOrder: 1,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey,
                    shouldRenderChildren: true
                ));
        }

        public class CategoryRetriever
        {
            public List<string> InnerCategories { get; set; }
            public List<string> OuterCategories { get; set; }

            public void Deconstruct(out List<string> innerCategories, out List<string> outerCategories)
            {
                innerCategories = InnerCategories;
                outerCategories = OuterCategories;
            }
        }
    }
}