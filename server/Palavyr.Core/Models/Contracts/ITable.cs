namespace Palavyr.Core.Models.Contracts
{
    public interface ITable : IRecord
    {
        public string TableId { get; set; }
    }
}