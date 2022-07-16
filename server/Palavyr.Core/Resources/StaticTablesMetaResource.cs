using System.Collections.Generic;

namespace Palavyr.Core.Resources
{
    public class StaticTablesMetaResource
    {
        public int TableOrder { get; set; }
        public string Description { get; set; }
        public string IntentId { get; set; }
        public IEnumerable<StaticTableRowResource> StaticTableRows { get; set; }
        public string AccountId { get; set; }
        public bool PerPersonInputRequired { get; set; }
        public bool IncludeTotals { get; set; }
    }
}