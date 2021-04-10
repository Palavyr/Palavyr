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
    public class BasicThresholdCompiler : BaseCompiler<BasicThreshold>, IDynamicTablesCompiler
    {
        private readonly IGenericDynamicTableRepository<BasicThreshold> repository;
        private readonly IConfigurationRepository configurationRepository;

        public BasicThresholdCompiler(IGenericDynamicTableRepository<BasicThreshold> repository, IConfigurationRepository configurationRepository) : base(repository)
        {
            this.repository = repository;
            this.configurationRepository = configurationRepository;
        }

        public void UpdateConversationNode(DashContext dashContext, DynamicTable table, string tableId)
        {
        }

        public Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var nodeTypeOption = NodeTypeOption.Create(
                dynamicTableMeta.MakeUniqueIdentifier(),
                dynamicTableMeta.ConvertToPrettyName(),
                new List<string>() {"Continue"},
                new List<string>() {"Continue"},
                true,
                false,
                false,
                NodeTypeOption.CustomTables,
                DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber,
                dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
            );
            nodes.AddAdditionalNode(nodeTypeOption);
            return Task.CompletedTask;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, List<Dictionary<string, string>> dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var dynamicResponseId = GetSingleResponseId(dynamicResponseIds);
            var responseValue = GetSingleResponseValue(dynamicResponse, dynamicResponseIds);

            var responseValueAsDouble = double.Parse(responseValue);
            var allRows = await repository.GetAllRowsMatchingDynamicResponseId(accountId, dynamicResponseId);

            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(allRows.First().TableId);

            var itemsToCreateRowsFor = allRows.Select(row => row.ItemName).Distinct();

            var tableRows = new List<TableRow>();
            foreach (var itemName in itemsToCreateRowsFor)
            {
                
                // should be ordered high to low
                var itemRows = allRows.Where(row => row.ItemName == itemName).OrderBy(row => row.Threshold).Reverse();

                foreach (var threshold in itemRows)
                {
                    if (responseValueAsDouble >= threshold.Threshold)
                    {
                        var minBaseAmount = threshold.ValueMin;
                        var maxBaseAmount = threshold.ValueMax;

                        tableRows.Add(
                            new TableRow(
                                dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : threshold.ItemName,
                                minBaseAmount,
                                maxBaseAmount,
                                false,
                                culture,
                                threshold.Range
                            )
                        );
                    }
                }
            }

            return tableRows;
        }
    }
}