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
using Palavyr.Core.Services.DynamicTableService.NodeUpdaters;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public class SelectOneFlatCompiler : BaseCompiler<SelectOneFlat>, IDynamicTablesCompiler
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConversationOptionSplitter splitter;
        private readonly ISelectOneFlatNodeUpdater selectOneFlatNodeUpdater;
        private readonly IResponseRetriever responseRetriever;

        public SelectOneFlatCompiler(
            IGenericDynamicTableRepository<SelectOneFlat> repository,
            IConfigurationRepository configurationRepository,
            IConversationOptionSplitter splitter,
            ISelectOneFlatNodeUpdater selectOneFlatNodeUpdater,
            IResponseRetriever responseRetriever
            

        ) : base(repository)
        {
            this.configurationRepository = configurationRepository;
            this.splitter = splitter;
            this.selectOneFlatNodeUpdater = selectOneFlatNodeUpdater;
            this.responseRetriever = responseRetriever;
        }

        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId, string areaIdentifier)
        {
            var currentSelectOneFlatUpdate = table.SelectOneFlat;

            var tableMeta = await context.DynamicTableMetas.SingleOrDefaultAsync(x => x.TableId == tableId);

            var conversationNodes = await configurationRepository.GetAreaConversationNodes(areaIdentifier);
            var node = conversationNodes.SingleOrDefault(x => x.IsDynamicTableNode && splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId);

            if (node != null && currentSelectOneFlatUpdate != null)
            {
                await selectOneFlatNodeUpdater.UpdateConversationNode(context, currentSelectOneFlatUpdate, tableMeta, node, conversationNodes, areaIdentifier);
            }

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
                dynamicTableMeta.ValuesAsPaths ? NodeTypeCode.XI : NodeTypeCode.X,
                shouldRenderChildren: true,
                dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(DynamicResponseParts dynamicResponseParts, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponseParts, dynamicResponseIds);

            var record = await RetrieveAllAvailableResponses(dynamicResponseId);

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

        public Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents dynamicResponseComponents)
        {
            return Task.FromResult(false);
        }

        private PricingStrategyValidationResult ValidationLogic(List<SelectOneFlat> table, string tableTag)
        {
            var reasons = new List<string>();
            var valid = true;

            var itemNames = table.Select(x => x.Option).ToList();

            if (itemNames.Count() != itemNames.Distinct().Count())
            {
                reasons.Add($"Duplicate threshold values found in {tableTag}");
                valid = false;
            }

            if (itemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
            {
                reasons.Add($"One or more categories did not contain text in {tableTag}");
                valid = false;
            }

            return valid
                ? PricingStrategyValidationResult.CreateValid(tableTag)
                : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        }

        public PricingStrategyValidationResult ValidatePricingStrategyPreSave(DynamicTable dynamicTable)
        {
            var table = dynamicTable.SelectOneFlat;
            var tableTag = dynamicTable.TableTag;
            return ValidationLogic(table, tableTag);
        }

        public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(DynamicTableMeta dynamicTableMeta)
        {
            var tableId = dynamicTableMeta.TableId;
            var areaId = dynamicTableMeta.AreaIdentifier;
            var table = await repository.GetAllRows(areaId, tableId);
            return ValidationLogic(table, dynamicTableMeta.TableTag);
        }

        public async Task<List<TableRow>> CreatePreviewData(DynamicTableMeta tableMeta, Area area, CultureInfo culture)
        {
            var availableOneFlat = await responseRetriever.RetrieveAllAvailableResponses<SelectOneFlat>(tableMeta.TableId);
            var responseParts = DynamicTableTypes.CreateSelectOneFlat().CreateDynamicResponseParts(availableOneFlat.First().TableId, availableOneFlat.First().Option);
            var currentRows = await CompileToPdfTableRow(responseParts, new List<string>() {tableMeta.TableId}, culture);
            return currentRows;
        }

        public async Task<List<SelectOneFlat>> RetrieveAllAvailableResponses(string dynamicResponseId)
        {
            return await GetAllRowsMatchingResponseId(dynamicResponseId);
        }
    }
}