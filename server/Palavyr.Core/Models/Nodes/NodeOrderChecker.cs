using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Models.Nodes
{
    public interface INodeOrderChecker
    {
        NodeOrderCheckResult AllPricingStrategyTypesAreOrderedCorrectlyByResolveOrder(ConversationNode[] nodeList);
    }

    public class NodeOrderChecker : INodeOrderChecker
    {
        private readonly INodeGetter nodeGetter;
        private readonly IGuidFinder guidFinder;

        public NodeOrderChecker(INodeGetter nodeGetter, IGuidFinder guidFinder)
        {
            this.nodeGetter = nodeGetter;
            this.guidFinder = guidFinder;
        }
        
        public NodeOrderCheckResult AllPricingStrategyTypesAreOrderedCorrectlyByResolveOrder(ConversationNode[] nodeList)
        {
            
            var pricingStrategyTypes = nodeList
                .Where(x => !string.IsNullOrWhiteSpace(x.PricingStrategyType))
                .Select(x => x.PricingStrategyType)
                .Distinct()
                .ToList();

            var orderResults = new List<bool>();
            var unorderedNodeTypes = new List<string>();
            foreach (var pricingStrategyType in pricingStrategyTypes)
            {
                // for each pricing strategy type, retrieve all of the nodes of this type (could be one or more here for multinode pricing stretegy types)
                var pricingStrategyTableNodes = nodeList
                    .Where(x => string.Equals(x.PricingStrategyType, pricingStrategyType))
                    .OrderBy(x => x.ResolveOrder)
                    .ToList();

                if (pricingStrategyTableNodes.Count == 1) continue; // not responsible for checking all required noes are included

                var nodeTypes = string.Join(",", pricingStrategyTableNodes.Select(x => guidFinder.RemoveGuid(x.NodeType).Trim('-')));
                var tableNodesAreOrderedInTheConvoCorrectly = true;
                for (var i = pricingStrategyTableNodes.Count - 1; i > 0; i--)
                {
                    var currentNodes = new List<ConversationNode>() {pricingStrategyTableNodes[i]}; // 2 of 0, 1, 2;

                    var parentToFind = pricingStrategyTableNodes[i - 1]; // 1 of 0, 1, 2;

                    var count = 0;
                    while (true)
                    {
                        if (currentNodes.Count == 1 && currentNodes.Single().IsRoot)
                        {
                            // then the current node is rootNode. 
                            tableNodesAreOrderedInTheConvoCorrectly = false;
                            unorderedNodeTypes.Add(nodeTypes);
                            break;
                        }

                        var parentNodes = nodeGetter.GetParentNodes(nodeList, currentNodes.ToArray());

                        if (parentNodes.Select(x => x.NodeId).Contains(parentToFind.NodeId))
                        {
                            break;
                        }

                        currentNodes = parentNodes.ToList();

                        count++;
                        if (count == 200)
                        {
                            throw new Exception("NodeList is to deep.");
                        }
                    }
                }

                orderResults.Add(tableNodesAreOrderedInTheConvoCorrectly);
            }

            return new NodeOrderCheckResult
            {
                IsOrdered = orderResults.All(x => x),
                ConcatenatedNodeTypes = unorderedNodeTypes
            };
        }
    }
}