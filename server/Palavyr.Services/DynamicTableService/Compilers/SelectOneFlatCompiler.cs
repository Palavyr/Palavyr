using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Configuration.Schemas.DynamicTables;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.PdfService.PdfSections.Util;
using Palavyr.Services.Repositories;

namespace Palavyr.Services.DynamicTableService.Compilers
{
    public class SelectOneFlatCompiler : BaseCompiler<SelectOneFlat>, IDynamicTablesCompiler
    {

        public SelectOneFlatCompiler(IGenericDynamicTableRepository<SelectOneFlat> repository) : base(repository)
        {
        }

        public async Task CompileToConfigurationNodes(
            DynamicTableMeta dynamicTableMeta,
            List<NodeTypeOption> nodes)
        {
            var rows = await GetTableRows(dynamicTableMeta);
            var valueOptions = rows.Select(x => x.Option).ToList();

            var nodeTypeOption = NodeTypeOption.Create(
                dynamicTableMeta.MakeUniqueIdentifier(),
                dynamicTableMeta.ConvertToPrettyName(),
                dynamicTableMeta.ValuesAsPaths ? valueOptions : new List<string>() {"Continue"},
                valueOptions,
                true,
                true,
                false,
                NodeTypeOption.CustomTables,
                dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue
            );
            nodes.AddAdditionalNode(nodeTypeOption);
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponse dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var record = await Repository
                .GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);

            var option = record.Single(tableRow => tableRow.Option == responseValue);

            var row = new TableRow(
                option.Option,
                option.ValueMin,
                option.ValueMax,
                false,
                culture,
                option.Range);
            return new List<TableRow>() {row};
        }
    }
}