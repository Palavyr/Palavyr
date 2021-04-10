using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Palavyr.Core.Models.Contracts;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public class ConversationNode : IRecord
    {
        [Key] public int? Id { get; set; }
        public string? AreaIdentifier { get; set; }
        public string? AccountId { get; set; }
        public string? NodeId { get; set; }
        public string? NodeType { get; set; }
        public string? DynamicType { get; set; }
        public bool Fallback { get; set; }
        public string? Text { get; set; }
        public bool IsRoot { get; set; }
        public string? OptionPath { get; set; }
        public bool IsCritical { get; set; }
        public string? ValueOptions { get; set; } // stored as comma delimited list as string
        public bool IsMultiOptionType { get; set; }
        public bool IsTerminalType { get; set; }
        public bool ShouldRenderChildren { get; set; }
        public bool IsSplitMergeType { get; set; }
        public bool ShouldShowMultiOption { get; set; }
        public bool IsAnabranchType { get; set; }
        public bool IsAnabranchMergePoint { get; set; }
        public bool IsDynamicTableNode { get; set; }
        public string? NodeComponentType { get; set; }
        public bool IsMultiOptionEditable { get; set; }
        public bool IsCurrency { get; set; }
        public int? ResolveOrder { get; set; }


        public string? NodeChildrenString { get; set; } // stored as comma delimited list as string

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
                    OptionPath = "", // Previous had this set to null...
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
                    IsAnabranchMergePoint = false,
                    IsDynamicTableNode = false,
                    IsCurrency = false,
                    IsMultiOptionEditable = false,
                    DynamicType = null
                }
            };
        }

        public static ConversationNode CreateNew(
            string? nodeId,
            string? nodeType,
            string? text,
            string? areaIdentifier,
            string? nodeChildrenString,
            string? optionPath,
            string? valueOptions,
            string accountId,
            string? nodeComponentType,
            bool isRoot = false,
            bool isCritical = true,
            bool isMultiOptionType = false,
            bool isTerminalType = false,
            bool shouldRenderChild = true,
            bool isSplitMergeType = false,
            bool shouldShowMultiOption = false,
            bool isAnabranchType = false,
            bool isAnabranchMergePoint = false,
            bool isDynamicTableNode = false,
            bool isCurrency = false,
            bool isMultiOptionEditable = false,
            int? resolveOrder = null,
            string? dynamicType = null
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
                IsAnabranchMergePoint = isAnabranchMergePoint,
                IsCurrency = isCurrency,
                IsMultiOptionEditable = isMultiOptionEditable,
                IsDynamicTableNode = isDynamicTableNode,
                ResolveOrder = resolveOrder,
                NodeComponentType = nodeComponentType,
                DynamicType = dynamicType
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
                    node.NodeComponentType,
                    node.IsRoot,
                    node.IsCritical,
                    node.IsMultiOptionType,
                    node.IsTerminalType,
                    node.ShouldRenderChildren,
                    node.IsSplitMergeType,
                    node.ShouldShowMultiOption,
                    node.IsAnabranchType,
                    node.IsAnabranchMergePoint,
                    node.IsDynamicTableNode,
                    node.IsCurrency,
                    node.IsMultiOptionEditable,
                    node.ResolveOrder,
                    node.DynamicType
                );
                mappedTransactions.Add(mappedNode);
            }

            return mappedTransactions;
        }
    }
}