
using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Data.Entities.PricingStrategyTables
{
    public class TwoNestedSelectTableRow : Entity, IOrderedTable, IPricingStrategyTable<TwoNestedSelectTableRow>, IHaveRange, IMultiItem, IHaveAccountId
    {
        private const string PrettyName = "Two Nested Categories";

        
        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string TableId { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string RowId { get; set; }
        public int RowOrder { get; set; }
        public string ItemId { get; set; }
        public int ItemOrder { get; set; }
        public string Category { get; set; }
        public string InnerItemName { get; set; }

        public TwoNestedSelectTableRow CreateNew(
            string accountId,
            string intentId,
            string tableId,
            double valueMin,
            double valueMax,
            bool range,
            string rowId,
            int rowOrder,
            string category,
            string subCategory,
            string itemId,
            int itemOrder
        )
        {
            return new TwoNestedSelectTableRow
            {
                AccountId = accountId,
                IntentId = intentId,
                TableId = tableId,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                RowId = rowId,
                RowOrder = rowOrder,
                Category = category,
                InnerItemName = subCategory,
                ItemId = itemId,
                ItemOrder = itemOrder
            };
        }

        public TwoNestedSelectTableRow CreateTemplate(string accountId, string intentId, string tableId)
        {
            return new TwoNestedSelectTableRow
            {
                AccountId = accountId,
                IntentId = intentId,
                TableId = tableId,
                ItemId = StaticGuidUtils.CreateNewId(),
                Category = "Default Category Text",
                InnerItemName = "",
                ValueMax = 0.0,
                ValueMin = 0.0,
                Range = false,
                RowId = StaticGuidUtils.CreateNewId(),
                RowOrder = 0,
                ItemOrder = 0
            };
        }

        public List<TwoNestedSelectTableRow> UpdateTable(PricingStrategyTable<TwoNestedSelectTableRow> table)
        {
            var mappedTableRows = new List<TwoNestedSelectTableRow>();
            foreach (var row in table.TableData)
            {
                mappedTableRows.Add(
                    CreateNew(
                        row.AccountId,
                        row.IntentId,
                        row.TableId,
                        row.ValueMin,
                        row.ValueMax,
                        row.Range,
                        row.RowId,
                        row.RowOrder,
                        row.Category,
                        row.InnerItemName,
                        row.ItemId,
                        row.ItemOrder));
            }

            return mappedTableRows;
        }

        public bool EnsureValid()
        {
            return true;
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