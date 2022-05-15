using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Requests;
using Palavyr.Core.Services.DynamicTableService.NodeUpdaters;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public interface ISelectOneFlatCompiler : IDynamicTablesCompiler
    {
    }

    public class SelectOneFlatCompiler : BaseCompiler<SelectOneFlat>, ISelectOneFlatCompiler
    {
        private readonly IConversationOptionSplitter splitter;
        private readonly ISelectOneFlatNodeUpdater selectOneFlatNodeUpdater;
        private readonly IResponseRetriever responseRetriever;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<DynamicTableMeta> dynamicTableMetaStore;

        public SelectOneFlatCompiler(
            IPricingStrategyEntityStore<SelectOneFlat> repository,
            IConversationOptionSplitter splitter,
            ISelectOneFlatNodeUpdater selectOneFlatNodeUpdater,
            IResponseRetriever responseRetriever,
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<DynamicTableMeta> dynamicTableMetaStore) : base(repository)
        {
            this.splitter = splitter;
            this.selectOneFlatNodeUpdater = selectOneFlatNodeUpdater;
            this.responseRetriever = responseRetriever;
            this.convoNodeStore = convoNodeStore;
            this.dynamicTableMetaStore = dynamicTableMetaStore;
        }

        public async Task UpdateConversationNode(DynamicTable table, string tableId, string areaIdentifier)
        {
            var currentSelectOneFlatUpdate = table.SelectOneFlat;

            var tableMeta = await dynamicTableMetaStore.Get(tableId, s => s.TableId);

            var conversationNodes = await convoNodeStore.GetMany(areaIdentifier, s => s.AreaIdentifier);
            var node = conversationNodes.SingleOrDefault(x => x.IsDynamicTableNode && splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId);

            if (node != null && currentSelectOneFlatUpdate != null)
            {
                await selectOneFlatNodeUpdater.UpdateConversationNode(currentSelectOneFlatUpdate, tableMeta, node, conversationNodes, areaIdentifier);
            }

            // do not save the context changes here. Following the unit of work pattern,we collect all changes, validate, and then save/commit..
        }

        public async Task CompileToConfigurationNodes(
            DynamicTableMeta dynamicTableMeta,
            List<NodeTypeOptionResource> nodes)
        {
            var rows = await GetTableRows(dynamicTableMeta);
            var valueOptions = rows.Select(x => x.Option).ToList();

            var nodeTypeOption = NodeTypeOptionResource.Create(
                dynamicTableMeta.MakeUniqueIdentifier(),
                dynamicTableMeta.ConvertToPrettyName(),
                dynamicTableMeta.ValuesAsPaths ? valueOptions : new List<string>() { "Continue" },
                valueOptions,
                true,
                dynamicTableMeta.ValuesAsPaths,
                false,
                NodeTypeOptionResource.CustomTables,
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
            var dynamicMeta = await dynamicTableMetaStore.Get(option.TableId, s => s.TableId);

            var row = new TableRow(
                dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : option.Option,
                option.ValueMin,
                option.ValueMax,
                false,
                culture,
                option.Range);
            return new List<TableRow>() { row };
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
            return ValidationLogic(table.ToList(), dynamicTableMeta.TableTag);
        }

        public async Task<List<TableRow>> CreatePreviewData(DynamicTableMeta tableMeta, Area area, CultureInfo culture)
        {
            var availableOneFlat = await responseRetriever.RetrieveAllAvailableResponses<SelectOneFlat>(tableMeta.TableId);
            var responseParts = PricingStrategyTableTypes.CreateSelectOneFlat().CreateDynamicResponseParts(availableOneFlat.First().TableId, availableOneFlat.First().Option);
            var currentRows = await CompileToPdfTableRow(responseParts, new List<string>() { tableMeta.TableId }, culture);
            return currentRows;
        }

        public async Task<List<SelectOneFlat>> RetrieveAllAvailableResponses(string dynamicResponseId)
        {
            return await GetAllRowsMatchingResponseId(dynamicResponseId);
        }
    }
}