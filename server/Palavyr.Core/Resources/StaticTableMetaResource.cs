using System.Collections.Generic;

namespace Palavyr.Core.Resources
{
    public class StaticTableMetaResource : NullableEntityResource
    {
        public int TableOrder { get; set; }
        public string Description { get; set; }
        public string IntentId { get; set; }
        public IEnumerable<StaticTableRowResource> StaticTableRowResources { get; set; }
        public bool PerPersonInputRequired { get; set; }
        public bool IncludeTotals { get; set; }
        public string TableId { get; set; }
    }
}