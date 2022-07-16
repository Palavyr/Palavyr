namespace Palavyr.Core.Resources.PricingStrategyResources
{
    public class BasicThresholdResource : PricingStrategyTableRowResource
    {
        public string AccountId { get; set; }
        public string IntentId { get; set; }
        public string RowId { get; set; }
        public double Threshold { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string ItemName { get; set; }
        public int RowOrder { get; set; }
        public bool TriggerFallback { get; set; }

    }
}