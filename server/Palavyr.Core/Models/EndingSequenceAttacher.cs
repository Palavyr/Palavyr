using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public interface IEndingSequenceAttacher
    {
        List<ConversationNode> AttachEndingSequenceToNodeList(List<ConversationNode> nodeList, string areaId, string accountId);
    }

    public class EndingSequenceAttacher : IEndingSequenceAttacher
    {
        private readonly IEndingSequenceNodes endingSequenceNodes;
        public static readonly string EmailSuccessfulNodeId = "EmailSuccessfulNodeId";
        public static readonly string EmailFailedNodeId = "EmailFailedNodeId";

        public static readonly string FallbackEmailSuccessfulNodeId = "FallbackEmailSuccessfulNodeId";
        public static readonly string FallbackEmailFailedNodeId = "FallbackEmailFailedNodeId";

        public EndingSequenceAttacher(IEndingSequenceNodes endingSequenceNodes)
        {
            this.endingSequenceNodes = endingSequenceNodes;
        }

        public static ConversationNode[] CleanTheIntroConvoEnding(ConversationNode[] introSequence)
        {
            var terminalIntroNode = introSequence.SingleOrDefault(x => x.IsTerminalType);
            terminalIntroNode.NodeChildrenString = "Transition-Selection";

            return introSequence;
        }
        
        
        public List<ConversationNode> AttachEndingSequenceToNodeList(List<ConversationNode> nodeList, string areaId, string accountId)
        {
            var sendEmail = endingSequenceNodes.CreateSendEmail(areaId, accountId, "Placeholder");
            var restartAfterDontSendEmail = endingSequenceNodes.CreateDontSendEmailRestart(areaId, accountId, "Placeholder");
            var mayWeSendAnEmail = endingSequenceNodes.CreateMayWeSendAnEmail(areaId, accountId, sendEmail.NodeId, restartAfterDontSendEmail.NodeId);
            var emailSendWasSuccessful = endingSequenceNodes.CreateEmailSendWasSuccessful(areaId, accountId, InternalNodeTypeOptions.Restart.StringName);
            var restart = endingSequenceNodes.CreateRestart(areaId, accountId, "Terminate");

            var retrySendEmailSecondAttempt = endingSequenceNodes.CreateRetrySendEmailSecondAttempt(areaId, accountId, "Placeholder");
            var emailSendFailedFirstAttempt = endingSequenceNodes.CreateEmailSendFailedFirstAttempt(areaId, accountId, retrySendEmailSecondAttempt.NodeId);
            var fallbackRetrySendEmailSecondAttempt = endingSequenceNodes.CreateFallbackRetrySendEmailSecondAttempt(areaId, accountId, "Placeholder");
            var fallbackEmailSendFailedFirstAttempt = endingSequenceNodes.CreateFallbackEmailSendFailedFirstAttempt(areaId, accountId, fallbackRetrySendEmailSecondAttempt.NodeId);

            var sendFallbackEmail = endingSequenceNodes.CreateSendFallbackEmail(areaId, accountId, "Placeholder");
            var mayWeSendAnInformationalEmailForTooComplicated = endingSequenceNodes.CreateMayWeSendAnInformationalEmailForTooComplicated(areaId, accountId, sendFallbackEmail.NodeId, restartAfterDontSendEmail.NodeId);

            var genericTooComplicated = endingSequenceNodes.CreateGenericTooComplicated(areaId, accountId);


            nodeList.Add(genericTooComplicated);
            foreach (var node in nodeList)
            {
                if (node.IsTerminalType)
                {
                    if (node.NodeType == DefaultNodeTypeOptions.TooComplicated.StringName)
                    {
                        node.NodeChildrenString = mayWeSendAnInformationalEmailForTooComplicated.NodeId;
                        continue;
                    }
            
                    if (node.NodeType == DefaultNodeTypeOptions.SendResponse.StringName)
                    {
                        node.NodeChildrenString = mayWeSendAnEmail.NodeId; // we're deciding that the thanksId node will be the entry point in to the response ending sequence
                        continue;
                    }
            
                    if (node.NodeType == DefaultNodeTypeOptions.EndWithoutEmail.StringName)
                    {
                        node.NodeChildrenString = restartAfterDontSendEmail.NodeId;
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
                    mayWeSendAnInformationalEmailForTooComplicated,
                    sendFallbackEmail
                });
            return nodeList;
        }
    }
}