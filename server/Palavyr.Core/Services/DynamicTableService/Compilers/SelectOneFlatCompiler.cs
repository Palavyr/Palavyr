﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public class SelectOneFlatCompiler : BaseCompiler<SelectOneFlat>, IDynamicTablesCompiler
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConversationOptionSplitter splitter;

        public SelectOneFlatCompiler(
            IGenericDynamicTableRepository<SelectOneFlat> repository,
            IConfigurationRepository configurationRepository,
            IConversationOptionSplitter splitter
        ) : base(repository)
        {
            this.configurationRepository = configurationRepository;
            this.splitter = splitter;
        }

        public void UpdateConversationNode(DashContext context, DynamicTable table, string tableId)
        {
            var currentSelectOneFlatUpdate = table.SelectOneFlat;
            var valueOptionString = string.Join(Delimiters.ValueOptionDelimiter, currentSelectOneFlatUpdate.Select(x => x.Option));
            var node = context.ConversationNodes.Single(x => splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId);
            node.ValueOptions = valueOptionString;
            // do not save the context changes here. Following the unit of work pattern,we collect all changes, validate, and then save/commit..
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
                dynamicTableMeta.ValuesAsPaths,
                false,
                NodeTypeOption.CustomTables,
                dynamicTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, List<Dictionary<string, string>> dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var record = await Repository
                .GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);

            var option = record.Single(tableRow => tableRow.Option == responseValue);
            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(option.TableId);

            var row = new TableRow(
                dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : option.Option,
                option.ValueMin,
                option.ValueMax,
                false,
                culture,
                option.Range);
            return new List<TableRow>() {row};
        }
    }
}