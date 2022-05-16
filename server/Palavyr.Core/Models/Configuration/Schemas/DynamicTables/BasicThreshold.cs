using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Requests;

namespace Palavyr.Core.Models.Configuration.Schemas.DynamicTables
{
    public class BasicThreshold : Entity, IOrderedTable, IDynamicTable<BasicThreshold>, IHaveRange, IOrderableThreshold, IHaveAccountId
    {
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
        public bool TriggerFallback { get; set; }

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
            bool range,
            bool triggerFallback)
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
                Range = range,
                TriggerFallback = triggerFallback
            };
        }

        public BasicThreshold CreateTemplate(string accountId, string areaId, string tableId)
        {
            return new BasicThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaId,
                TableId = tableId,
                RowId = StaticGuidUtils.CreateNewId()
            };
        }

        public List<BasicThreshold> UpdateTable(DynamicTable<BasicThreshold> table)
        {
            var mappedTableRows = new List<BasicThreshold>();
            foreach (var row in table.TableData)
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
                    row.Range,
                    row.TriggerFallback
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