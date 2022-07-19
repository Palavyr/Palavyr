using System.Collections.Generic;

namespace Palavyr.Core.Resources
{
    public class StaticTableMetaResource : IEntityResource
    {
        public int TableOrder { get; set; }
        public string Description { get; set; }
        public string IntentId { get; set; }
        public IEnumerable<StaticTableRowResource> StaticTableRowResources { get; set; }
        public string AccountId { get; set; }
        public bool PerPersonInputRequired { get; set; }
        public bool IncludeTotals { get; set; }
        public int Id { get; set; }
    }
}