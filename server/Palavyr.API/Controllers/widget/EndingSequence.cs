using System.Collections.Generic;
using Server.Domain;

namespace Palavyr.API.Controllers
{
    /// <summary>
    /// Temp class to hold the standard ending sequence nodes which are attached automatically when the widget fetches
    /// </summary>
    public static class EndingSequence
    {
        public static List<ConversationNode> AttachEndingSequenceToNodeList(List<ConversationNode> nodeList, string areaId, string accountId)
        {
            
            // TODO: replace later with user customized ending sequence nodes;
            var name = ConversationNode.CreateNew(
                NodeTypes.Name, 
                NodeTypes.Name,
                "Could you please provide your name?", 
                areaId,
                NodeTypes.Phone,
                "",
                accountId,
                false,
                true
            );
            var phone = ConversationNode.CreateNew(
                NodeTypes.Phone,
                NodeTypes.Phone,
                "Could you please provide your phone number? If you would prefer to keep your phone number private, leave this input blank.",
                areaId, 
                NodeTypes.Email,
                "",
                accountId,
                false,
                true
            );
            var email = ConversationNode.CreateNew(
                NodeTypes.Email,
                NodeTypes.Email,
                "Please provide an email we can use to contact you.", 
                areaId, 
                NodeTypes.SendEmail,
                "",
                accountId,
                false,
                true
                );
            var emailInfo = ConversationNode.CreateNew(
                NodeTypes.SendEmail,
                NodeTypes.SendEmail,
                "Wait just a moment while I send you a confirmation email with some information",
                areaId,
                NodeTypes.Restart,
                "",
                accountId,
                false,
                false
            );
            var restart = ConversationNode.CreateNew(
                NodeTypes.Restart,
                NodeTypes.Restart,
                "Thanks for your time. I'm going to restart this window now.",
                areaId,
                "Terminate",
                "",
                accountId,
                false,
                false
            );

            foreach (var node in nodeList)
            {
                if (node.NodeType == NodeTypes.EndingSequence || node.NodeType == NodeTypes.TooComplicated)
                {
                    node.NodeChildrenString = NodeTypes.Name; // First in the ending sequence
                }
            }
            
            nodeList.AddRange(new List<ConversationNode> {name, email, phone, emailInfo, restart});
            return nodeList;
        }
    }
}