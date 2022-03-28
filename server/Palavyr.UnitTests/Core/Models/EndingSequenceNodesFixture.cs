using System.Threading.Tasks;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Shouldly;
using Test.Common;
using Test.Common.Random;
using Xunit;

namespace PalavyrServer.UnitTests.Core.Models
{
    public class EndingSequenceNodesFixture : IUnitTestFixture, IAsyncLifetime
    {
        public IEndingSequenceNodes endingSequenceNodes = null!;


        [Fact]
        public void CreatesMayWeSendAnEmail()
        {
            // arrange & act
            var node = endingSequenceNodes.CreateMayWeSendAnEmail("456", "account-123", "id-1", "id-2", "id-3");

            // assert
            node.NodeChildrenString.ShouldBe(TreeUtils.CreateNodeChildrenString("id-1", "id-2", "id-3"));
            node.NodeTypeCode.ShouldBe(NodeTypeCode.V);
            node.NodeType.ShouldBe(DefaultNodeTypeOptions.YesNo.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeTrue();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesSendEmail()
        {
            var node = endingSequenceNodes.CreateSendEmail("123", "456");

            node.NodeType.ShouldBe(InternalNodeTypeOptions.SendEmail.StringName);
            node.OptionPath.ShouldBe(DefaultNodeTypeOptions.YesNo.Yes);
            node.NodeComponentType.ShouldBe(InternalNodeTypeOptions.SendEmail.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesDontSendEmailRestart()
        {
            var node = endingSequenceNodes.CreateDontSendEmailRestart(A.RandomId(), A.RandomAccountId(), "Placeholder");

            node.NodeChildrenString.ShouldBe("Placeholder");
            node.NodeType.ShouldBe(InternalNodeTypeOptions.Restart.StringName);
            node.OptionPath.ShouldBe(DefaultNodeTypeOptions.YesNo.No);
            node.ValueOptions.ShouldBe("");
            node.NodeComponentType.ShouldBe(InternalNodeTypeOptions.Restart.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeTrue();
        }

        [Fact]
        public void CreatesEmailSendWasSuccessful()
        {
            var node = endingSequenceNodes.CreateEmailSendWasSuccessful(A.RandomId(), A.RandomAccountId(), InternalNodeTypeOptions.Restart.StringName);

            node.NodeChildrenString.ShouldBe(InternalNodeTypeOptions.Restart.StringName);
            node.NodeType.ShouldBe(DefaultNodeTypeOptions.ProvideInfo.StringName);
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(DefaultNodeTypeOptions.ProvideInfo.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesRestart()
        {
            var node = endingSequenceNodes.CreateRestart(A.RandomId(), A.RandomAccountId(), "Terminate");

            node.NodeType.ShouldBe(InternalNodeTypeOptions.Restart.StringName);
            node.NodeChildrenString.ShouldBe("Terminate");
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(InternalNodeTypeOptions.Restart.StringName);

            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeTrue();
        }

        [Fact]
        public async Task CreatesEmailSendFailedFirstAttempt()
        {
            var node = endingSequenceNodes.CreateEmailSendFailedFirstAttempt(A.RandomId(), A.RandomAccountId(), "id-1");

            node.NodeType.ShouldBe("EmailSendFailedFirstAttempt");
            node.NodeChildrenString.ShouldBe("id-1");
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(DefaultNodeTypeOptions.ProvideInfo.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreateRetrySendEmailSecondAttempt()
        {
            var node = endingSequenceNodes.CreateRetrySendEmailSecondAttempt(A.RandomId(), A.RandomAccountId(), "Placeholder");

            node.NodeType.ShouldBe(InternalNodeTypeOptions.SendEmail.StringName);
            node.NodeChildrenString.ShouldBe("Placeholder");
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(InternalNodeTypeOptions.SendEmail.StringName);

            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesFallbackEmailSendFailedFirstAttempt()
        {
            var node = endingSequenceNodes.CreateFallbackEmailSendFailedFirstAttempt(A.RandomId(), A.RandomAccountId(), "123", "456");

            node.NodeType.ShouldBe("EmailSendFailedFirstAttempt");
            node.NodeChildrenString.ShouldBe("123,456");
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(DefaultNodeTypeOptions.ProvideInfo.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesFallbackEmailSendFailedSecondAttempt()
        {
            var node = endingSequenceNodes.CreateFallbackRetrySendEmailSecondAttempt(A.RandomId(), A.RandomAccountId(), "Placeholder");

            node.NodeType.ShouldBe(InternalNodeTypeOptions.SendEmail.StringName);
            node.NodeChildrenString.ShouldBe("Placeholder");
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(InternalNodeTypeOptions.SendEmail.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesMyWeSendAnInformationalEmailForTooComplicated()
        {
            var node = endingSequenceNodes.CreateMayWeSendAnInformationalEmailForTooComplicated(A.RandomId(), A.RandomAccountId(), "abc", "def");

            node.NodeType.ShouldBe(DefaultNodeTypeOptions.YesNo.StringName);
            node.NodeChildrenString.ShouldBe("abc,def");
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBe(TreeUtils.JoinValueOptionsOnDelimiter(DefaultNodeTypeOptions.YesNo.No, DefaultNodeTypeOptions.YesNo.Yes));
            node.NodeComponentType.ShouldBe(DefaultNodeTypeOptions.YesNo.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeTrue();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesSendFallbackEmail()
        {
            var node = endingSequenceNodes.CreateSendFallbackEmail(A.RandomId(), A.RandomAccountId(), "Placeholder");

            node.NodeType.ShouldBe(InternalNodeTypeOptions.SendTooComplicatedEmail.StringName);
            node.NodeChildrenString.ShouldBe("Placeholder");
            node.OptionPath.ShouldBe(DefaultNodeTypeOptions.YesNo.Yes);
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(InternalNodeTypeOptions.SendTooComplicatedEmail.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public void CreatesGenericTooComplicated()
        {
            var node = endingSequenceNodes.CreateGenericTooComplicated(A.RandomId(), A.RandomAccountId());

            node.NodeType.ShouldBe(DefaultNodeTypeOptions.TooComplicated.StringName);
            node.NodeChildrenString.ShouldBeEmpty();
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(DefaultNodeTypeOptions.TooComplicated.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeTrue();
        }


        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
            endingSequenceNodes = new EndingSequenceNodes(new GuidUtils());
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}