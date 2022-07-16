#nullable disable

using System.Collections.Generic;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Data.Entities.DynamicTables
{
    public class SimpleSelectTableRow : Entity, IOrderedTable, IPricingStrategyTable<SimpleSelectTableRow>, IHaveAccountId
    {
        private const string PrettyName = "Simple Select One Option";


        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string Option { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public int RowOrder { get; set; }

        public static SimpleSelectTableRow CreateNew(
            string accountId,
            string areaIdentifier,
            string option,
            double valueMin,
            double valueMax,
            bool range,
            string tableId,
            int rowOrder)
        {
            return new SimpleSelectTableRow
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                Option = option,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                RowOrder = rowOrder
            };
        }

        public SimpleSelectTableRow CreateTemplate(string accountId, string areaIdentifier, string tableId)
        {
            return new SimpleSelectTableRow()
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                Option = "Option Placeholder",
                ValueMin = 0.00,
                ValueMax = 0.00,
                Range = false,
                TableId = tableId,
            };
        }

        public List<SimpleSelectTableRow> UpdateTable(PricingStrategyTable<SimpleSelectTableRow> table)
        {
            var mappedTableRows = new List<SimpleSelectTableRow>();
            foreach (var row in table.TableData)
            {
                var mappedRow = CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.Option,
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