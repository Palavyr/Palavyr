using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Models.Configuration.Schemas.DynamicTables
{
    public class CategoryNestedThresholdResource : IPricingStrategyTableRowResource
    {
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string RowId { get; set; }
        public int RowOrder { get; set; }
        public string ItemId { get; set; }
        public int ItemOrder { get; set; }
        public string ItemName { get; set; }
        public double Threshold { get; set; }
        public bool TriggerFallback { get; set; }
    }

    public class CategoryNestedThreshold : Entity, IOrderedTable, IDynamicTable<CategoryNestedThreshold>, IHaveRange, IMultiItem, IOrderableThreshold, IHaveAccountId
    {
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string RowId { get; set; }
        public int RowOrder { get; set; }
        public string ItemId { get; set; }
        public int ItemOrder { get; set; }
        public string ItemName { get; set; }
        public double Threshold { get; set; }
        public bool TriggerFallback { get; set; }

        public CategoryNestedThreshold CreateNew(
            string accountId,
            string areaIdentifier,
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
            return new CategoryNestedThreshold
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                RowId = rowId,
                RowOrder = rowOrder,
                ItemName = category,
                Threshold = threshold,
                ItemId = itemId,
                ItemOrder = itemOrder,
                TriggerFallback = triggerFallback
            };
        }

        public CategoryNestedThreshold CreateTemplate(string accountId, string areaIdentifier, string tableId)
        {
            return new CategoryNestedThreshold
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                ItemId = StaticGuidUtils.CreateNewId(),
                ItemName = "Default Category Text",
                Threshold = 0.0,
                ValueMax = 0.0,
                ValueMin = 0.0,
                Range = false,
                RowId = StaticGuidUtils.CreateNewId(),
                RowOrder = 0,
                ItemOrder = 0
            };
        }

        public List<CategoryNestedThreshold> UpdateTable(DynamicTable table)
        {
            var mappedTableRows = new List<CategoryNestedThreshold>();
            foreach (var row in table.CategoryNestedThreshold)
            {
                mappedTableRows.Add(
                    CreateNew(
                        row.AccountId,
                        row.AreaIdentifier,
                        row.TableId,
                        row.ValueMin,
                        row.ValueMax,
                        row.Range,
                        row.RowId,
                        row.RowOrder,
                        row.ItemName,
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
    }
}