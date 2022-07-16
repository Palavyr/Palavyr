using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.PricingStrategyTableServices.NodeUpdaters
{
    public interface ISelectOneFlatNodeUpdater
    {
        Task UpdateConversationNode(
            List<CategorySelectTableRow> currentSelectOneFlatUpdate,
            PricingStrategyTableMeta tableTableMeta,
            ConversationNode node,
            List<ConversationNode> conversationNodes,
            string intentId
        );
    }

    public class SelectOneFlatNodeUpdater : ISelectOneFlatNodeUpdater
    {
        private readonly IConversationOptionSplitter splitter;
        private readonly IConversationNodeUpdater nodeUpdater;
        private readonly IAccountIdTransport accountIdTransport;

        public SelectOneFlatNodeUpdater(
            IConversationOptionSplitter splitter,
            IConversationNodeUpdater nodeUpdater,
            IAccountIdTransport accountIdTransport
        )
        {
            this.splitter = splitter;
            this.nodeUpdater = nodeUpdater;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task UpdateConversationNode(
            List<CategorySelectTableRow> currentSelectOneFlatUpdate,
            PricingStrategyTableMeta tableTableMeta,
            ConversationNode node,
            List<ConversationNode> conversationNodes,
            string intentId
        )
        {
            List<ConversationNode> updatedNodes;
            if (tableTableMeta.ValuesAsPaths)
            {
                updatedNodes = await ConvertToAsPaths(currentSelectOneFlatUpdate, node, intentId, accountIdTransport.AccountId, conversationNodes);
            }
            else
            {
                updatedNodes = await ConvertToAsContinue(node, conversationNodes);
            }

            await nodeUpdater.UpdateConversation(intentId, updatedNodes, CancellationToken.None);
        }

        private async Task<List<ConversationNode>> ConvertToAsPaths(List<CategorySelectTableRow> currentSelectOneFlatUpdate, ConversationNode node, string intentId, string accountId, List<ConversationNode> conversationNodes)
        {
            // 1. from not as paths
            // 2. add 1 or more siblings
            // 3. remove 1 or more siblings

            var currentOptions = splitter.SplitValueOptions(node.ValueOptions);
            var currentNumberOfOptions = currentOptions.Count;
            var currentNodeChildrenStrings = splitter.SplitNodeChildrenString(node.NodeChildrenString);

            var incomingOptions = currentSelectOneFlatUpdate.Select(x => x.Category).ToList();
            var numberOfIncomingOptions = incomingOptions.Count;
            var newValueOptionsString = string.Join(Delimiters.ValueOptionDelimiter, incomingOptions);

            node.ValueOptions = newValueOptionsString;
            node.NodeComponentType = DefaultNodeTypeOptions.MultipleChoiceContinue.StringName;

            var newNodeChildren = new List<string>();
            newNodeChildren.AddRange(currentNodeChildrenStrings);

            if (numberOfIncomingOptions > currentNumberOfOptions) // adding nodes
            {
                for (var i = 0; i < numberOfIncomingOptions - currentNumberOfOptions; i++)
                {
                    var newDefaultNode = ConversationNode.CreateDefaultNode(intentId, accountId).Single();
                    conversationNodes.Add(newDefaultNode);
                    node.AddChildId(newDefaultNode.NodeId!, splitter);
                    newNodeChildren.Add(newDefaultNode.NodeId);
                }
            }
            else if (numberOfIncomingOptions < currentNumberOfOptions)
            {
                // remove nodes that don't belong anymore
                var remainingChildrenNodeStrings = currentNodeChildrenStrings.Take(numberOfIncomingOptions).ToList(); // NEED TO GRAB INDICES THAT ARE MISSING INSTEAD
                var excludedChildStrings = currentNodeChildrenStrings.Where(ncs => !remainingChildrenNodeStrings.Contains(ncs));
                conversationNodes = conversationNodes.Where(n => !excludedChildStrings.Contains(n.NodeId)).ToList();
                newNodeChildren = newNodeChildren.Take(numberOfIncomingOptions).ToList();
            }

            for (var i = 0; i < numberOfIncomingOptions; i++)
            {
                var currentNode = conversationNodes.Single(n => n.NodeId == newNodeChildren[i]);
                currentNode.OptionPath = incomingOptions[i];
            }

            node.NodeChildrenString = splitter.JoinNodeChildrenArray(newNodeChildren);

            await Task.CompletedTask;

            return conversationNodes;
        }

        private async Task<List<ConversationNode>> ConvertToAsContinue(ConversationNode node, List<ConversationNode> conversationNodes)
        {
            node.ValueOptions = "Continue";
            node.NodeComponentType = DefaultNodeTypeOptions.MultipleChoiceAsPath.StringName;
            var unwantedChildIds = splitter.SplitNodeChildrenString(node.NodeChildrenString)[1..];
            foreach (var unwantedChildId in unwantedChildIds)
            {
                var unwantedNode = conversationNodes.Single(x => x.NodeId == unwantedChildId);
                conversationNodes.Remove(unwantedNode);
            }

            node.TruncateChildIdsAt(0, splitter);
            var singleChild = conversationNodes.Single(x => x.NodeId == node.NodeChildrenString);
            singleChild.OptionPath = "Continue";

            await Task.CompletedTask;

            return conversationNodes;
        }
    }
}