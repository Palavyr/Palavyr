namespace Palavyr.Domain.Contracts
{
    public interface ITable : IRecord
    {
        public string TableId { get; set; }
    }
}