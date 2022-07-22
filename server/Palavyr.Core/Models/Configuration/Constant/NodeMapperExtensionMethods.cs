using System;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public static class NodeMapperExtensionMethods
    {
        public static ConversationNode MapNodeTypeOptionToConversationNode(
            this NodeTypeOptionResource nodeTypeOptionResource,
            string nodeId,
            string text,
            bool isRoot,
            string nodeChildrenString,
            string nodeType,
            string accountId,
            string intentId,
            string optionPath,
            bool isPricingStrategy,
            bool isCritical = false,
            string nodeComponentType = "",
            int resolveOrder = 0,
            string pricingStrategyType = "",
            bool loopbackAnchor = false
        )
        {
            if (nodeComponentType == null && nodeTypeOptionResource.NodeComponentType == null)
            {
                throw new Exception("NodeComponent must be set for pricing strategy table node types"); // TODO: can I enforce this via the compiler? Rosalyn Analyzer
            }

            return new ConversationNode
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
                IntentId = intentId,
                IsMultiOptionType = nodeTypeOptionResource.IsMultiOptionType,
                IsTerminalType = nodeTypeOptionResource.IsTerminalType,
                IsPricingStrategyTableNode = isPricingStrategy,
                ResolveOrder = resolveOrder,
                PricingStrategyType = pricingStrategyType,
                ShouldRenderChildren = nodeTypeOptionResource.ShouldRenderChildren,
                IsAnabranchType = nodeTypeOptionResource.IsAnabranchType,
                IsAnabranchMergePoint = nodeTypeOptionResource.IsAnabranchMergePoint,
                IsMultiOptionEditable = nodeTypeOptionResource.IsMultiOptionEditable,
                ShouldShowMultiOption = nodeTypeOptionResource.ShouldShowMultiOption,
                IsCurrency = nodeTypeOptionResource.IsCurrency,
                IsCritical = isCritical,
                IsLoopbackAnchorType = loopbackAnchor,
                NodeTypeCodeEnum = nodeTypeOptionResource.NodeTypeCodeEnum
            };
        }
    }
}