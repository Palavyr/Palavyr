using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Palavyr.Domain.Configuration.Schemas
{
    public class ConversationNode
    {
        [Key] public int? Id { get; set; }
        public string NodeId { get; set; }
        public string NodeType { get; set; } = null!;
        public bool Fallback { get; set; }
        public string Text { get; set; } = null!;
        public bool IsRoot { get; set; }
        public string AreaIdentifier { get; set; } = null!;
        public string OptionPath { get; set; }
        public bool IsCritical { get; set; }
        public string ValueOptions { get; set; } = null!; // stored as comma delimited list as string
        public string AccountId { get; set; } = null!;
        public bool IsMultiOptionType { get; set; }
        public bool IsTerminalType { get; set; }
        public bool ShouldRenderChildren { get; set; }
        public bool IsSplitMergeType { get; set; }
        public bool ShouldShowMultiOption { get; set; }
        public bool IsAnabranchType  { get; set; }
        public bool IsAnabranchMergePoint { get; set; }

        public string NodeChildrenString { get; set; } = null!; // stored as comma delimited list as string

        public ConversationNode()
        {
        }

        public static List<ConversationNode> CreateDefaultNode(string areaIdentifier, string accountId)
        {
            return new List<ConversationNode>()
            {
                new ConversationNode()
                {
                    NodeId = Guid.NewGuid().ToString(),
                    NodeType = "",
                    Fallback = false,
                    Text = "Default Text from server",
                    IsRoot = true,
                    AreaIdentifier = areaIdentifier,
                    OptionPath = null,
                    NodeChildrenString = "",
                    ValueOptions = "",
                    IsCritical = false,
                    AccountId = accountId,
                    IsMultiOptionType = false,
                    IsTerminalType = false,
                    ShouldRenderChildren = true,
                    IsSplitMergeType = false,
                    ShouldShowMultiOption = false,
                    IsAnabranchType = false,
                    IsAnabranchMergePoint = false
                }
            };
        }

        public static ConversationNode CreateNew(
            string nodeId,
            string nodeType,
            string text,
            string areaIdentifier,
            string nodeChildrenString,
            string optionPath,
            string valueOptions,
            string accountId,
            bool isRoot = false,
            bool isCritical = true,
            bool isMultiOptionType = false,
            bool isTerminalType = false,
            bool shouldRenderChild = true,
            bool isSplitMergeType = false,
            bool shouldShowMultiOption = false,
            bool isAnabranchType = false,
            bool isAnabranchMergePoint = false
        )
        {
            return new ConversationNode()
            {
                NodeId = nodeId,
                NodeType = nodeType,
                Fallback = false,
                Text = text,
                IsRoot = isRoot,
                AreaIdentifier = areaIdentifier,
                NodeChildrenString = nodeChildrenString,
                OptionPath = optionPath,
                ValueOptions = valueOptions,
                IsCritical = isCritical,
                AccountId = accountId,
                IsMultiOptionType = isMultiOptionType,
                IsTerminalType = isTerminalType,
                ShouldRenderChildren = shouldRenderChild,
                IsSplitMergeType = isSplitMergeType,
                ShouldShowMultiOption = shouldShowMultiOption,
                IsAnabranchType = isAnabranchType,
                IsAnabranchMergePoint = isAnabranchMergePoint
            };
        }

        public static List<ConversationNode> MapUpdate(string accountId, List<ConversationNode> nodeUpdates)
        {
            var mappedTransactions = new List<ConversationNode>();
            foreach (var node in nodeUpdates)
            {
                var mappedNode = CreateNew(
                    node.NodeId,
                    node.NodeType,
                    node.Text,
                    node.AreaIdentifier,
                    node.NodeChildrenString,
                    node.OptionPath,
                    node.ValueOptions,
                    accountId,
                    node.IsRoot,
                    node.IsCritical,
                    node.IsMultiOptionType,
                    node.IsTerminalType,
                    node.ShouldRenderChildren,
                    node.IsSplitMergeType,
                    node.ShouldShowMultiOption,
                    node.IsAnabranchType,
                    node.IsAnabranchMergePoint
                );
                mappedTransactions.Add(mappedNode);
            }
            return mappedTransactions;
        }
    }
}