namespace Palavyr.Core.Models.Contracts
{
    public interface IMultiItem
    {
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public int ItemOrder { get; set; }
    }
}