﻿namespace Palavyr.Core.Resources.PricingStrategyResources
{
    public class SelectOneFlatResource : PricingStrategyTableRowResource
    {
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string Option { get; set; }
        public double ValueMin { get; set; }
        public double ValueMax { get; set; }
        public bool Range { get; set; }
        public int RowOrder { get; set; }
    }
}