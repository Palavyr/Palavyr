using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Data.Entities
{
    public class ConversationNode : Entity, IRecord, IHaveAccountId
    {
        public string IntentId { get; set; }
        public string AccountId { get; set; }
        public string NodeId { get; set; }
        public string Text { get; set; }
        public bool IsRoot { get; set; }
        public bool IsCritical { get; set; }
        public bool IsMultiOptionType { get; set; }
        public bool IsTerminalType { get; set; }
        public bool ShouldRenderChildren { get; set; }
        public bool IsLoopbackAnchorType { get; set; }
        public bool IsAnabranchType { get; set; }
        public bool IsAnabranchMergePoint { get; set; }
        public bool ShouldShowMultiOption { get; set; }
        public bool IsPricingStrategyTableNode { get; set; }
        public bool IsMultiOptionEditable { get; set; }
        public bool IsImageNode { get; set; }
        public string FileId { get; set; }
        public string OptionPath { get; set; }
        public string ValueOptions { get; set; }
        public string NodeType { get; set; }
        public string PricingStrategyType { get; set; }
        public string NodeComponentType { get; set; }
        public int ResolveOrder { get; set; }
        public bool IsCurrency { get; set; }
        public string NodeChildrenString { get; set; } = ""; // stored as comma delimited list as string
        public NodeTypeCode NodeTypeCode { get; set; }

        public ConversationNode()
        {
        }

        public void AddChildId(string newChildId, IConversationOptionSplitter splitter)
        {
            var childrenArray = splitter.SplitNodeChildrenString(NodeChildrenString).ToList();
            childrenArray.Add(newChildId);
            NodeChildrenString = splitter.JoinNodeChildrenArray(childrenArray);
        }

        public void TruncateChildIdsAt(int index, IConversationOptionSplitter splitter)
        {
            var childrenArray = splitter.SplitNodeChildrenString(NodeChildrenString).Take(index + 1);
            NodeChildrenString = splitter.JoinNodeChildrenArray(childrenArray.ToList());
        }

        public static List<ConversationNode> CreateDefaultNode(string intentId, string accountId, bool isRoot = false)
        {
            return new List<ConversationNode>
            {
                new ConversationNode
                {
                    NodeId = Guid.NewGuid().ToString(),
                    NodeType = "",
                    Text = "Click to add some meaningful text. Don't forget to add some personality!",
                    IsRoot = isRoot,
                    IntentId = intentId,
                    OptionPath = string.Empty, // Previous had this set to null...
                    NodeChildrenString = string.Empty,
                    ValueOptions = "",
                    IsCritical = false,
                    AccountId = accountId,
                    IsMultiOptionType = false,
                    IsTerminalType = false,
                    ShouldRenderChildren = true,
                    ShouldShowMultiOption = false,
                    IsAnabranchType = false,
                    IsAnabranchMergePoint = false,
                    IsPricingStrategyTableNode = false,
                    IsCurrency = false,
                    IsMultiOptionEditable = false,
                    PricingStrategyType = string.Empty,
                    IsImageNode = false,
                    FileId = string.Empty,
                    IsLoopbackAnchorType = false,
                    NodeComponentType = DefaultNodeTypeOptions.NodeComponentTypes.ProvideInfo,
                    ResolveOrder = 0,
                    NodeTypeCode = NodeTypeCode.II
                }
            };
        }

        public static List<ConversationNode> CreateDefaultRootNode(string intentId, string accountId)
        {
            return CreateDefaultNode(intentId, accountId, isRoot: true);
        }

        public static ConversationNode CreateNew(
            string nodeId,
            string nodeType,
            string text,
            string intentId,
            string nodeChildrenString,
            string optionPath,
            string valueOptions,
            string accountId,
            string nodeComponentType,
            NodeTypeCode nodeTypeCode,
            bool isRoot = false,
            bool isCritical = true,
            bool isMultiOptionType = false,
            bool isTerminalType = false,
            bool shouldRenderChild = true,
            bool shouldShowMultiOption = false,
            bool isAnabranchType = false,
            bool isAnabranchMergePoint = false,
            bool isPricingStrategyTableNode = false,
            bool isCurrency = false,
            bool isMultiOptionEditable = false,
            int resolveOrder = 0,
            string pricingStrategyType = null,
            bool isImageNode = false,
            string imageId = "",
            bool isLoopbackAnchor = false
        )
        {
            return new ConversationNode
            {
                NodeId = nodeId,
                NodeType = nodeType,
                Text = text,
                IsRoot = isRoot,
                IntentId = intentId,
                NodeChildrenString = nodeChildrenString,
                OptionPath = optionPath,
                ValueOptions = valueOptions,
                IsCritical = isCritical,
                AccountId = accountId,
                IsMultiOptionType = isMultiOptionType,
                IsTerminalType = isTerminalType,
                ShouldRenderChildren = shouldRenderChild,
                ShouldShowMultiOption = shouldShowMultiOption,
                IsAnabranchType = isAnabranchType,
                IsAnabranchMergePoint = isAnabranchMergePoint,
                IsCurrency = isCurrency,
                IsMultiOptionEditable = isMultiOptionEditable,
                IsPricingStrategyTableNode = isPricingStrategyTableNode,
                ResolveOrder = resolveOrder,
                NodeComponentType = nodeComponentType,
                PricingStrategyType = pricingStrategyType,
                IsImageNode = isImageNode,
                FileId = imageId,
                IsLoopbackAnchorType = isLoopbackAnchor,
                NodeTypeCode = nodeTypeCode
            };
        }
    }
}