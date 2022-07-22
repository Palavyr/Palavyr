namespace Palavyr.Core.Resources.PricingStrategyResources
{
    public class CategorySelectTableRowResource : PricingStrategyTableRowResource
    {
        public string IntentId { get; set; }
        public string Category { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public int RowOrder { get; set; }
    }
}