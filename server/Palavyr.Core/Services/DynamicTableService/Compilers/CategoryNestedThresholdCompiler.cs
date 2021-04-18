using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

namespace Palavyr.Core.Services.DynamicTableService.Compilers
{
    public class CategoryNestedThresholdCompiler : BaseCompiler<CategoryNestedThreshold>, IDynamicTablesCompiler
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConversationOptionSplitter splitter;

        public CategoryNestedThresholdCompiler(
            IGenericDynamicTableRepository<CategoryNestedThreshold> repository,
            IConfigurationRepository configurationRepository,
            IConversationOptionSplitter splitter
        ) : base(repository)
        {
            this.configurationRepository = configurationRepository;
            this.splitter = splitter;
        }

        public List<string> GetCategories(List<CategoryNestedThreshold> rawRows)
        {
            var rows = rawRows.OrderBy(row => row.RowOrder).ToList();
            var categories = rows.Select(row => row.Category).Distinct().ToList();
            return categories;
        }
        public async Task UpdateConversationNode(DashContext context, DynamicTable table, string tableId)
        {
            var update = table.CategoryNestedThreshold;
            var category = GetCategories(update);

            var nodes = (await context.ConversationNodes.ToListAsync())
                .Where(x => x.IsDynamicTableNode && splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId)
                .OrderBy(x => x.ResolveOrder).ToList();
            if (nodes.Count > 0)
            {
                nodes.Single(x => x.ResolveOrder == 0).ValueOptions = splitter.JoinValueOptions(category);
            }
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var rawRows = await GetTableRows(dynamicTableMeta);
            var categories = GetCategories(rawRows);
            var widgetResponseKey = dynamicTableMeta.MakeUniqueIdentifier();

            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Category"),
                    dynamicTableMeta.ConvertToPrettyName(),
                    new List<string>() {"Continue"},
                    categories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 0,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey
                )
            );

            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Threshold"),
                    dynamicTableMeta.ConvertToPrettyName(),
                    new List<string>() {"Continue"},
                    new List<string>() {"Continue"},
                    true,
                    false,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.TakeNumber,
                    resolveOrder: 1,
                    isMultiOptionEditable: false,
                    dynamicType: dynamicTableMeta.MakeUniqueIdentifier()
                )
            );
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, List<Dictionary<string, string>> dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(dynamicResponseIds);
            var records = await Repository.GetAllRowsMatchingDynamicResponseId(accountId, responseId);

            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(dynamicResponse);
            var category = GetResponseByResponseId(orderedResponseIds[0], dynamicResponse);
            var amount = GetResponseByResponseId(orderedResponseIds[1], dynamicResponse);

            var categorySubset = records.Where(rec => rec.Category == category).OrderBy(x => x.Threshold);

            var responseAmountAsDouble = double.Parse(amount);

            var dynamicMeta = await configurationRepository.GetDynamicTableMetaByTableId(records.First().TableId);
            
            var tableRows = new List<TableRow>();
            foreach (var threshold in categorySubset)
            {
                if (responseAmountAsDouble >= threshold.Threshold)
                {
                    var minBaseAmount = threshold.ValueMin;
                    var maxBaseAmount = threshold.ValueMax;
                    
                    tableRows.Add(
                        new TableRow(
                            dynamicMeta.UseTableTagAsResponseDescription ? dynamicMeta.TableTag : string.Join(" @ ", new[] {category, responseAmountAsDouble.ToString("C", culture)}),
                            minBaseAmount,
                            maxBaseAmount, 
                            false,
                            culture,
                            threshold.Range
                            ));
                    break;
                }
            }

            return tableRows;
        }
    }
}