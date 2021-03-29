using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Palavyr.Domain.Contracts;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.Domain.Configuration.Schemas.DynamicTables
{
    public class TwoNestedCategory : IOrderedTable, IDynamicTable<TwoNestedCategory>, IHaveRange, IMultiItem
    {
        [Key] public int? Id { get; set; }
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
        public string Category { get; set; }
        public string SubCategory { get; set; }

        public TwoNestedCategory CreateNew(
            string accountId,
            string areaIdentifier,
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
            return new TwoNestedCategory
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                RowId = rowId,
                RowOrder = rowOrder,
                Category = category,
                SubCategory = subCategory,
                ItemId = itemId,
                ItemOrder = itemOrder
            };
        }

        public TwoNestedCategory CreateTemplate(string accountId, string areaIdentifier, string tableId)
        {
            return new TwoNestedCategory
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
            };
        }

        public List<TwoNestedCategory> UpdateTable(DynamicTable table)
        {
            var mappedTableRows = new List<TwoNestedCategory>();
            foreach (var row in table.CategorySelectCount)
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
                        row.Category,
                        row.SubCategory,
                        row.ItemId,
                        row.ItemOrder));
            }

            return mappedTableRows;
        }
    }
}