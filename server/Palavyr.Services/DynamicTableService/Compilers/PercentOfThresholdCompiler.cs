using System;
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
    public class PercentOfThresholdCompiler : BaseCompiler<PercentOfThreshold>, IDynamicTablesCompiler
    {
        public PercentOfThresholdCompiler(IGenericDynamicTableRepository<PercentOfThreshold> repository) : base(repository)
        {
        }

        public Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier(),
                    dynamicTableMeta.ConvertToPrettyName(),
                    new List<string>() {"Continue"},
                    new List<string>() {"Continue"},
                    true,
                    false,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber // this is for the tree, so okay, but it should be what the dynamic table item type is. We don't have access to that here, so we just say its a number.
                ));
            return Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, DynamicResponse dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await Repository.GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);

            var itemIds = allRows.Select(item => item.ItemId).Distinct().ToArray();
            foreach (var itemId in itemIds)
            {
                var itemThresholds = allRows.Where(item => item.ItemId == itemId).ToList();
                itemThresholds.Sort();
                foreach (var threshold in itemThresholds)
                {
                    if (responseValueAsDouble >= threshold.Threshold)
                    {
                        var minBaseAmount = threshold.ValueMin;
                        var maxBaseAmount = threshold.ValueMax;

                        if (threshold.PosNeg)
                        {
                            minBaseAmount += minBaseAmount * (threshold.Modifier / 100);
                            maxBaseAmount += maxBaseAmount * (threshold.Modifier / 100);
                        }
                        else
                        {
                            minBaseAmount -= minBaseAmount * (threshold.Modifier / 100);
                            maxBaseAmount -= maxBaseAmount * (threshold.Modifier / 100);
                        }

                        return new List<TableRow>()
                        {
                            new TableRow(
                                threshold.ItemName,
                                minBaseAmount,
                                maxBaseAmount,
                                false,
                                culture,
                                threshold.Range
                            )
                        };
                    }
                }
            }

            throw new InvalidOperationException("Provided threshold value was too high. This is a configuration error.");
        }
    }
}