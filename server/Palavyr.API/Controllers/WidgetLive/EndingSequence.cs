using System.Collections.Generic;
using Server.Domain.Configuration.Constant;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.WidgetLive
{
    /// <summary>
    /// Temp class to hold the standard ending sequence nodes which are attached automatically when the widget fetches
    /// </summary>
    public static class EndingSequence
    {
        public static List<ConversationNode> AttachEndingSequenceToNodeList(List<ConversationNode> nodeList,
            string areaId, string accountId)
        {
            // TODO: replace later with user customized ending sequence nodes;
            var name = ConversationNode.CreateNew(
                DefaultNodeTypeOptions.Name.StringName,
                DefaultNodeTypeOptions.Name.StringName,
                "Could you please provide your name?",
                areaId,
                DefaultNodeTypeOptions.Phone.StringName,
                "",
                "",
                accountId,
                false,
                true,
                false,
                false
            );
            var phone = ConversationNode.CreateNew(
                DefaultNodeTypeOptions.Phone.StringName,
                DefaultNodeTypeOptions.Phone.StringName,
                "Could you please provide your phone number? If you would prefer to keep your phone number private, leave this input blank.",
                areaId,
                DefaultNodeTypeOptions.Email.StringName,
                "",
                "",
                accountId,
                false,
                true,
                false,
                false
            );
            var email = ConversationNode.CreateNew(
                DefaultNodeTypeOptions.Email.StringName,
                DefaultNodeTypeOptions.Email.StringName,
                "Please provide an email we can use to contact you.",
                areaId,
                DefaultNodeTypeOptions.SendEmail.StringName,
                "",
                "",
                accountId,
                false,
                true,
                false,
                false
            );
            var emailInfo = ConversationNode.CreateNew(
                DefaultNodeTypeOptions.SendEmail.StringName,
                DefaultNodeTypeOptions.SendEmail.StringName,
                "Wait just a moment while I send you a confirmation email with some information",
                areaId,
                DefaultNodeTypeOptions.Restart.StringName,
                "",
                "",
                accountId,
                false,
                false,
                false,
                true
            );
            var restart = ConversationNode.CreateNew(
                DefaultNodeTypeOptions.Restart.StringName,
                DefaultNodeTypeOptions.Restart.StringName,
                "Thanks for your time. I'm going to restart this window now.",
                areaId,
                "Terminate",
                "",
                "",
                accountId,
                false,
                false,
                false,
                true
            );

            foreach (var node in nodeList)
            {
                if (node.NodeType == DefaultNodeTypeOptions.EndingSequence.StringName ||
                    node.NodeType == DefaultNodeTypeOptions.TooComplicated.StringName)
                {
                    node.NodeChildrenString = DefaultNodeTypeOptions.Name.StringName; // First in the ending sequence
                }
            }

            nodeList.AddRange(new List<ConversationNode> {name, email, phone, emailInfo, restart});
            return nodeList;
        }
    }
}