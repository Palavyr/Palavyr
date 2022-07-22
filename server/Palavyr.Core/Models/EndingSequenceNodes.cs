using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Models
{
    public interface IEndingSequenceNodes
    {
        ConversationNode CreateMayWeSendAnEmail(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateSendEmail(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateDontSendEmailRestart(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateEmailSendWasSuccessful(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateRestart(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateEmailSendFailedFirstAttempt(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateRetrySendEmailSecondAttempt(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateFallbackEmailSendFailedFirstAttempt(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateFallbackRetrySendEmailSecondAttempt(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateMayWeSendAnInformationalEmailForTooComplicated(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateSendFallbackEmail(string intentId, string accountId, params string[] nodeChildrenIds);
        ConversationNode CreateGenericTooComplicated(string intentId, string accountId, params string[] nodeChildrenIds);

        ConversationNode CreateShowResponseFileAsset(string intentId, string accountId, params string[] nodeChildrenIds);
    }


    public class EndingSequenceNodes : IEndingSequenceNodes
    {
        private readonly IGuidUtils guidUtils;

        public EndingSequenceNodes(IGuidUtils guidUtils)
        {
            this.guidUtils = guidUtils;
        }

        public ConversationNode CreateMayWeSendAnEmail(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            // TODO: add text lookup from the database for the ending sequence table -- this could be a new tree builder hey
            var text = "We'd like to send you an email with some information regarding your enquiry. Would that be okay?";

            var nodeId = guidUtils.CreateNewId();
            return ConversationNode.CreateNew(
                nodeId,
                DefaultNodeTypeOptions.YesNo.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                TreeUtils.JoinValueOptionsOnDelimiter(DefaultNodeTypeOptions.YesNo.No, DefaultNodeTypeOptions.YesNo.Yes),
                accountId,
                DefaultNodeTypeOptions.YesNo.StringName,
                NodeTypeCodeEnum.V,
                false,
                false,
                true,
                false
            );
        }

        public ConversationNode CreateSendEmail(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();

            var text = "To receive a confirmation email please click on the button below.";

            return ConversationNode.CreateNew(
                nodeId,
                InternalNodeTypeOptions.SendEmail.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                DefaultNodeTypeOptions.YesNo.Yes,
                "",
                accountId,
                InternalNodeTypeOptions.SendEmail.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );
        }

        public ConversationNode CreateDontSendEmailRestart(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();

            var text = "Thanks for your time. If you'd like to start again, click the button below.";

            return ConversationNode.CreateNew(
                nodeId,
                InternalNodeTypeOptions.Restart.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                DefaultNodeTypeOptions.YesNo.No,
                "",
                accountId,
                InternalNodeTypeOptions.Restart.StringName,
                NodeTypeCodeEnum.IV,
                false,
                false,
                false,
                true
            );
        }

        public ConversationNode CreateEmailSendWasSuccessful(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = EndingSequenceAttacher.EmailSuccessfulNodeId;
            var text = "We've sent through an email to the address you provided. Sometimes these emails are picked up as spam, so if you don't see it, be sure to check your spam folder.";

            return ConversationNode.CreateNew(
                nodeId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );
        }

        public ConversationNode CreateRestart(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = InternalNodeTypeOptions.Restart.StringName;
            var text = "Thanks for your time. Feel free to restart the chat using the arrow icon below.";

            return ConversationNode.CreateNew(
                nodeId,
                InternalNodeTypeOptions.Restart.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                InternalNodeTypeOptions.Restart.StringName,
                NodeTypeCodeEnum.IV,
                false,
                false,
                false,
                true
            );
        }

        public ConversationNode CreateEmailSendFailedFirstAttempt(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            if (nodeChildrenIds.Length == 0) throw new DomainException($"{nameof(CreateEmailSendFailedFirstAttempt)} must have at least one child node.");
            var nodeId = EndingSequenceAttacher.EmailFailedNodeId;

            var text = "Hmm, we were not able to send an email to the address provided. Could you check that is correct?";

            return ConversationNode.CreateNew(
                nodeId,
                "EmailSendFailedFirstAttempt",
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );
        }

        public ConversationNode CreateRetrySendEmailSecondAttempt(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();
            return ConversationNode.CreateNew(
                nodeId,
                InternalNodeTypeOptions.SendEmail.StringName,
                "Wait just a moment while I try that again.",
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                InternalNodeTypeOptions.SendEmail.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );
        }

        public ConversationNode CreateFallbackEmailSendFailedFirstAttempt(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            if (nodeChildrenIds.Length == 0) throw new DomainException($"{nameof(CreateFallbackEmailSendFailedFirstAttempt)} must have at least one child node.");

            var nodeId = EndingSequenceAttacher.FallbackEmailFailedNodeId;
            var text = "Hmm, we were not able to send an email to the address provided. Could you check that is correct?";

            return ConversationNode.CreateNew(
                nodeId,
                "EmailSendFailedFirstAttempt",
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                DefaultNodeTypeOptions.ProvideInfo.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );
        }

        public ConversationNode CreateFallbackRetrySendEmailSecondAttempt(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();
            return ConversationNode.CreateNew(
                nodeId,
                InternalNodeTypeOptions.SendEmail.StringName,
                "Wait just a moment while I try that again.",
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                InternalNodeTypeOptions.SendEmail.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );
        }

        public ConversationNode CreateMayWeSendAnInformationalEmailForTooComplicated(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();
            var text = "We'd like to send you a follow-up email with some general information regarding your enquiry. Would that be okay?";

            return ConversationNode.CreateNew(
                nodeId,
                DefaultNodeTypeOptions.YesNo.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                TreeUtils.JoinValueOptionsOnDelimiter(DefaultNodeTypeOptions.YesNo.No, DefaultNodeTypeOptions.YesNo.Yes),
                accountId,
                DefaultNodeTypeOptions.YesNo.StringName,
                NodeTypeCodeEnum.V,
                false,
                false,
                true,
                false
            );
        }

        public ConversationNode CreateSendFallbackEmail(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();

            return ConversationNode.CreateNew( // leads to sending an email to the 'too complicated controller'
                nodeId,
                InternalNodeTypeOptions.SendTooComplicatedEmail.StringName,
                "Wait just a moment while I send an email.",
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                DefaultNodeTypeOptions.YesNo.Yes,
                "",
                accountId,
                InternalNodeTypeOptions.SendTooComplicatedEmail.StringName,
                NodeTypeCodeEnum.II,
                false,
                false,
                false,
                false
            );
        }

        public ConversationNode CreateGenericTooComplicated(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();

            var text = "We will need to discuss your situation in person.";
            return ConversationNode.CreateNew(
                nodeId,
                DefaultNodeTypeOptions.TooComplicated.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                DefaultNodeTypeOptions.TooComplicated.StringName,
                NodeTypeCodeEnum.I,
                false,
                false,
                false,
                true
            );
        }

        public ConversationNode CreateShowResponseFileAsset(string intentId, string accountId, params string[] nodeChildrenIds)
        {
            var nodeId = guidUtils.CreateNewId();
            var text = "You can view your pdf here.";

            return ConversationNode.CreateNew(
                nodeId,
                InternalNodeTypeOptions.ShowResponseFileAsset.StringName,
                text,
                intentId,
                nodeChildrenString: TreeUtils.CreateNodeChildrenString(nodeChildrenIds),
                "",
                "",
                accountId,
                InternalNodeTypeOptions.ShowResponseFileAsset.StringName,
                NodeTypeCodeEnum.IV,
                false,
                false,
                false,
                false,
                isImageNode: true,
                imageId: ""
            );
        }
    }
}