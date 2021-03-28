using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Configuration.Schemas.DynamicTables;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.DatabaseService;
using Palavyr.Services.PdfService.PdfSections.Util;
using Palavyr.Services.Repositories;

namespace Palavyr.Services.DynamicTableService.Compilers
{
    public class CategorySelectCountCompiler : BaseCompiler<CategorySelectCount>, IDynamicTablesCompiler
    {
        private readonly IGenericDynamicTableRepository<CategorySelectCount> repository;
        private readonly IDashConnector dashConnector;

        public CategorySelectCountCompiler(IGenericDynamicTableRepository<CategorySelectCount> repository, IDashConnector dashConnector) : base(repository)
        {
            this.repository = repository;
            this.dashConnector = dashConnector;
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var rows = (await GetTableRows(dynamicTableMeta)).OrderBy(row => row.RowOrder);
            var categories = rows.Select(row => row.ItemName).ToList();

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

            // create node for multi-select (counts)
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
            
            var result = records.Single(rec => rec.ItemName == categoryFilterResponse && rec.Count == countFilterResponse);
            return new List<TableRow>()
            {
                new TableRow(
                    result.ItemName,
                    result.ValueMin,
                    result.ValueMax,
                    false,
                    culture,
                    result.Range
                )
            };
        }

        private string GetResponseByResponseId(string responseId, DynamicResponse dynamicResponse)
        {
            return dynamicResponse.ResponseComponents.Single(x => x.ContainsKey(responseId)).Values.ToList().Single();
        }

        private async Task<List<string>> GetResponsesOrderedByResolveOrder(DynamicResponse dynamicResponse)
        {
            var responseKeys = dynamicResponse.ResponseComponents.SelectMany(row => row.Keys).ToList();
            return (await dashConnector.GetConversationNodeByIds(responseKeys))
                .OrderBy(row => row.ResolveOrder)
                .Select(x => x.NodeId)
                .ToList();
        }
    }
}