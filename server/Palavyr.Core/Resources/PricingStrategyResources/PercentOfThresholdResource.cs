namespace Palavyr.Core.Resources.PricingStrategyResources
{
    public class PercentOfThresholdResource : PricingStrategyTableRowResource
    {
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public double Modifier { get; set; }
        public bool PosNeg { get; set; }
        public int RowOrder { get; set; }
        public bool TriggerFallback { get; set; }

        public int ItemOrder { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; } // unfortunate - doesn't fit in meta, and here it will be duplicated - we don't keep a table for the subtables held by this
    }
}