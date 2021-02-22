using System.Collections.Generic;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Domain.Configuration.Constant
{
    /// <summary>
    /// Node type option represents the object used to configure the tree in the dashboard
    ///  
    /// </summary>
    public class NodeTypeOption
    {
        // Groups
        public static readonly string MultipleChoice = "Multiple Choice";
        public static readonly string InfoCollection = "Info Collection";
        public static readonly string InfoProvide = "Provide Info";
        public static readonly string CustomTables = "Custom Tables";
        public static readonly string Terminal = "Terminal";

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
         * Whether or not the node type comes from a dynamic table type.
         */
        public bool IsDynamicType { get; set; }

        /*
         * The key by which we group nodes in the selector
         */
        public string GroupName { get; set; } = null!;

        public virtual string StringName => null!;

        public static NodeTypeOption Create(
            string value,
            string text,
            List<string> pathOptions,
            List<string> valueOptions,
            bool isDynamicType,
            bool isMultiOptionType,
            bool isTerminalType,
            string groupName
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
                GroupName = groupName
            };
        }

        public ConversationNode MapNodeTypeOptionToConversationNode(
            string nodeId,
            string text,
            bool isRoot,
            string nodeChildrenString,
            string nodeType,
            string accountId,
            string areaIdentifier,
            string optionPath
        )
        {
            return new ConversationNode()
            {
                NodeId = nodeId,
                Text = text,
                IsRoot = isRoot,
                NodeChildrenString = nodeChildrenString, //"node-456,node-789",
                NodeType = nodeType,
                OptionPath = optionPath,
                ValueOptions = string.Join(Delimiters.PathOptionDelimiter, this.ValueOptions),
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                IsMultiOptionType = this.IsMultiOptionType,
                IsTerminalType = this.IsTerminalType
            };
        }
    }
}