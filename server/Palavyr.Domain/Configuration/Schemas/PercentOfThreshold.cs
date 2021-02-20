using System;
using System.ComponentModel.DataAnnotations;

namespace Palavyr.Domain.Configuration.Schemas
{
    /// <summary>
    /// The table meta provides a tableID, for this dynamic table though, we allow the records to be split into subtables
    /// so that we can associate multiple different items with the same requested value.
    /// So 1 value for a house might provide 3 different items, each with a different threshold by which pricing is determined.
    /// The ItemId/ItemName represents this partition key.
    /// The itemName unfortunately has to be duplicated along with the itemId.
    /// </summary>
    public class PercentOfThreshold : IComparable<PercentOfThreshold>
    {
        [Key] public int? Id { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; } // unfortunate - doesn't fit in meta, and here it will be duplicated - we don't keep a table for the subtables held by this
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public double Modifier { get; set; }
        public bool PosNeg { get; set; }

        public static PercentOfThreshold CreateNew(
            string accountId,
            string areaIdentifier,
            string tableId,
            string rowId,
            double threshold,
            double modifier,
            string itemName,
            string itemId,
            double valueMin,
            double valueMax,
            bool range,
            bool posNeg
        )
        {
            return new PercentOfThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                RowId = rowId,
                Threshold = threshold,
                Modifier = modifier,
                ItemName = itemName,
                ItemId = itemId,
                ValueMin = valueMin,
                ValueMax = valueMax,
                Range = range,
                PosNeg = posNeg
            };
        }

        public static PercentOfThreshold CreateTemplate(
            string accountId,
            string areaIdentifier,
            string tableId,
            string rowId,
            string itemId)
        {
            return new PercentOfThreshold()
            {
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                TableId = tableId,
                RowId = rowId,
                Threshold = 0.00,
                Modifier = 0.00,
                ItemName = "Default Item",
                ItemId = itemId,
                ValueMin = 0.00,
                ValueMax = 0.00,
                Range = false,
                PosNeg = true
            };
        }
        
        public int CompareTo(PercentOfThreshold obj)  
        {  
            return obj.Threshold.CompareTo(Threshold);  
  
        }  
    }
}