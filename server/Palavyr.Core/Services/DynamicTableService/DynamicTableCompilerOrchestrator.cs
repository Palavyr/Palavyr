using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService
{
    public class DynamicTableCompilerOrchestrator : IDynamicTableCompilerOrchestrator
    {
        private readonly DynamicTableCompilerRetriever dynamicTableCompilerRetriever;

        public DynamicTableCompilerOrchestrator(
            DynamicTableCompilerRetriever dynamicTableCompilerRetriever
        )
        {
            this.dynamicTableCompilerRetriever = dynamicTableCompilerRetriever;
        }

        public async Task<List<Table>> CompileTablesToPdfRows(
            string accountId,
            List<Dictionary<string, DynamicResponse>> dynamicResponses,
            CultureInfo culture
        )
        {
            var tableRows = new List<TableRow>();
            foreach (var dynamicResponse in dynamicResponses)
            {
                var dynamicTableKeys = dynamicResponse.Keys.ToList(); // in the future, there could be multiple key values
                // for now, we are only expecting one key. Not sure yet how we can combine multiple tables or if there is even a point to doing this.
                if (dynamicTableKeys.Count > 1) throw new Exception("Multiple dynamic table keys specified. This is a configuration error");
                var dynamicTableKey = dynamicTableKeys[0];
                var responses = dynamicResponse[dynamicTableKey];


                var dynamicTableName = dynamicTableKey.Split("-").First();
                var compiler = dynamicTableCompilerRetriever.RetrieveCompiler(dynamicTableName);

                List<TableRow> rows;
                rows = await compiler.CompileToPdfTableRow(accountId, responses, dynamicTableKeys, culture);
                tableRows.AddRange(rows);
            }

            var table = new Table("Variable estimates determined by your responses", tableRows, culture);
            return new List<Table>() {table};
        }

        public async Task<List<NodeTypeOption>> CompileTablesToConfigurationNodes(
            IEnumerable<DynamicTableMeta> dynamicTableMetas,
            string accountId,
            string areaId
        )
        {
            var nodes = new List<NodeTypeOption>() { };
            foreach (var dynamicTableMeta in dynamicTableMetas)
            {
                var compiler = dynamicTableCompilerRetriever.RetrieveCompiler(dynamicTableMeta.TableType);
                await compiler.CompileToConfigurationNodes(dynamicTableMeta, nodes);
            }

            return nodes;
        }

        // public Task<List<ConversationNode>> CompileToConversationNode(IEnumerable<DynamicTableMeta> dynamicTableMetas, string accountId, string areaId)
        // {
        //     var convoNodes = new List<ConversationNode>();
        //     foreach (var dynamicTableMeta in dynamicTableMetas)
        //     {
        //         var compiler = dynamicTableCompilerRetriever.RetrieveCompiler(dynamicTableMeta.TableType);
        //         await compiler.
        //     }
        // }
    }
}
        //
        // {
        //     // This table type does not facilitate multiple branches. I.e. the inner categories are all the same for all of the outer categories.
        //     var rows = (await GetTableRows(dynamicTableMeta)).OrderBy(row => row.RowOrder);
        //     var outerCategories = rows.Select(row => row.Category).ToList();
        //
        //     var itemId = rows.Select(row => row.ItemId).Distinct().First();
        //     var innerCategories = rows.Where(row => row.ItemId == itemId).Select(row => row.SubCategory).ToList();
        //
        //     // Outer-category
        //     nodes.AddAdditionalNode(
        //         NodeTypeOption.Create(
        //             dynamicTableMeta.MakeUniqueIdentifier("Outer-Categories", GuidUtils.CreateShortenedGuid(1)),
        //             dynamicTableMeta.ConvertToPrettyName("Outer"),
        //             new List<string>() {"Continue"},
        //             outerCategories.Distinct().ToList(),
        //             true,
        //             true,
        //             false,
        //             NodeTypeOption.CustomTables,
        //             dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
        //             resolveOrder: 0
        //         ));
        //
        //     // inner-categories
        //     nodes.AddAdditionalNode(
        //         NodeTypeOption.Create(
        //             dynamicTableMeta.MakeUniqueIdentifier("Inner-Categories", GuidUtils.CreateShortenedGuid(1)),
        //             dynamicTableMeta.ConvertToPrettyName("Inner"),
        //             new List<string>() {"Continue"},
        //             innerCategories,
        //             true,
        //             true,
        //             false,
        //             NodeTypeOption.CustomTables,
        //             dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
        //             resolveOrder: 1
        //         ));
