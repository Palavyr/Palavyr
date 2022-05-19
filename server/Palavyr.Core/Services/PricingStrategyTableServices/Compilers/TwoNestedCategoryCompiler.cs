using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices.Compilers
{
    public interface ITwoNestedCategoryCompiler : IPricingStrategyTableCompiler
    {
    }

    public class TwoNestedCategoryCompiler : BaseCompiler<TwoNestedCategory>, ITwoNestedCategoryCompiler
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IConversationOptionSplitter splitter;
        private readonly IResponseRetriever responseRetriever;
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;

        public TwoNestedCategoryCompiler(
            IEntityStore<ConversationNode> convoNodeStore,
            IPricingStrategyEntityStore<TwoNestedCategory> repository,
            IConversationOptionSplitter splitter,
            IResponseRetriever responseRetriever,
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore) : base(repository)
        {
            this.convoNodeStore = convoNodeStore;
            this.splitter = splitter;
            this.responseRetriever = responseRetriever;
            this.dynamicTableMetaStore = dynamicTableMetaStore;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(DynamicResponseParts dynamicResponseParts, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(dynamicResponseIds);
            var records = await RetrieveAllAvailableResponses(responseId);

            // itemName
            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(dynamicResponseParts);
            var outerCategory = GetResponseByResponseId(orderedResponseIds[0], dynamicResponseParts);
            var innerCategory = GetResponseByResponseId(orderedResponseIds[1], dynamicResponseParts);

            var result = records.Single(rec => rec.ItemName == outerCategory && rec.InnerItemName == innerCategory);
            var dynamicTableMeta = await dynamicTableMetaStore.Get(result.TableId, s => s.TableId);

            return new List<TableRow>()
            {
                new TableRow(
                    dynamicTableMeta.UseTableTagAsResponseDescription ? dynamicTableMeta.TableTag : string.Join(" & ", new[] { result.ItemName, result.InnerItemName }),
                    result.ValueMin,
                    result.ValueMax,
                    false,
                    culture,
                    result.Range
                )
            };
        }


        public Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents pricingStrategyResponseComponents)
        {
            return Task.FromResult(false);
        }

        public PricingStrategyValidationResult ValidationLogic(IEnumerable<TwoNestedCategory> table, string tableTag)
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

            var itemId = itemIds.First();
            var innerItemNames = table.Where(x => x.ItemId == itemId).Select(x => x.InnerItemName).ToList();
            if (innerItemNames.Count() != innerItemNames.Distinct().Count())
            {
                reasons.Add($"Duplicate inner category values found in {tableTag}");
                valid = false;
            }

            if (innerItemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
            {
                reasons.Add($"One or more inner categories did not contain text in {tableTag}");
                valid = false;
            }

            return valid
                ? PricingStrategyValidationResult.CreateValid(tableTag)
                : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        }

        public PricingStrategyValidationResult ValidatePricingStrategyPreSave<T>(PricingStrategyTable<T> pricingStrategyTable)
        {
            var table = pricingStrategyTable.TableData as List<TwoNestedCategory>;
            var tableTag = pricingStrategyTable.TableTag;
            return ValidationLogic(table, tableTag);
        }

        public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(DynamicTableMeta dynamicTableMeta)
        {
            var tableId = dynamicTableMeta.TableId;
            var areaId = dynamicTableMeta.AreaIdentifier;
            var table = await repository.GetAllRows(areaId, tableId);
            return ValidationLogic(table, dynamicTableMeta.TableTag);
        }

        public async Task<List<TableRow>> CreatePreviewData(DynamicTableMeta tableMeta, Area _, CultureInfo culture)
        {
            var availableTwoNested = await responseRetriever.RetrieveAllAvailableResponses<TwoNestedCategory>(tableMeta.TableId);
            var currentRows = new List<TableRow>()
            {
                new TableRow(
                    tableMeta.UseTableTagAsResponseDescription ? tableMeta.TableTag : string.Join(" & ", new[] { availableTwoNested.First().ItemName, availableTwoNested.First().InnerItemName }),
                    availableTwoNested.First().ValueMin,
                    availableTwoNested.First().ValueMax,
                    false,
                    culture,
                    availableTwoNested.First().Range
                )
            };
            return currentRows;
        }

        public async Task<List<TwoNestedCategory>> RetrieveAllAvailableResponses(string dynamicResponseId)
        {
            return await GetAllRowsMatchingResponseId(dynamicResponseId);
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

        public async Task UpdateConversationNode<T>(PricingStrategyTable<T> table, string tableId, string areaIdentifier)
        {
            var update = table.TableData as List<TwoNestedCategory>;

            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(update);

            var nodes = await convoNodeStore.Query()
                .Where(x => x.IsDynamicTableNode && splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId)
                .OrderBy(x => x.ResolveOrder).ToListAsync(convoNodeStore.CancellationToken);

            if (nodes.Count > 0)
            {
                nodes.Single(x => x.ResolveOrder == 0).ValueOptions = splitter.JoinValueOptions(outerCategories);
                nodes.Single(x => x.ResolveOrder == 1).ValueOptions = splitter.JoinValueOptions(innerCategories);
            }
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOptionResource> nodes)
        {
            var rawRows = await GetTableRows(dynamicTableMeta);
            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(rawRows);
            var widgetResponseKey = dynamicTableMeta.MakeUniqueIdentifier();

            // Outer-category
            nodes.AddAdditionalNode(
                NodeTypeOptionResource.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Outer-Categories"),
                    dynamicTableMeta.ConvertToPrettyName("Outer"),
                    new List<string>() { "Continue" },
                    outerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOptionResource.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    NodeTypeCode.X,
                    resolveOrder: 0,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey,
                    shouldRenderChildren: true
                ));

            // inner-categories
            nodes.AddAdditionalNode(
                NodeTypeOptionResource.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Inner-Categories"),
                    dynamicTableMeta.ConvertToPrettyName("Inner"),
                    new List<string>() { "Continue" },
                    innerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOptionResource.CustomTables,
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