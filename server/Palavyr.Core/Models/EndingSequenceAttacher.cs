using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public interface IEndingSequenceAttacher
    {
        List<ConversationNode> AttachEndingSequenceToNodeList(List<ConversationNode> nodeList, string intentId, string accountId);
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

        public List<ConversationNode> AttachEndingSequenceToNodeList(List<ConversationNode> nodeList, string intentId, string accountId)
        {
            var sendEmail = endingSequenceNodes.CreateSendEmail(intentId, accountId, "Placeholder");
            var restartAfterDontSendEmail = endingSequenceNodes.CreateDontSendEmailRestart(intentId, accountId, "Placeholder");
            var mayWeSendAnEmail = endingSequenceNodes.CreateMayWeSendAnEmail(intentId, accountId, sendEmail.NodeId, restartAfterDontSendEmail.NodeId);
            var restart = endingSequenceNodes.CreateRestart(intentId, accountId, "Terminate");
            var showYourFileAttachmentIfOneExists = endingSequenceNodes.CreateShowResponseFileAsset(intentId, accountId, restart.NodeId);
            var emailSendWasSuccessful = endingSequenceNodes.CreateEmailSendWasSuccessful(intentId, accountId, showYourFileAttachmentIfOneExists.NodeId);

            var retrySendEmailSecondAttempt = endingSequenceNodes.CreateRetrySendEmailSecondAttempt(intentId, accountId, "Placeholder");
            var emailSendFailedFirstAttempt = endingSequenceNodes.CreateEmailSendFailedFirstAttempt(intentId, accountId, retrySendEmailSecondAttempt.NodeId);
            var fallbackRetrySendEmailSecondAttempt = endingSequenceNodes.CreateFallbackRetrySendEmailSecondAttempt(intentId, accountId, "Placeholder");
            var fallbackEmailSendFailedFirstAttempt = endingSequenceNodes.CreateFallbackEmailSendFailedFirstAttempt(intentId, accountId, fallbackRetrySendEmailSecondAttempt.NodeId);

            var sendFallbackEmail = endingSequenceNodes.CreateSendFallbackEmail(intentId, accountId, "Placeholder");
            var mayWeSendAnInformationalEmailForTooComplicated = endingSequenceNodes.CreateMayWeSendAnInformationalEmailForTooComplicated(intentId, accountId, sendFallbackEmail.NodeId, restartAfterDontSendEmail.NodeId);

            var genericTooComplicated = endingSequenceNodes.CreateGenericTooComplicated(intentId, accountId);


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
                    showYourFileAttachmentIfOneExists,
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