namespace Palavyr.Domain.Configuration.Schemas.DynamicTables
{
    public interface IMultiItem
    {
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public int ItemOrder { get; set; }
    }
}