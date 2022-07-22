﻿namespace Palavyr.Core.Resources.PricingStrategyResources
{
    public class SelectWithNestedSelectResource : PricingStrategyTableRowResource
    {
        public string IntentId { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public string RowId { get; set; }
        public int RowOrder { get; set; }
        public string ItemId { get; set; }
        public int ItemOrder { get; set; }
        public string ItemName { get; set; }
        public string InnerItemName { get; set; }
    }
}