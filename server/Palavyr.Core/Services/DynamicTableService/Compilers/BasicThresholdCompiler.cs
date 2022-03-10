﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Aliases;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.DynamicTableService.Thresholds;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public interface IBasicThresholdCompiler : IDynamicTablesCompiler
    {
    }

    public class BasicThresholdCompiler : BaseCompiler<BasicThreshold>, IBasicThresholdCompiler
    {
        private readonly IEntityStore<DynamicTableMeta> dynamicTableStore;
        private readonly IThresholdEvaluator thresholdEvaluator;
        private readonly IResponseRetriever responseRetriever;

        public BasicThresholdCompiler(
            IEntityStore<DynamicTableMeta> dynamicTableStore,
            IPricingStrategyEntityStore<BasicThreshold> repository,
            IThresholdEvaluator thresholdEvaluator,
            IResponseRetriever responseRetriever
        ) : base(repository)
        {
            this.dynamicTableStore = dynamicTableStore;
            this.thresholdEvaluator = thresholdEvaluator;
            this.responseRetriever = responseRetriever;
        }

        public async Task UpdateConversationNode(DashContext dashContext, DynamicTable table, string tableId, string areaIdentifier)
        {
            await Task.CompletedTask;
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var nodeTypeOption = NodeTypeOption.Create(
                dynamicTableMeta.MakeUniqueIdentifier(),
                dynamicTableMeta.ConvertToPrettyName(),
                new List<string>() { "Continue" },
                new List<string>() { "Continue" },
                true,
                false,
                false,
                NodeTypeOption.CustomTables,
                DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber,
                NodeTypeCode.II,
                resolveOrder: 0,
                dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
            await Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(DynamicResponseParts dynamicResponseParts, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponseParts, dynamicResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await RetrieveAllAvailableResponses(dynamicResponseId);

            var dynamicMeta = await dynamicTableStore.Get(allRows.First().TableId, s => s.TableId);

            var itemsToCreateRowsFor = allRows.Where(x => !string.IsNullOrWhiteSpace(x.ItemName)).Select(row => row.ItemName).Distinct();

            var tableRows = new List<TableRow>();

            foreach (var itemName in itemsToCreateRowsFor)
            {
                var itemRows = allRows.Where(row => row.ItemName == itemName).ToList();
                if (itemRows.Count > 0)
                {
                    var thresholdResult = thresholdEvaluator.Evaluate(responseValueAsDouble, itemRows);
                    tableRows.Add(
                        new TableRow(
                            dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : itemName,
                            thresholdResult.ValueMin,
                            thresholdResult.ValueMax,
                            false,
                            culture,
                            thresholdResult.Range
                        )
                    );
                }
            }

            return tableRows;
        }

        public async Task<bool> PerformInternalCheck(ConversationNode node, string response, DynamicResponseComponents _)
        {
            var thresholds = await repository.GetAllRowsMatchingDynamicResponseId(node.DynamicType);
            var currentResponseAsDouble = double.Parse(response);
            var isTooComplicated = thresholdEvaluator.EvaluateForFallback(currentResponseAsDouble, thresholds);
            return isTooComplicated;
        }

        private PricingStrategyValidationResult ValidationLogic(List<BasicThreshold> table, string tableTag)
        {
            var reasons = new List<string>();
            var valid = true;

            if (table.Select(x => x.Threshold).Distinct().Count() != table.Count)
            {
                reasons.Add($"Duplicate threshold values found in {tableTag}");
                valid = false;
            }

            if (table.Any(x => x.Threshold < 0))
            {
                reasons.Add($"Negative threshold value found in {tableTag}");
                valid = false;
            }


            return valid
                ? PricingStrategyValidationResult.CreateValid(tableTag)
                : PricingStrategyValidationResult.CreateInvalid(tableTag, reasons);
        }

        public PricingStrategyValidationResult ValidatePricingStrategyPreSave(DynamicTable dynamicTable)
        {
            var table = dynamicTable.BasicThreshold;
            var tableTag = dynamicTable.TableTag;
            return ValidationLogic(table, tableTag);
        }

        public async Task<PricingStrategyValidationResult> ValidatePricingStrategyPostSave(DynamicTableMeta dynamicTableMeta)
        {
            var tableId = dynamicTableMeta.TableId;
            var areaId = dynamicTableMeta.AreaIdentifier;
            var thresholds = await repository.GetAllRows(areaId, tableId);
            return ValidationLogic(thresholds, dynamicTableMeta.TableTag);
        }

        public async Task<List<TableRow>> CreatePreviewData(DynamicTableMeta tableMeta, Area area, CultureInfo culture)
        {
            var availableBasicThreshold = await responseRetriever.RetrieveAllAvailableResponses<BasicThreshold>(tableMeta.TableId);
            var responseParts = DynamicTableTypes.CreateBasicThreshold().CreateDynamicResponseParts(availableBasicThreshold.First().TableId, availableBasicThreshold.First().Threshold.ToString());
            var currentRows = await CompileToPdfTableRow(responseParts, new List<string>() { tableMeta.TableId }, culture);
            return currentRows;
        }

        public async Task<List<BasicThreshold>> RetrieveAllAvailableResponses(string dynamicResponseId)
        {
            return await GetAllRowsMatchingResponseId(dynamicResponseId);
        }
    }
}