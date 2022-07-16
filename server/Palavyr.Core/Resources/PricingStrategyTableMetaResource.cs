using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Resources
{
    public class PricingStrategyTableMetaResource : IPricingStrategyTableRowResource
    {
        public int? Id { get; set; }
        public string TableTag { get; set; }
        public string TableType { get; set; }
        public string TableId { get; set; }
        public string IntentId { get; set; }
        public bool ValuesAsPaths { get; set; }
        public string PrettyName { get; set; }
        public string UnitPrettyName { get; set; }
        public string UnitGroup { get; set; }
        public UnitIds UnitId { get; set; }
        public string AccountId { get; set; }
    }
}