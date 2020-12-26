using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Configuration.Schemas
{
    public class PercentOfThreshold
    {
        [Key] public int? Id { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
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
                PosNeg = true
            };
        }
    }
}