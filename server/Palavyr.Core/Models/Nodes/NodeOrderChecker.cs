using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Nodes
{
    public class NodeOrderChecker
    {
        private readonly NodeGetter nodeGetter;
        private readonly GuidFinder guidFinder;

        public NodeOrderChecker(NodeGetter nodeGetter, GuidFinder guidFinder)
        {
            this.nodeGetter = nodeGetter;
            this.guidFinder = guidFinder;
        }
        
        public NodeOrderCheckResult AllDynamicTypesAreOrderedCorrectlyByResolveOrder(ConversationNode[] nodeList)
        {
            
            var dynamicTypes = nodeList
                .Where(x => !string.IsNullOrWhiteSpace(x.DynamicType))
                .Select(x => x.DynamicType)
                .Distinct()
                .ToList();

            var orderResults = new List<bool>();
            var unorderedNodeTypes = new List<string>();
            foreach (var dynamicType in dynamicTypes)
            {
                // for each dynamic type, retrieve all of the nodes of this type (could be one or more here for multinode dynamic types)
                var dynamicTableNodes = nodeList
                    .Where(x => string.Equals(x.DynamicType, dynamicType))
                    .OrderBy(x => x.ResolveOrder)
                    .ToList();

                if (dynamicTableNodes.Count == 1) continue; // not responsible for checking all required noes are included

                var nodeTypes = string.Join(",", dynamicTableNodes.Select(x => guidFinder.RemoveGuid(x.NodeType).Trim('-')));
                var tableNodesAreOrderedInTheConvoCorrectly = true;
                for (var i = dynamicTableNodes.Count - 1; i > 0; i--)
                {
                    var currentNodes = new List<ConversationNode>() {dynamicTableNodes[i]}; // 2 of 0, 1, 2;

                    var parentToFind = dynamicTableNodes[i - 1]; // 1 of 0, 1, 2;

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