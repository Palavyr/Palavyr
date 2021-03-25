namespace Palavyr.Domain.Contracts
{
    public interface IStaticTable : IRecord
    {
        public int TableOrder { get; set; }
    }
}