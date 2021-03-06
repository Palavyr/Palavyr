using System;
using System.ComponentModel.DataAnnotations;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class BasicThreshold : IComparable<BasicThreshold>
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

        public BasicThreshold()
        {
        }

        public static BasicThreshold CreateNew(string accountId, string areaId, string tableId, string rowId, double threshold, double valueMin, double valueMax, bool range)
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

        public static BasicThreshold CreateTemplate(string accountId, string areaId, string tableId, string rowId)
        {
            return new BasicThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaId,
                TableId = tableId,
                RowId = rowId
            };
        }

        public int CompareTo(BasicThreshold other)
        {
            return other.Threshold.CompareTo(Threshold);
        }
    }
}