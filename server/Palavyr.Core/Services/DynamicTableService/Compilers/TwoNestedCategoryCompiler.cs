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
    public class TwoNestedCategoryCompiler : BaseCompiler<TwoNestedCategory>, IDynamicTablesCompiler
    {
        private readonly IGenericDynamicTableRepository<TwoNestedCategory> repository;
        private readonly IConfigurationRepository configurationRepository;
        private readonly IConversationOptionSplitter splitter;

        public TwoNestedCategoryCompiler(
            IGenericDynamicTableRepository<TwoNestedCategory> repository,
            IConfigurationRepository configurationRepository,
            IConversationOptionSplitter splitter
        ) : base(repository)
        {
            this.repository = repository;
            this.configurationRepository = configurationRepository;
            this.splitter = splitter;
        }

        public async Task<List<TableRow>> CompileToPdfTableRow(string accountId, List<Dictionary<string, string>> dynamicResponse, List<string> dynamicResponseIds, CultureInfo culture)
        {
            var responseId = GetSingleResponseId(dynamicResponseIds);
            var records = await repository.GetAllRowsMatchingDynamicResponseId(accountId, responseId);

            // itemName then Count
            var orderedResponseIds = await GetResponsesOrderedByResolveOrder(dynamicResponse);
            var outerCategory = GetResponseByResponseId(orderedResponseIds[0], dynamicResponse);
            var innerCategory = GetResponseByResponseId(orderedResponseIds[1], dynamicResponse);

            var result = records.Single(rec => rec.Category == outerCategory && rec.SubCategory == innerCategory);
            var dynamicTableMeta = await configurationRepository.GetDynamicTableMetaByTableId(result.TableId);

            return new List<TableRow>()
            {
                new TableRow(
                    dynamicTableMeta.UseTableTagAsResponseDescription ? dynamicTableMeta.TableTag : string.Join(" & ", new[] {result.Category, result.SubCategory}),
                    result.ValueMin,
                    result.ValueMax,
                    false,
                    culture,
                    result.Range
                )
            };
        }

        private CategoryRetriever GetInnerAndOuterCategories(List<TwoNestedCategory> rawRows)
        {
            // This table type does not facilitate multiple branches. I.e. the inner categories are all the same for all of the outer categories.
            var rows = rawRows.OrderBy(row => row.RowOrder).ToList();

            var outerCategories = rows.Select(row => row.Category).Distinct().ToList();

            var itemId = rows.Select(row => row.ItemId).Distinct().First();
            var innerCategories = rows.Where(row => row.ItemId == itemId).Select(row => row.SubCategory).ToList();

            return new CategoryRetriever
            {
                InnerCategories = innerCategories,
                OuterCategories = outerCategories
            };
        }

        public void UpdateConversationNode(DashContext context, DynamicTable table, string tableId)
        {
            var update = table.TwoNestedCategory;

            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(update);
            var nodes = context
                .ConversationNodes
                .Where(x => splitter.GetTableIdFromDynamicNodeType(x.NodeType) == tableId)
                .OrderBy(x => x.ResolveOrder);
            nodes.First().ValueOptions = splitter.JoinValueOptions(outerCategories);
            nodes.Last().ValueOptions = splitter.JoinValueOptions(innerCategories);
        }

        public async Task CompileToConfigurationNodes(DynamicTableMeta dynamicTableMeta, List<NodeTypeOption> nodes)
        {
            var rawRows = await GetTableRows(dynamicTableMeta);
            var (innerCategories, outerCategories) = GetInnerAndOuterCategories(rawRows);
            var widgetResponseKey = dynamicTableMeta.MakeUniqueIdentifier();

            // Outer-category
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Outer-Categories"),
                    dynamicTableMeta.ConvertToPrettyName("Outer"),
                    new List<string>() {"Continue"},
                    outerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 0,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey
                ));

            // inner-categories
            nodes.AddAdditionalNode(
                NodeTypeOption.Create(
                    dynamicTableMeta.MakeUniqueIdentifier("Inner-Categories"),
                    dynamicTableMeta.ConvertToPrettyName("Inner"),
                    new List<string>() {"Continue"},
                    innerCategories,
                    true,
                    true,
                    false,
                    NodeTypeOption.CustomTables,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue,
                    resolveOrder: 1,
                    isMultiOptionEditable: false,
                    dynamicType: widgetResponseKey
                ));
        }

        public class CategoryRetriever
        {
            public List<string> InnerCategories { get; set; }
            public List<string> OuterCategories { get; set; }

            public void Deconstruct(out List<string> innerCategories, out List<string> outerCategories)
            {
                innerCategories = InnerCategories;
                outerCategories = OuterCategories;
            }
        }
    }
}