#nullable disable

using System.Collections.Generic;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Data.Entities.DynamicTables
{
    public class CategorySelectTableRow : Entity, IOrderedTable, IPricingStrategyTable<CategorySelectTableRow>, IHaveAccountId
    {
        private const string PrettyName = "Simple Select One Option";


        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string TableId { get; set; }
        public string Category { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public int RowOrder { get; set; }

        public static CategorySelectTableRow CreateNew(
            string accountId,
            string intentId,
            string category,
            double valueMin,
            double valueMax,
            bool range,
            string tableId,
            int rowOrder)
        {
            return new CategorySelectTableRow
            {
                AccountId = accountId,
                IntentId = intentId,
                TableId = tableId,
                Category = category,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                RowOrder = rowOrder
            };
        }

        public CategorySelectTableRow CreateTemplate(string accountId, string intentId, string tableId)
        {
            return new CategorySelectTableRow()
            {
                AccountId = accountId,
                IntentId = intentId,
                Category = "Option Placeholder",
                ValueMin = 0.00,
                ValueMax = 0.00,
                Range = false,
                TableId = tableId,
            };
        }

        public List<CategorySelectTableRow> UpdateTable(PricingStrategyTable<CategorySelectTableRow> table)
        {
            var mappedTableRows = new List<CategorySelectTableRow>();
            foreach (var row in table.TableData)
            {
                var mappedRow = CreateNew(
                    row.AccountId,
                    row.IntentId,
                    row.Category,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range,
                    row.TableId,
                    row.RowOrder
                );
                mappedTableRows.Add(mappedRow);
            }

            return mappedTableRows;
        }


        public bool EnsureValid()
        {
            return true; // TODO implement validation logic. This is called in the handler
        }

        public string GetPrettyName()
        {
            return PrettyName;
        }


        public string GetTableType()
        {
            return GetType().Name;
        }
    }
}