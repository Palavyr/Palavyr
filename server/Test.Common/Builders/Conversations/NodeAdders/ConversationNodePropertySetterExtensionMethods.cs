using System.Collections.Generic;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Services.DynamicTableService;

namespace Test.Common.Builders.Conversations.NodeAdders
{
    public static class ConversationNodePropertySetterExtensionMethods
    {
        public static SingleNodeReturnObject WithText(this SingleNodeReturnObject container, string nodeText)
        {
            container.PreviousNode.Text = nodeText;
            return container;
        }

        public static MultiNodeReturnObject WithText(this MultiNodeReturnObject container, string nodeText)
        {
            container.PreviousNode.Text = nodeText;
            return container;
        }


        public static void SetAreaId(this ConversationNode node, string areaId)
        {
            node.AreaIdentifier = areaId;
        }

        public static void SetNodeChildrenString(this ConversationNode node, string childrenString)
        {
            node.NodeChildrenString = childrenString;
        }

        public static void SetOptionPath(this ConversationNode node, string optionPath)
        {
            node.OptionPath = optionPath;
        }

        public static void SetValueOptions(this ConversationNode node, List<string> valueOptions)
        {
            var splitter = new ConversationOptionSplitter(new GuidFinder());
            node.ValueOptions = splitter.JoinValueOptions(valueOptions);
            node.SetIsMultiOptionType(true);
        }

        public static void SetAccountId(this ConversationNode node, string accountId)
        {
            node.AccountId = accountId;
        }

        public static void SetNodeComponentType(this ConversationNode node, string componentType)
        {
            node.NodeComponentType = componentType;
        }

        public static void SetIsRoot(this ConversationNode node, bool isRoot)
        {
            node.IsRoot = isRoot;
        }


        public static void SetIsCritical(this ConversationNode node, bool isCrit)
        {
            node.IsCritical = isCrit;
        }

        public static void SetIsMultiOptionType(this ConversationNode node, bool isMulti)
        {
            node.IsMultiOptionType = isMulti;
        }

        public static void SetIsTerminalType(this ConversationNode node, bool isTerminal)
        {
            node.IsTerminalType = isTerminal;
        }

        public static void SetShouldRenderChildren(this ConversationNode node, bool shouldRenderChildren)
        {
            node.ShouldRenderChildren = shouldRenderChildren;
        }


        public static void SetShouldShowMultiOption(this ConversationNode node, bool show)
        {
            node.ShouldShowMultiOption = show;
        }

        public static void SetIsAnabranch(this ConversationNode node, bool isAna)
        {
            node.IsAnabranchType = isAna;
        }

        public static void SetIsAnabranchMergePoint(this ConversationNode node, bool isAnaMergePoint)
        {
            node.IsAnabranchMergePoint = isAnaMergePoint;
        }

        public static void SetIsDynamic(this ConversationNode node, bool isDynamic)
        {
            node.IsDynamicTableNode = isDynamic;
        }

        public static void SetIsCurrency(this ConversationNode node, bool isCurrency)
        {
            node.IsCurrency = isCurrency;
        }

        public static void SetIsMultiOptionEditable(this ConversationNode node, bool editable)
        {
            node.IsMultiOptionEditable = editable;
        }

        public static void SetResolveOrder(this ConversationNode node, int resolveOrder)
        {
            node.ResolveOrder = resolveOrder;
        }

        public static void SetDynamicType(this ConversationNode node, string dynamicType)
        {
            node.DynamicType = dynamicType;
        }
    }
}