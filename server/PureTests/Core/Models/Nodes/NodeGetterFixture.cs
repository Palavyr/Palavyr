using System.Threading.Tasks;
using Palavyr.Core.Models.Nodes;
using Shouldly;
using Test.Common;
using Test.Common.Builders.Conversations;
using Test.Common.Builders.Conversations.NodeAdders;
using Test.Common.ExtensionsMethods;
using Xunit;

namespace Pure.Core.Models.Nodes
{
    [Trait("Node Getter", "Conversations")]
    public class NodeGetterFixture : IUnitTestFixture, IAsyncLifetime
    {
        private INodeGetter nodeGetter;

        public Task InitializeAsync()
        {
            nodeGetter = this.GetNodeGetter();
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public void WhenGettingAParentNode_AndTheCurrentNodeHasASingleParent_ThatParentIsReturned()
        {
            var convo = this.CreateConversationBuilder();
            var root = this.CreateConversationNodeBuilder().CreateRootNode();
            var middleNode = convo.WithNode(root.AddProvideInfo().WithText("Some Text"));
            var endNode = convo.WithNode(middleNode.AddProvideInfo().WithText("more text"));
            convo.WithNode(endNode);
            var conversation = convo.Build();

            var parent = nodeGetter.GetAnyParentNode(conversation, endNode);

            parent.NodeId.ShouldBe(middleNode.NodeId);
        }

        [Fact]
        public void WhenGettingAParentNode_AndTheCurrentNodeIsRoot_NullIsReturned()
        {
            var convo = this.CreateConversationBuilder();
            var root = this.CreateConversationNodeBuilder().CreateRootNode();
            convo.WithNode(root);
            var conversation = convo.Build();

            var result = nodeGetter.GetAnyParentNode(conversation, root);

            result.ShouldBeNull();
        }

        [Fact]
        public void WhenGettingAParentNode_AndTheCurrentNodeHasMultipleParents_TheFirstNodeInTheNodeListIsReturned()
        {
            var convo = this.CreateConversationBuilder();
            var root = this.CreateConversationNodeBuilder().CreateRootNode();
            var secondNode = convo.WithNode(root.AddProvideInfo().WithText("Some Text"));
            // var = convo.WithNode(secondNode.AddAnabranch());

            // var endNode = convo.WithNode(middleNode.AddProvideInfo().WithText("more text"));
            // convo.WithNode(endNode);
            var conversation = convo.Build();


            // var result = nodeGetter
        }
    }
}