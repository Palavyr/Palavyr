using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Common.UIDUtils;
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

        public TwoNestedCategoryCompiler(IGenericDynamicTableRepository<TwoNestedCategory> repository, IConfigurationRepository configurationRepository) : base(repository)
        {
            this.repository = repository;
            this.configurationRepository = configurationRepository;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponse dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(dynamicResponseIds);
            var records = await repository.GetAllRowsMatchingDynamicResponseId(accountId, responseId);

            // itemName then Count
            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(dynamicResponse);
            var categoryFilterResponse = GetResponseByResponseId(orderedResponseIds[0], dynamicResponse);
            var countFilterResponse = GetResponseByResponseId(orderedResponseIds[1], dynamicResponse);

            var result = records.Single(rec => rec.Category == categoryFilterResponse && rec.SubCategory == countFilterResponse);
            return new List<TableRow>()
            {
                new TableRow(
                    string.Join("&", new[] {result.Category, result.SubCategory}),
                    result.ValueMin,
                    result.ValueMax,
                    false,
                    culture,
                    result.Range
                )
            };
        }

        private async Task<CategoryRetriever> GetInnerAndOuterCategories(DynamicTableMeta dynamicTableMeta)
        {
            // This table type does not facilitate multiple branches. I.e. the inner categories are all the same for all of the outer categories.
            var rawRows = await GetTableRows(dynamicTableMeta);
            var rows = rawRows.OrderBy(row => row.RowOrder).ToList();

            var outerCategories = rows.Select(row => row.Category).Distinct().ToList();

            var itemId = rows.Select(row => row.ItemId).Distinct().First();
            var innerCategories = rows.Where(row => row.ItemId == itemId).Select(row => row.SubCategory).ToList();

            return new CategoryRetriever
            {
                InnerCategories = innerCategories,
                OuterCategories = outerCategories
            };
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {

            var (innerCategories, outerCategories) = await GetInnerAndOuterCategories(dynamicTableMeta);
            var widgetResponseKey = dynamicTableMeta.MakeUniqueIdentifier();

            // Outer-category
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Outer-Categories", GuidUtils.CreateShortenedGuid(1)),
                    dynamicTableMeta.ConvertToPrettyName("Outer"),
                    new List<string>() {"Continue"},
                    outerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, //dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 0,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey
                ));

            // inner-categories
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Inner-Categories", GuidUtils.CreateShortenedGuid(1)),
                    dynamicTableMeta.ConvertToPrettyName("Inner"),
                    new List<string>() {"Continue"},
                    innerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, //dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 1,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey
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