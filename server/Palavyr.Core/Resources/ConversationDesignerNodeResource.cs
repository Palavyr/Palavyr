using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Resources
{
    public class ConversationDesignerNodeResource
    {
        public ConversationDesignerNodeResource()
        {
        }

        public string? IntentId { get; set; }
        public string? AccountId { get; set; }
        public string? NodeId { get; set; }
        public string? Text { get; set; }
        public bool IsRoot { get; set; }
        public bool IsCritical { get; set; }
        public bool IsMultiOptionType { get; set; }
        public bool IsTerminalType { get; set; }
        public bool ShouldRenderChildren { get; set; }

        public bool IsLoopbackAnchorType { get; set; }

        public bool IsAnabranchType { get; set; }
        public bool IsAnabranchMergePoint { get; set; }

        public bool ShouldShowMultiOption { get; set; }
        public bool IsDynamicTableNode { get; set; }
        public bool IsMultiOptionEditable { get; set; }
        public bool IsImageNode { get; set; }
        public string? ImageId { get; set; } // no extension on this (don't add .png)
        // public string? FileId { get; set; } // TODO: Migration to add this and move away from imageId
        public string? OptionPath { get; set; }
        public string? ValueOptions { get; set; }
        public string? NodeType { get; set; }
        public string? DynamicType { get; set; }
        public string? NodeComponentType { get; set; }
        public int? ResolveOrder { get; set; }
        public bool IsCurrency { get; set; }
        public string? NodeChildrenString { get; set; } = ""; // stored as comma delimited list as string
        public NodeTypeCode NodeTypeCode { get; set; }
    }
}