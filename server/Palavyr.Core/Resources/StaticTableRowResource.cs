﻿namespace Palavyr.Core.Resources
{
    public class StaticTableRowResource : NullableEntityResource
    {
        
        public int RowOrder { get; set; }
        public string Description { get; set; } 
        public StaticFeeResource Fee { get; set; }
        public bool Range { get; set; }
        public bool PerPerson { get; set; }
        public int TableOrder { get; set; }
        public string IntentId { get; set; }
    }
}