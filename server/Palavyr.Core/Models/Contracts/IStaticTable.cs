namespace Palavyr.Core.Models.Contracts
{
    public interface IStaticTable : IRecord
    {
        public int TableOrder { get; set; }
    }
}