
using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Data.Entities.PricingStrategyTables
{
    public class CategoryNestedThresholdTableRow : Entity, IOrderedTable, IPricingStrategyTable<CategoryNestedThresholdTableRow>, IHaveRange, IMultiItem, IOrderableThreshold, IHaveAccountId
    {
        private const string PrettyName = "Category with Nested Threshold";

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
        public double Threshold { get; set; }
        public bool TriggerFallback { get; set; }

        public CategoryNestedThresholdTableRow CreateNew(
            string accountId,
            string intentId,
            string tableId,
            double valueMin,
            double valueMax,
            bool range,
            string rowId,
            int rowOrder,
            string category,
            double threshold,
            string itemId,
            int itemOrder,
            bool triggerFallback
        )
        {
            return new CategoryNestedThresholdTableRow
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
                Threshold = threshold,
                ItemId = itemId,
                ItemOrder = itemOrder,
                TriggerFallback = triggerFallback
            };
        }

        public CategoryNestedThresholdTableRow CreateTemplate(string accountId, string intentId, string tableId)
        {
            return new CategoryNestedThresholdTableRow
            {
                AccountId = accountId,
                IntentId = intentId,
                TableId = tableId,
                ItemId = StaticGuidUtils.CreateNewId(),
                Category = "Default Category Text",
                Threshold = 0.0,
                ValueMax = 0.0,
                ValueMin = 0.0,
                Range = false,
                RowId = StaticGuidUtils.CreateNewId(),
                RowOrder = 0,
                ItemOrder = 0
            };
        }

        public List<CategoryNestedThresholdTableRow> UpdateTable(PricingStrategyTable<CategoryNestedThresholdTableRow> table)
        {
            var mappedTableRows = new List<CategoryNestedThresholdTableRow>();
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
                        row.Threshold,
                        row.ItemId,
                        row.ItemOrder,
                        row.TriggerFallback));
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