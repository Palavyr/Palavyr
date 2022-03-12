using System.Collections.Generic;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.Core.Models.Configuration.Schemas.DynamicTables
{
    public class SelectOneFlat : Entity, IOrderedTable, IDynamicTable<SelectOneFlat>, IHaveAccountId
    {
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string Option { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public int RowOrder { get; set; }

        public static SelectOneFlat CreateNew(string accountId, string areaIdentifier, string option, double valueMin, double valueMax, bool range, string tableId, int rowOrder)
        {
            return new SelectOneFlat()
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

        public SelectOneFlat CreateTemplate(string accountId, string areaIdentifier, string tableId)
        {
            return new SelectOneFlat()
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

        public List<SelectOneFlat> UpdateTable(DynamicTable table)
        {
            var mappedTableRows = new List<SelectOneFlat>();
            foreach (var row in table.SelectOneFlat)
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
    }
}