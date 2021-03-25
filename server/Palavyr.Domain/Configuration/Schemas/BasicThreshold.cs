using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Palavyr.Common.UIDUtils;
using Palavyr.Domain.Contracts;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class BasicThreshold : IComparable<BasicThreshold>, IOrderedTable, IDynamicTable<BasicThreshold>
    {
        [Key] public int? Id { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public string ItemName { get; set; }
        public int RowOrder { get; set; }
        public bool Range { get; set; }

        public BasicThreshold() { }

        public BasicThreshold CreateNew(string accountId, string areaId, string tableId, string rowId, double threshold, double valueMin, double valueMax, bool range)
        {
            return new BasicThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaId,
                TableId = tableId,
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
            foreach (var row in table.BasicThreshold)
            {
                var mappedRow = CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.TableId,
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

        public int CompareTo(BasicThreshold other)
        {
            return other.Threshold.CompareTo(Threshold);
        }
    }
}