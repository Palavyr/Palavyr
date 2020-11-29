using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Configuration.Schemas
{
    public class ConversationNode
    {
        [Key]
        public int? Id { get; set; }
        public string NodeId { get; set; }
        public string NodeType { get; set; }    
        public bool Fallback { get; set; }
        public string Text { get; set; }
        public bool IsRoot { get; set; }
        public string AreaIdentifier { get; set; }
        public string OptionPath { get; set; }
        public bool IsCritical { get; set; }
        public string ValueOptions { get; set; } // stored as comma delimited list as string
        public string AccountId { get; set; }
        public bool IsMultiOptionType { get; set; }
        public bool IsTerminalType { get; set; }
        

        public string NodeChildrenString { get; set; } // stored as comma delimited list as string
        
        public static List<ConversationNode> CreateDefaultNode(string AreaIdentifier, string accountId)
        {
            return new List<ConversationNode>()
            {
                new ConversationNode() {
                    NodeId = Guid.NewGuid().ToString(),
                    NodeType = "",
                    Fallback = false,
                    Text = "Default Text from server",
                    IsRoot = true,
                    AreaIdentifier = AreaIdentifier,
                    OptionPath = null,
                    NodeChildrenString = "",
                    ValueOptions = "",
                    IsCritical = false,
                    AccountId = accountId,
                    IsMultiOptionType = false,
                    IsTerminalType = false
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
            bool isTerminalType = false
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
                IsTerminalType = isTerminalType
            };
        }
    }
}