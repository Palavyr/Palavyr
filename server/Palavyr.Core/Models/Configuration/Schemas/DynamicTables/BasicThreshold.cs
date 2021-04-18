using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.Core.Models.Configuration.Schemas.DynamicTables
{
    public class BasicThreshold : IOrderedTable, IDynamicTable<BasicThreshold>, IHaveRange
    {
        [Key]
        public int? Id { get; set; }

        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string ItemName { get; set; }
        public int RowOrder { get; set; }

        public BasicThreshold()
        {
        }

        public BasicThreshold CreateNew(
            string accountId,
            string areaId,
            string tableId,
            string itemName,
            string rowId,
            double threshold,
            double valueMin,
            double valueMax,
            bool range)
        {
            return new BasicThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaId,
                TableId = tableId,
                ItemName = itemName,
                RowId = rowId,
                Threshold = threshold,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range
            };
        }

        public BasicThreshold CreateTemplate(string accountId, string areaId, string tableId)
        {
            return new BasicThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaId,
                TableId = tableId,
                RowId = GuidUtils.CreateNewId()
            };
        }

        public List<BasicThreshold> UpdateTable(DynamicTable table)
        {
            var mappedTableRows = new List<BasicThreshold>();
            foreach (var row in table.BasicThreshold!)
            {
                var mappedRow = CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.TableId,
                    row.ItemName,
                    row.RowId,
                    row.Threshold,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range
                );
                mappedTableRows.Add(mappedRow);
            }

            return mappedTableRows;
        }

        public bool EnsureValid()
        {
            return true;
        }
    }
}