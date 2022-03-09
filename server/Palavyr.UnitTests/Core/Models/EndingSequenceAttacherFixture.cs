using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Shouldly;
using Test.Common;
using Test.Common.Random;
using Xunit;

namespace PalavyrServer.UnitTests.Core.Models
{
    public class EndingSequenceAttacherFixture : IUnitTestFixture, IAsyncLifetime
    {
        private IEndingSequenceAttacher sequenceAttacher = null!;

        [Fact]
        public async Task AllNodesAreCreated()
        {
            var result = sequenceAttacher.AttachEndingSequenceToNodeList(new List<ConversationNode>() { }, A.RandomId(), A.RandomAccount());
            result.Count.ShouldBe(12);
        }


        [Fact]
        public async Task NodesAreReturnedInTheCorrectOrder()
        {
            var nodeList = sequenceAttacher.AttachEndingSequenceToNodeList(new List<ConversationNode>() { }, A.RandomId(), A.RandomAccount());

            var result = nodeList.Select(x => x.NodeId).ToList();

            var expected = new List<string>()
            {
                "6", "1", "A", "0", "EmailSuccessfulNodeId", "Restart", "EmailFailedNodeId", "2", "FallbackEmailFailedNodeId", "3", "5", "4"
            };

            result.ShouldBeEquivalentTo(expected);
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
            var guidSub = Substitute.For<IGuidUtils>();
            var range = Enumerable.Range(0, 20).Select(x => x.ToString()).ToArray();
            guidSub.CreateNewId().Returns("A", range);

            var endingSequenceNodes = new EndingSequenceNodes(guidSub);
            sequenceAttacher = new EndingSequenceAttacher(endingSequenceNodes);
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }
    }
}