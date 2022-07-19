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
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices.Compilers
{
    public interface ITwoNestedCategoryCompiler : IPricingStrategyTableCompiler
    {
    }

    public class TwoNestedCategoryCompiler : BaseCompiler<TwoNestedSelectTableRow>, ITwoNestedCategoryCompiler
    {
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<TwoNestedSelectTableRow> psStore;
        private readonly IConversationOptionSplitter splitter;
        private readonly IResponseRetriever<TwoNestedSelectTableRow> responseRetriever;
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;

        public TwoNestedCategoryCompiler(
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<TwoNestedSelectTableRow> psStore,
            IConversationOptionSplitter splitter,
            IResponseRetriever<TwoNestedSelectTableRow> responseRetriever,
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore) : base(psStore, convoNodeStore)
        {
            this.convoNodeStore = convoNodeStore;
            this.psStore = psStore;
            this.splitter = splitter;
            this.responseRetriever = responseRetriever;
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(PricingStrategyResponseParts pricingStrategyResponseParts, List<string> pricingStrategyResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(pricingStrategyResponseIds);
            var records = await RetrieveAllAvailableResponses(responseId);

            // itemName
            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(pricingStrategyResponseParts);
            var outerCategory = GetResponseByResponseId(orderedResponseIds[0], pricingStrategyResponseParts);
            var innerCategory = GetResponseByResponseId(orderedResponseIds[1], pricingStrategyResponseParts);

            var result = records.Single(rec => rec.Category == outerCategory && rec.InnerItemName == innerCategory);
            var pricingStrategyTableMeta = await pricingStrategyTableMetaStore.Get(result.TableId, s => s.TableId);

            return new List<TableRow>()
            {
                new TableRow(
                    pricingStrategyTableMeta.UseTableTagAsResponseDescription ? pricingStrategyTableMeta.TableTag : string.Join(" & ", new[] { result.Category, result.InnerItemName }),
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

        // public PricingStrategyValidationResult ValidationLogic(IEnumerable<TwoNestedCategory> table, string tableTag)
        // {
        //     var reasons = new List<string>();
        //     var valid = true;
        //
        //     var itemIds = table.Select(x => x.ItemId).Distinct().ToList();
        //     var itemNames = table.Select(x => x.ItemName).Distinct().ToList();
        //     var numCategories = itemIds.Count();
        //     if (itemNames.Count() != numCategories)
        //     {
        //         reasons.Add($"Duplicate outer category values found in {tableTag}");
        //         valid = false;
        //     }
        //
        //     if (itemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
        //     {
        //         reasons.Add($"One or more outer categories did not contain text in {tableTag}");
        //         valid = false;
        //     }
        //
        //     var itemId = itemIds.First();
        //     var innerItemNames = table.Where(x => x.ItemId == itemId).Select(x => x.InnerItemName).ToList();
        //     if (innerItemNames.Count() != innerItemNames.Distinct().Count())
        //     {
        //         reasons.Add($"Duplicate inner category values found in {tableTag}");
        //         valid = false;
        //     }
        //
        //     if (innerItemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
        //     {
        //         reasons.Add($"One or more inner categories did not contain text in {tableTag}");
        //         valid = false;
        //     }
        //
        //     return valid
        //         ? PricingStrategyValidationResult.CreateValid(tableTag)
        //         : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        // }

        // public PricingStrategyValidationResult ValidatePricingStrategyPreSave<T>(PricingStrategyTable<T> pricingStrategyTable)
        // {
        //     var table = pricingStrategyTable.TableData as List<TwoNestedCategory>;
        //     var tableTag = pricingStrategyTable.TableTag;
        //     return ValidationLogic(table, tableTag);
        // }

        // public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(PricingStrategyTableMeta pricingStrategyTableMeta)
        // {
        //     var tableId = pricingStrategyTableMeta.TableId;
        //
        //     var table = await psStore.GetMany(tableId, s => s.TableId);
        //     return ValidationLogic(table, pricingStrategyTableMeta.TableTag);
        // }

        public async Task<List<TableRow>> CreatePreviewData(PricingStrategyTableMeta tableTableMeta, Intent intent, CultureInfo culture)
        {
            var availableTwoNested = await responseRetriever.RetrieveAllAvailableResponses(tableTableMeta.TableId);
            var currentRows = new List<TableRow>()
            {
                new TableRow(
                    tableTableMeta.UseTableTagAsResponseDescription ? tableTableMeta.TableTag : string.Join(" & ", new[] { availableTwoNested.First().Category, availableTwoNested.First().InnerItemName }),
                    availableTwoNested.First().ValueMin,
                    availableTwoNested.First().ValueMax,
                    false,
                    culture,
                    availableTwoNested.First().Range
                )
            };
            return currentRows;
        }

        public async Task<List<TwoNestedSelectTableRow>> RetrieveAllAvailableResponses(string responseId)
        {
            return await GetAllRowsMatchingResponseId(responseId);
        }

        private CategoryRetriever GetInnerAndOuterCategories(List<TwoNestedSelectTableRow> rawRows)
        {
            // This table type does not facilitate multiple branches. I.e. the inner categories are all the same for all of the outer categories.
            var rows = rawRows.OrderBy(row => row.RowOrder).ToList();

            var outerCategories = rows.Select(row => row.Category).Distinct().ToList();

            var itemId = rows.Select(row => row.ItemId).Distinct().First();
            var innerCategories = rows.Where(row => row.ItemId == itemId).Select(row => row.InnerItemName).ToList();

            return new CategoryRetriever
            {
                InnerCategories = innerCategories,
                OuterCategories = outerCategories
            };
        }

        public async Task UpdateConversationNode<T>(List<T> table, string tableId, string intentId)
        {
            var update = table as List<TwoNestedSelectTableRow>;

            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(update);

            var nodes = await convoNodeStore.Query()
                .Where(x => x.IsPricingStrategyTableNode && splitter.GetTableIdFromPricingStrategyNodeType(x.NodeType) == tableId)
                .OrderBy(x => x.ResolveOrder).ToListAsync(convoNodeStore.CancellationToken);

            if (nodes.Count > 0)
            {
                nodes.Single(x => x.ResolveOrder == 0).ValueOptions = splitter.JoinValueOptions(outerCategories);
                nodes.Single(x => x.ResolveOrder == 1).ValueOptions = splitter.JoinValueOptions(innerCategories);
            }
        }

        public async Task CompileToConfigurationNodes(PricingStrategyTableMeta pricingStrategyTableMeta, List<NodeTypeOptionResource> nodes)
        {
            var rawRows = await GetTableRows(pricingStrategyTableMeta);
            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(rawRows);
            var widgetResponseKey = pricingStrategyTableMeta.MakeUniqueIdentifier();

            // Outer-category
            nodes.AddAdditionalNode(
                NodeTypeOptionResource.Create(
                    pricingStrategyTableMeta.MakeUniqueIdentifier("Outer-Categories"),
                    pricingStrategyTableMeta.ConvertToPrettyName("Outer"),
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
                    pricingStrategyType: widgetResponseKey,
                    shouldRenderChildren: true
                ));

            // inner-categories
            nodes.AddAdditionalNode(
                NodeTypeOptionResource.Create(
                    pricingStrategyTableMeta.MakeUniqueIdentifier("Inner-Categories"),
                    pricingStrategyTableMeta.ConvertToPrettyName("Inner"),
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
                    pricingStrategyType: widgetResponseKey,
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