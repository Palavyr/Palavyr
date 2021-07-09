#nullable enable
using System.Collections.Generic;

namespace Palavyr.Core.Models.Configuration.Constant
{
    /// <summary>
    /// Node type option represents the object used to configure the tree in the dashboard
    ///  
    /// </summary>
    public class NodeTypeOption
    {
        // Groups
        public static readonly string MultipleChoice = "Multiple Choice";
        public static readonly string Teleport = "Teleport";
        public static readonly string InfoCollection = "Info Collection";
        public static readonly string InfoProvide = "Provide Info";
        public static readonly string CustomTables = "Custom Tables";
        public static readonly string Terminal = "Terminal";
        public static readonly string SplitAndMerge = "Split then Merge";
        public static readonly string Other = "Other";

        /*
         * The string form name of the node type. Derived from either 'nameof(T)' or extension method: dynamicTableMeta.MakeUniqueIdentifier()
         */
        public NodeTypeCode NodeTypeCode { get; set; }

        /*
         * The string form name of the node type. Derived from either 'nameof(T)' or extension method: dynamicTableMeta.MakeUniqueIdentifier()
         */
        public string Value { get; set; }

        /*
         * The text presented in the node tree dropdown menu.
         */
        public string Text { get; set; }

        /*
         * The options used to determine how many child nodes this node will have (defines the paths from this node)
         */
        public List<string> PathOptions { get; set; }

        /*
         * The physical options that a user can select on this node. May or may not correlate with pathOptions
         */
        public List<string> ValueOptions { get; set; }

        /*
         * Whether or not this NodeOptionType produces multiple paths
         */
        public bool IsMultiOptionType { get; set; }

        /*
         * Whether or not this type should be used to determine incomplete tree paths when searching for missing node types.
         */
        public bool IsTerminalType { get; set; }


        /*
         * Whether or not this type should render its children (used with splitmerge - where children will be duplicated'
         */
        public bool ShouldRenderChildren { get; set; }

        /*
         * Whether or not this type should show the multioption selector
         */
        public bool ShouldShowMultiOption { get; set; }

        /*
         * Whether or not its children will result in a remerge of the branch after splitting into N children (all children must remerge)
         */
        public bool IsSplitMergeType { get; set; } = false;

        /*
         * Whether or not the node is an Anabranch type (will split, and then all leaves will either terminate or merge into a single node.
         */
        public bool IsAnabranchType { get; set; }

        /*
         * Whether or not the node is an Anabranch merge node. This is mutually exclusive with the Anabranch Type. (Anabranch node types cannot be Anabranch merge points).
         */
        public bool IsAnabranchMergePoint { get; set; }

        /*
         * Whether or not the node type comes from a dynamic table type.
         */
        public bool IsDynamicType { get; set; }

        /*
         * The string identifier of the node component Type
         */
        public string NodeComponentType { get; set; }

        /*
         * Whether or not the response value is currency
         */
        public bool IsCurrency { get; set; }

        /*
         * Path options are editable
         */
        public bool IsMultiOptionEditable { get; set; }

        /*
         * The key by which we group nodes in the selector
         */
        public string GroupName { get; set; } = null!;

        /*
         * Used when the nodeTypeOption is for a dynamic type. The order in which the result should be used to filter the dynamic table configuration.
         * DO NOT SET on the default node types. Only used for dynamic node types.
         */
        public int? ResolveOrder { get; set; }

        /*
         * Used when the nodetype option is a dynamic type and we need to specify a common type for the dynamic type compiler
         * The widget will use this to key the collection of dynamic type responses.
         */
        public string? DynamicType { get; set; }

        /*
         * Used to indicate whether or not this node provides an image in the chat. In the dashboard, used to determine whether
         * or not to show the image upload component.
         */
        public bool IsImageNode { get; set; }

        /*
         * Used to indicate if this nodeOption is a loopback anchor.
         */
        public bool IsLoopbackAnchor { get; set; }

        public virtual string StringName => null!;

        public static NodeTypeOption Create(
            string value,
            string text,
            List<string> pathOptions,
            List<string> valueOptions,
            bool isDynamicType,
            bool isMultiOptionType,
            bool isTerminalType,
            string groupName,
            string nodeComponentType,
            NodeTypeCode nodeTypeCode,
            bool isMultiOptionEditable = true,
            bool isCurrency = false,
            bool isAnabranchType = false,
            bool isAnabranchMergeType = false,
            bool isSplitMergeType = false,
            bool shouldRenderChildren = true,
            bool shouldShowMultiOption = false,
            int? resolveOrder = null,
            string? dynamicType = null,
            bool loopbackAnchor = false
        )
        {
            return new NodeTypeOption()
            {
                Value = value,
                Text = text,
                PathOptions = pathOptions,
                ValueOptions = valueOptions,
                IsMultiOptionType = isMultiOptionType,
                IsTerminalType = isTerminalType,
                IsDynamicType = isDynamicType,
                GroupName = groupName,
                NodeComponentType = nodeComponentType,
                IsCurrency = isCurrency,
                IsMultiOptionEditable = isMultiOptionEditable,
                IsAnabranchType = isAnabranchType,
                IsAnabranchMergePoint = isAnabranchMergeType,
                IsSplitMergeType = isSplitMergeType,
                ShouldRenderChildren = shouldRenderChildren,
                ShouldShowMultiOption = shouldShowMultiOption,
                ResolveOrder = resolveOrder,
                DynamicType = dynamicType,
                IsLoopbackAnchor = loopbackAnchor,
                NodeTypeCode = nodeTypeCode
            };
        }
    }
}