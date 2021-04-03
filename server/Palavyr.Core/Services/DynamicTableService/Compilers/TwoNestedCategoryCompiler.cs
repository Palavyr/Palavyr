using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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

        public TwoNestedCategoryCompiler(IGenericDynamicTableRepository<TwoNestedCategory> repository, IConfigurationRepository configurationRepository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            // This table type does not facilitate multiple branches. I.e. the inner categories are all the same for all of the outer categories.
            var rows = (await GetTableRows(dynamicTableMeta)).OrderBy(row => row.RowOrder);
            var outerCategories = rows.Select(row => row.Category).ToList();

            var itemId = rows.Select(row => row.ItemId).Distinct().First();
            var innerCategories = rows.Where(row => row.ItemId == itemId).Select(row => row.SubCategory).ToList();

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
                    dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 0
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
                    dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 1
                ));
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
    }
}