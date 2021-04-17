using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public static class NodeMapperExtensionMethods
    {
        public static List<WidgetNodeResource> MapConversationToWidgetNodes(this List<ConversationNode> nodes)
        {
            return nodes.Select(x => MapConversationNodeToWidgetNodeResource(x)).ToList();
        }

        public static WidgetNodeResource MapConversationNodeToWidgetNodeResource(this ConversationNode node)
        {
            return new WidgetNodeResource
            {
                AreaIdentifier = node.AreaIdentifier,
                Text = node.Text,
                NodeType = node.NodeType,
                NodeId = node.NodeId,
                NodeChildrenString = node.NodeChildrenString,
                IsRoot = node.IsRoot,
                IsCritical = node.IsCritical,
                OptionPath = node.OptionPath,
                ValueOptions = node.ValueOptions,
                NodeComponentType = node.NodeComponentType,
                IsDynamicTableNode = node.IsDynamicTableNode,
                DynamicType = node.DynamicType
            };
        }

        public static ConversationNode MapNodeTypeOptionToConversationNode(
            this NodeTypeOption nodeTypeOption,
            string nodeId,
            string text,
            bool isRoot,
            string nodeChildrenString,
            string nodeType,
            string accountId,
            string areaIdentifier,
            string optionPath,
            bool isDynamic,
            string? nodeComponent = null,
            int? resolveOrder = null,
            string? dynamicType = null
        )
        {
            if (nodeComponent == null && nodeTypeOption.NodeComponent == null)
            {
                throw new Exception("NodeComponent must be set for dynamic table node types"); // TODO: can I enforce this via the compiler?
            }

            if (isDynamic && (resolveOrder == null || dynamicType == null))
            {
                throw new Exception("Dynamic node types MUST provide a resolve order and dynamic type name.");
            }

            return new ConversationNode()
            {
                NodeId = nodeId,
                Text = text,
                IsRoot = isRoot,
                NodeChildrenString = nodeChildrenString, //"node-456,node-789",
                NodeType = nodeType,
                OptionPath = optionPath,
                ValueOptions = string.Join(Delimiters.ValueOptionDelimiter, nodeTypeOption.ValueOptions),
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                IsMultiOptionType = nodeTypeOption.IsMultiOptionType,
                IsTerminalType = nodeTypeOption.IsTerminalType,
                IsDynamicTableNode = isDynamic,
                NodeComponentType = nodeComponent ?? nodeTypeOption.NodeComponent,
                ResolveOrder = resolveOrder,
                DynamicType = dynamicType
            };
        }
    }
}