﻿using System.Threading.Tasks;
using NSubstitute;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Repositories;
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
            var node = endingSequenceNodes.CreateDontSendEmailRestart(A.RandomId(), A.RandomAccount(), "Placeholder");

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
        public async Task CreatesEmailSendWasSuccessful()
        {
            var node = endingSequenceNodes.CreateEmailSendWasSuccessful(A.RandomId(), A.RandomAccount(), InternalNodeTypeOptions.Restart.StringName);

            node.NodeChildrenString.ShouldBe(InternalNodeTypeOptions.Restart.StringName);
            node.NodeType.ShouldBe(DefaultNodeTypeOptions.ProvideInfo.StringName);
            node.OptionPath.ShouldBeEmpty();
            node.ValueOptions.ShouldBeEmpty();
            node.NodeComponentType.ShouldBe(DefaultNodeTypeOptions.ProvideInfoWithPdfLink.StringName);
            node.IsRoot.ShouldBeFalse();
            node.IsCritical.ShouldBeFalse();
            node.IsMultiOptionType.ShouldBeFalse();
            node.IsTerminalType.ShouldBeFalse();
        }

        [Fact]
        public async Task CreatesRestart()
        {
            var node = endingSequenceNodes.CreateRestart(A.RandomId(), A.RandomAccount(), "Terminate");

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
            var node = endingSequenceNodes.CreateEmailSendFailedFirstAttempt(A.RandomId(), A.RandomAccount(), "id-1");

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
        public async Task CreateRetrySendEmailSecondAttempt()
        {
            var node = endingSequenceNodes.CreateRetrySendEmailSecondAttempt(A.RandomId(), A.RandomAccount(), "Placeholder");

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
        public async Task CreatesFallbackEmailSendFailedFirstAttempt()
        {
            var node = endingSequenceNodes.CreateFallbackEmailSendFailedFirstAttempt(A.RandomId(), A.RandomAccount(), "123", "456");

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
        public async Task CreatesFallbackEmailSendFailedSecondAttempt()
        {
            var node = endingSequenceNodes.CreateFallbackRetrySendEmailSecondAttempt(A.RandomId(), A.RandomAccount(), "Placeholder");

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
        public async Task CreatesMyWeSendAnInformationalEmailForTooComplicated()
        {
            var node = endingSequenceNodes.CreateMayWeSendAnInformationalEmailForTooComplicated(A.RandomId(), A.RandomAccount(), "abc", "def");

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
        public async Task CreatesSendFallbackEmail()
        {
            var node = endingSequenceNodes.CreateSendFallbackEmail(A.RandomId(), A.RandomAccount(), "Placeholder");

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
        public async Task CreatesGenericTooComplicated()
        {
            var node = endingSequenceNodes.CreateGenericTooComplicated(A.RandomId(), A.RandomAccount());

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
            var configurationRepository = Substitute.For<IConfigurationRepository>();
            endingSequenceNodes = new EndingSequenceNodes(configurationRepository, new GuidUtils());
        }


        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}