using System;
using System.Collections.Generic;
using Palavyr.API.Utils;
using Palavyr.Common.UIDUtils;
using Server.Domain.Configuration.Constant;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.WidgetLive
{
    /// <summary>
    /// Class to hold the response email
    /// </summary>
    public static class EndingSequence
    {
        public static readonly string EmailSuccessfulNodeId = "EmailSuccessfulNodeId";
        public static readonly string EmailFailedNodeId = "EmailFailedNodeId";

        public static readonly string FallbackEmailSuccessfulNodeId = "FallbackEmailSuccessfulNodeId";
        public static readonly string FallbackEmailFailedNodeId = "FallbackEmailFailedNodeId";

        public static List<ConversationNode> AttachEndingSequenceToNodeList(List<ConversationNode> nodeList, string areaId, string accountId)
        {
            var mayWeSendAnEmailId = GuidUtils.CreateNewId();
            var sendEmailId = GuidUtils.CreateNewId();
            var dontSendEmailRestartId = GuidUtils.CreateNewId();
            var retrySendEmailSecondAttemptId = GuidUtils.CreateNewId();
            var fallbackRetrySendEmailSecondAttemptId = GuidUtils.CreateNewId();
            var sendTooComplicatedEmailId = GuidUtils.CreateNewId();
            var mayWeSendAnEmailTooComplicatedId = GuidUtils.CreateNewId();
            
            var mayWeSendAnEmail = ConversationNode.CreateNew(
                mayWeSendAnEmailId,
                DefaultNodeTypeOptions.YesNo.StringName,
                "We'd like to send you an email with some information regarding your enquiry. Would that be okay?",
                areaId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(sendEmailId, dontSendEmailRestartId), // TODO: Get the nodeIds from the yes and No
                "",
                TreeUtils.CreateValueOptions(DefaultNodeTypeOptions.YesNo.No, DefaultNodeTypeOptions.YesNo.Yes),
                accountId,
                false,
                false,
                true,
                false
            );

            var sendEmail = ConversationNode.CreateNew(
                sendEmailId,
                DefaultNodeTypeOptions.SendEmail.StringName,
                "Wait just a moment while I send you a confirmation email with some information",
                areaId,
                nodeChildrenString: "Placeholder", // The node child here is not set because we send the email and provide the ID of the next node dynamically depending on the email send result. (SendWdigetResonseEmailController)
                DefaultNodeTypeOptions.YesNo.Yes,
                "",
                accountId,
                false,
                false,
                false,
                false
            );

            var restartAfterDontSendEmail = ConversationNode.CreateNew(
                dontSendEmailRestartId,
                DefaultNodeTypeOptions.Restart.StringName,
                "Thanks for your time. If you'd like to start again, click the button below.",
                areaId,
                "",
                DefaultNodeTypeOptions.YesNo.No,
                "",
                accountId,
                false,
                false,
                false,
                true
            );

            var emailSendWasSuccessful = ConversationNode.CreateNew(
                EmailSuccessfulNodeId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                "We've sent through an email to the address you provided. Sometimes these emails are picked up as spam, so if you don't see it, be sure to check your spam folder.",
                areaId,
                DefaultNodeTypeOptions.Restart.StringName,
                "",
                "",
                accountId,
                false,
                false,
                false,
                false
            );

            var restart = ConversationNode.CreateNew(
                DefaultNodeTypeOptions.Restart.StringName,
                DefaultNodeTypeOptions.Restart.StringName,
                "Thanks for your time. If you'd like to start again, click the button below.",
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
            
            var emailSendFailedFirstAttempt = ConversationNode.CreateNew(
                EmailFailedNodeId,
                "EmailSendFailedFirstAttempt",
                "Hmm, we were not able to send an email to the address provided. Could you check that is correct?",
                areaId,
                retrySendEmailSecondAttemptId,
                "",
                "",
                accountId,
                false,
                false,
                false,
                false
            );
                       
            var fallbackEmailSendFailedFirstAttempt = ConversationNode.CreateNew(
                FallbackEmailFailedNodeId,
                "EmailSendFailedFirstAttempt",
                "Hmm, we were not able to send an email to the address provided. Could you check that is correct?",
                areaId,
                fallbackRetrySendEmailSecondAttemptId,
                "",
                "",
                accountId,
                false,
                false,
                false,
                false
            );

            var retrySendEmailSecondAttempt = ConversationNode.CreateNew(
                retrySendEmailSecondAttemptId,
                DefaultNodeTypeOptions.SendEmail.StringName,
                "Wait just a moment while I try that again.",
                areaId,
                nodeChildrenString: "Placeholder", // The node child here is not set because we send the email and provide the ID of the next node dynamically depending on the email send result. (SendWdigetResonseEmailController)
                "",
                "",
                accountId,
                false,
                false,
                false,
                false
            );

            var fallbackRetrySendEmailSecondAttempt = ConversationNode.CreateNew(
                fallbackRetrySendEmailSecondAttemptId,
                DefaultNodeTypeOptions.SendEmail.StringName,
                "Wait just a moment while I try that again.",
                areaId,
                nodeChildrenString: "Placeohlder", // The node child here is not set because we send the email and provide the ID of the next node dynamically depending on the email send result. (SendWdigetResonseEmailController)
                "",
                "",
                accountId,
                false,
                false,
                false,
                false
            );

            var mayWeSendAnInformationalEmail_ForTooComplicated = ConversationNode.CreateNew(
                mayWeSendAnEmailTooComplicatedId,
                DefaultNodeTypeOptions.YesNo.StringName,
                "We'd like to send you a followup email with some general information regarding your enquiry. Would that be okay?",
                areaId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(sendTooComplicatedEmailId, dontSendEmailRestartId),
                "",
                TreeUtils.CreateValueOptions(DefaultNodeTypeOptions.YesNo.No, DefaultNodeTypeOptions.YesNo.Yes),
                accountId,
                false,
                false,
                true,
                false
            );
            
            var sendFallbackEmail = ConversationNode.CreateNew( // leads to sending an email to the 'too complicated controller'
                sendTooComplicatedEmailId,
                DefaultNodeTypeOptions.SendTooComplicatedEmail.StringName,
                "Wait just a moment while I send an email.",
                areaId,
                nodeChildrenString: "Placeholder", // The node child here is not set because we send the email and provide the ID of the next node dynamically depending on the email send result. (SendWdigetResonseEmailController)
                DefaultNodeTypeOptions.YesNo.Yes,
                "",
                accountId,
                false,
                false,
                false,
                false
            );
            
            foreach (var node in nodeList)
            {
                if (node.IsTerminalType)
                {
                    if (node.NodeType == DefaultNodeTypeOptions.TooComplicated.StringName)
                    {
                        node.NodeChildrenString = mayWeSendAnEmailTooComplicatedId;
                        continue;
                    }

                    if (node.NodeType == DefaultNodeTypeOptions.SendResponse.StringName)
                    {
                        node.NodeChildrenString = mayWeSendAnEmailId; // we're deciding that the thanksId node will be the entry point in to the response ending sequence
                        continue;
                    }

                    throw new Exception($"Our bad - Node type: {node.NodeType} is not handled (EndingSequence.cs)");
                }
            }

            nodeList.AddRange(
                new List<ConversationNode>
                {
                    mayWeSendAnEmail,
                    sendEmail,
                    restartAfterDontSendEmail,
                    emailSendWasSuccessful,
                    restart,
                    emailSendFailedFirstAttempt,
                    retrySendEmailSecondAttempt,
                    fallbackEmailSendFailedFirstAttempt, 
                    fallbackRetrySendEmailSecondAttempt, 
                    mayWeSendAnInformationalEmail_ForTooComplicated, 
                    sendFallbackEmail
                });
            return nodeList;
        }
    }
}