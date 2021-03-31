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
            // interesting node - if we have outer categories as paths, then each inner category has to be a separate node
            // we would have as many inner nodes as there were outer categories
            // if we at first allow this as multichoiceContinue, then we extend it *later* to facilitate individual paths
            
            var rows = (await GetTableRows(dynamicTableMeta)).OrderBy(row => row.RowOrder);
            var categories = rows.Select(row => row.Category).ToList();

            // node for multiselect category (Category)
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier(),
                    dynamicTableMeta.ConvertToPrettyName(),
                    dynamicTableMeta.ValuesAsPaths ? categories : new List<string>() {"Continue"},
                    categories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 0
                ));

            // create node for sub category
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier(),
                    dynamicTableMeta.ConvertToPrettyName(),
                    new List<string>() {"Continue"},
                    new List<string>() {"Continue"},
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