namespace Palavyr.Core.Models.Configuration.Schemas.DynamicTables
{
    public class TwoNestedCategoryResource : IPricingStrategyTableRowResource
    {
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
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