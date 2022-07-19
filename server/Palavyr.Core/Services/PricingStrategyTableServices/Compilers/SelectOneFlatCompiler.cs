using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.PricingStrategyTableServices.NodeUpdaters;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.PricingStrategyTableServices.Compilers
{
    public interface ISelectOneFlatCompiler : IPricingStrategyTableCompiler
    {
    }

    public class SelectOneFlatCompiler : BaseCompiler<CategorySelectTableRow>, ISelectOneFlatCompiler
    {
        private readonly IEntityStore<CategorySelectTableRow> psStore;
        private readonly IConversationOptionSplitter splitter;
        private readonly ISelectOneFlatNodeUpdater selectOneFlatNodeUpdater;
        private readonly IResponseRetriever<CategorySelectTableRow> responseRetriever;
        private readonly IEntityStore<ConversationNode> convoNodeStore;
        private readonly IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore;

        public SelectOneFlatCompiler(
            IEntityStore<CategorySelectTableRow> psStore,
            IConversationOptionSplitter splitter,
            ISelectOneFlatNodeUpdater selectOneFlatNodeUpdater,
            IResponseRetriever<CategorySelectTableRow> responseRetriever,
            IEntityStore<ConversationNode> convoNodeStore,
            IEntityStore<PricingStrategyTableMeta> pricingStrategyTableMetaStore) : base(psStore, convoNodeStore)
        {
            this.psStore = psStore;
            this.splitter = splitter;
            this.selectOneFlatNodeUpdater = selectOneFlatNodeUpdater;
            this.responseRetriever = responseRetriever;
            this.convoNodeStore = convoNodeStore;
            this.pricingStrategyTableMetaStore = pricingStrategyTableMetaStore;
        }

        public async Task UpdateConversationNode<T>(List<T> table, string tableId, string intentId)
        {
            var currentSelectOneFlatUpdate = table as List<CategorySelectTableRow>;

            var tableMeta = await pricingStrategyTableMetaStore.Get(tableId, s => s.TableId);

            var conversationNodes = await convoNodeStore.GetMany(intentId, s => s.IntentId);
            var node = conversationNodes.SingleOrDefault(x => x.IsPricingStrategyTableNode && splitter.GetTableIdFromPricingStrategyNodeType(x.NodeType) == tableId);

            if (node != null && currentSelectOneFlatUpdate != null)
            {
                await selectOneFlatNodeUpdater.UpdateConversationNode(currentSelectOneFlatUpdate, tableMeta, node, conversationNodes, intentId);
            }

            // do not save the context changes here. Following the unit of work pattern,we collect all changes, validate, and then save/commit..
        }

        public async Task CompileToConfigurationNodes(
            PricingStrategyTableMeta pricingStrategyTableMeta,
            List<NodeTypeOptionResource> nodes)
        {
            var rows = await GetTableRows(pricingStrategyTableMeta);
            var valueOptions = rows.Select(x => x.Category).ToList();

            var nodeTypeOption = NodeTypeOptionResource.Create(
                pricingStrategyTableMeta.MakeUniqueIdentifier(),
                pricingStrategyTableMeta.ConvertToPrettyName(),
                pricingStrategyTableMeta.ValuesAsPaths ? valueOptions : new List<string>() { "Continue" },
                valueOptions,
                true,
                pricingStrategyTableMeta.ValuesAsPaths,
                false,
                NodeTypeOptionResource.CustomTables,
                pricingStrategyTableMeta.ValuesAsPaths ? DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceAsPath : DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                pricingStrategyTableMeta.ValuesAsPaths ? NodeTypeCode.XI : NodeTypeCode.X,
                shouldRenderChildren: true,
                pricingStrategyType: pricingStrategyTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(PricingStrategyResponseParts pricingStrategyResponseParts, List<string> pricingStrategyResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(pricingStrategyResponseIds);
            var responseValue = GetSingleResponseValue(pricingStrategyResponseParts, pricingStrategyResponseIds);

            var record = await RetrieveAllAvailableResponses(responseId);

            var option = record.Single(tableRow => tableRow.Category == responseValue);
            var pricingStrategyMeta = await pricingStrategyTableMetaStore.Get(option.TableId, s => s.TableId);

            var row = new TableRow(
                pricingStrategyMeta.UseTableTagAsResponseDescription ? pricingStrategyMeta.TableTag : option.Category,
                option.ValueMin,
                option.ValueMax,
                false,
                culture,
                option.Range);
            return new List<TableRow>() { row };
        }

        public Task<bool> PerformInternalCheck(ConversationNode node, string response, PricingStrategyResponseComponents pricingStrategyResponseComponents)
        {
            return Task.FromResult(false);
        }

        // private PricingStrategyValidationResult ValidationLogic(List<SelectOneFlat> table, string tableTag)
        // {
        //     var reasons = new List<string>();
        //     var valid = true;
        //
        //     var itemNames = table.Select(x => x.Option).ToList();
        //
        //     if (itemNames.Count() != itemNames.Distinct().Count())
        //     {
        //         reasons.Add($"Duplicate threshold values found in {tableTag}");
        //         valid = false;
        //     }
        //
        //     if (itemNames.Any(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)))
        //     {
        //         reasons.Add($"One or more categories did not contain text in {tableTag}");
        //         valid = false;
        //     }
        //
        //     return valid
        //         ? PricingStrategyValidationResult.CreateValid(tableTag)
        //         : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        // }

        // public PricingStrategyValidationResult ValidatePricingStrategyPreSave<T>(PricingStrategyTable<T> pricingStrategyTable)
        // {
        //     var table = pricingStrategyTable.TableData as List<SelectOneFlat>;
        //     var tableTag = pricingStrategyTable.TableTag;
        //     return ValidationLogic(table, tableTag);
        // }

        // public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(PricingStrategyTableMeta pricingStrategyTableMeta)
        // {
        //     var tableId = pricingStrategyTableMeta.TableId;
        //
        //     var table = await psStore.GetMany(tableId, s => s.TableId);
        //     return ValidationLogic(table.ToList(), pricingStrategyTableMeta.TableTag);
        // }

        public async Task<List<TableRow>> CreatePreviewData(PricingStrategyTableMeta tableTableMeta, Intent intent, CultureInfo culture)
        {
            var availableOneFlat = await responseRetriever.RetrieveAllAvailableResponses(tableTableMeta.TableId);
            var responseParts = PricingStrategyResponsePartJoiner.CreatePricingStrategyResponseParts(availableOneFlat.First().TableId, availableOneFlat.First().Category);
            var currentRows = await CompileToPdfTableRow(responseParts, new List<string>() { tableTableMeta.TableId }, culture);
            return currentRows;
        }

        public async Task<List<CategorySelectTableRow>> RetrieveAllAvailableResponses(string responseId)
        {
            return await GetAllRowsMatchingResponseId(responseId);
        }
    }
}