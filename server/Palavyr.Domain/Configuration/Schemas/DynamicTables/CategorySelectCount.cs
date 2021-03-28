using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Palavyr.Domain.Contracts;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.Domain.Configuration.Schemas.DynamicTables
{
    public class CategorySelectCount : IOrderedTable, IDynamicTable<CategorySelectCount>, IHaveRange, IMultiItem
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
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public int ItemOrder { get; set; }
        public string Count { get; set; }

        public CategorySelectCount CreateNew(
            string accountId,
            string areaIdentifier,
            string tableId,
            double valueMin,
            double valueMax,
            bool range,
            string rowId,
            int rowOrder,
            string itemName,
            string itemId,
            int itemOrder
        )
        {
            return new CategorySelectCount
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                RowId = rowId,
                RowOrder = rowOrder,
                ItemName = itemName,
                ItemId = itemId,
                ItemOrder = itemOrder
            };
        }

        public CategorySelectCount CreateTemplate(string accountId, string areaIdentifier, string tableId)
        {
            return new CategorySelectCount
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
            };
        }

        public List<CategorySelectCount> UpdateTable(DynamicTable table)
        {
            var mappedTableRows = new List<CategorySelectCount>();
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
                        row.ItemName,
                        row.ItemId,
                        row.ItemOrder));
            }

            return mappedTableRows;
        }
    }
}