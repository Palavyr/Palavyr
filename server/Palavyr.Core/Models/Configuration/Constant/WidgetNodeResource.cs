namespace Palavyr.Core.Models.Configuration.Constant
{
    public class WidgetNodeResource
    {
        public string AreaIdentifier { get; set; }
        public string NodeId { get; set; }
        public string Text { get; set; }
        public string NodeType { get; set; }
        public string NodeChildrenString { get; set; }
        public bool IsRoot { get; set; }
        public bool IsCritical { get; set; }
        public string OptionPath { get; set; }
        public string ValueOptions { get; set; }
        public string NodeComponentType { get; set; }
        public bool IsDynamicTableNode { get; set; }
        public string DynamicType { get; set; }
    }
}