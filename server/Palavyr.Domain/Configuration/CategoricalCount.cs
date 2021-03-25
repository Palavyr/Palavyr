using Palavyr.Domain.Contracts;

namespace Palavyr.Domain.Configuration
{
    public class CategoricalCount : ITable
    {

        public int? Id { get; set; }
        public string AccountId { get; set; }
        public string AreaIdentifier { get; set; }
        public string TableId { get; set; }
    }
}