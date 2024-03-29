﻿using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Models.Conversation
{
    public interface IOrphanRemover
    {
        List<ConversationNode> RemoveOrphanedNodes(IEnumerable<ConversationNode> conversationNodes);
    }

    public class OrphanRemover : IOrphanRemover
    {
        public List<ConversationNode> RemoveOrphanedNodes(IEnumerable<ConversationNode> conversationNodes)
        {
            var uniqueReferences = CompileAllReferences(conversationNodes);
            
            var deorphaned = new List<ConversationNode>();
            foreach (var node in conversationNodes)
            {
                if (IsReferencedByAnotherNode(node, uniqueReferences) || node.IsRoot)
                {
                    deorphaned.Add(node);
                }
            }

            return deorphaned;
        }

        private bool IsReferencedByAnotherNode(ConversationNode node, string[] uniqueReferences)
        {
            return uniqueReferences.Contains(node.NodeId);
        }

        private string[] CompileAllReferences(IEnumerable<ConversationNode> conversation)
        {
            var references = new List<string>();
            foreach (var conversationNode in conversation)
            {
                if (!string.IsNullOrWhiteSpace(conversationNode.NodeChildrenString))
                {
                    var children = conversationNode.NodeChildrenString.Split(",").ToList();
                    references.AddRange(children);
                }
            }

            return references.Distinct().ToArray();
        }
    }
}