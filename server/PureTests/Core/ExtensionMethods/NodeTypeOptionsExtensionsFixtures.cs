using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Constant;
using Shouldly;
using Xunit;

namespace Pure.Core.ExtensionMethods
{
    [Trait("Extension Methods ", "Node Type Options")]
    public class NodeTypeOptionsExtensionsFixtures : IAsyncLifetime
    {
        public List<NodeTypeOptionResource> NodeList { get; set; }


        [Fact]
        public void AddAdditionalNode_addsNode()
        {
            var newNode = NodeTypeOptionResource.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOptionResource.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.IV);
            NodeList.AddAdditionalNode(newNode);
            NodeList.ShouldContain(newNode);
            NodeList.Count.ShouldBe(3);
        }

        [Fact]
        public void AddAdditionalNode_addsNodes()
        {
            var node1 = NodeTypeOptionResource.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOptionResource.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.III);
            var node2 = NodeTypeOptionResource.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOptionResource.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.III);

            var newNodes = new List<NodeTypeOptionResource>()
            {
                node1, node2
            };

            NodeList.AddAdditionalNodes(newNodes);

            NodeList.Count.ShouldBe(4);
            NodeList.ShouldContain(node1);
            NodeList.ShouldContain(node2);
        }

        public Task InitializeAsync()
        {
            NodeList = new List<NodeTypeOptionResource>()
            {
                NodeTypeOptionResource.Create(
                    "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOptionResource.MultipleChoice,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.III),
                NodeTypeOptionResource.Create(
                    "three", "four", new List<string>(), new List<string>(), true, true, true, NodeTypeOptionResource.MultipleChoice,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.III)
            };
            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}