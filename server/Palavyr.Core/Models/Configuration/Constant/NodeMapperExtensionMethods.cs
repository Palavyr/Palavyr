
using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;

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
                NodeComponentType = node.NodeComponentType,
                NodeId = node.NodeId,
                NodeChildrenString = node.NodeChildrenString,
                IsRoot = node.IsRoot,
                IsCritical = node.IsCritical,
                OptionPath = node.OptionPath,
                ValueOptions = node.ValueOptions,
                IsDynamicTableNode = node.IsDynamicTableNode,
                DynamicType = node.DynamicType,
                ResolveOrder = node.ResolveOrder
            };
        }

        public static ConversationNode MapNodeTypeOptionToConversationNode(
            this NodeTypeOptionResource nodeTypeOptionResource,
            string nodeId,
            string text,
            bool isRoot,
            string nodeChildrenString,
            string nodeType,
            string accountId,
            string areaIdentifier,
            string optionPath,
            bool isDynamic,
            bool isCritical = false,
            string? nodeComponentType = null,
            int? resolveOrder = null,
            string? dynamicType = null,
            bool loopbackAnchor = false
        )
        {
            if (nodeComponentType == null && nodeTypeOptionResource.NodeComponentType == null)
            {
                throw new Exception("NodeComponent must be set for dynamic table node types"); // TODO: can I enforce this via the compiler? Rosalyn Analyzer
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
                NodeComponentType = nodeComponentType ?? nodeTypeOptionResource.NodeComponentType,
                OptionPath = optionPath,
                ValueOptions = string.Join(Delimiters.ValueOptionDelimiter, nodeTypeOptionResource.ValueOptions),
                AccountId = accountId,
                AreaIdentifier = areaIdentifier,
                IsMultiOptionType = nodeTypeOptionResource.IsMultiOptionType,
                IsTerminalType = nodeTypeOptionResource.IsTerminalType,
                IsDynamicTableNode = isDynamic,
                ResolveOrder = resolveOrder,
                DynamicType = dynamicType,
                ShouldRenderChildren = nodeTypeOptionResource.ShouldRenderChildren,
                IsAnabranchType = nodeTypeOptionResource.IsAnabranchType,
                IsAnabranchMergePoint = nodeTypeOptionResource.IsAnabranchMergePoint,
                IsMultiOptionEditable = nodeTypeOptionResource.IsMultiOptionEditable,
                ShouldShowMultiOption = nodeTypeOptionResource.ShouldShowMultiOption,
                IsCurrency = nodeTypeOptionResource.IsCurrency,
                IsCritical = isCritical,
                IsLoopbackAnchorType = loopbackAnchor,
                NodeTypeCode = nodeTypeOptionResource.NodeTypeCode
            };
        }
    }
}