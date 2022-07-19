using Palavyr.Core.Mappers;

namespace Palavyr.Core.Resources
{
    public class WidgetNodeResource
    {
        public string IntentId { get; set; }
        public string NodeId { get; set; }
        public string Text { get; set; }
        public string NodeType { get; set; }
        public string NodeChildrenString { get; set; }
        public bool IsRoot { get; set; }
        public bool IsCritical { get; set; }
        public string OptionPath { get; set; }
        public string ValueOptions { get; set; }
        public string NodeComponentType { get; set; }
        public bool IsPricingStrategyTableNode { get; set; }
        public string PricingStrategyType { get; set; }
        public int ResolveOrder { get; set; }
        public int UnitId { get; set; }

        public FileAssetResource? FileAssetResource { get; set; }
        // public string? UnitGroup { get; set; }
        // public string? UnitPrettyName { get; set; }
        // public string? CurrencySymbol { get; set; }
    }
}