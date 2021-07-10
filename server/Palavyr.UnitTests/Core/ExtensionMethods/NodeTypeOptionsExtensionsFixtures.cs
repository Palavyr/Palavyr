using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Constant;
using Shouldly;
using Xunit;

namespace PalavyrServer.UnitTests.Core.ExtensionMethods
{
    [Trait("Extension Methods ", "Node Type Options")]
    public class NodeTypeOptionsExtensionsFixtures : IAsyncLifetime
    {
        public List<NodeTypeOption> NodeList { get; set; }


        [Fact]
        public void AddAdditionalNode_addsNode()
        {
            var newNode = NodeTypeOption.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.IV);
            NodeList.AddAdditionalNode(newNode);
            NodeList.ShouldContain(newNode);
            NodeList.Count.ShouldBe(3);
        }

        [Fact]
        public void AddAdditionalNode_addsNodes()
        {
            var node1 = NodeTypeOption.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.III);
            var node2 = NodeTypeOption.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.III);

            var newNodes = new List<NodeTypeOption>()
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
            NodeList = new List<NodeTypeOption>()
            {
                NodeTypeOption.Create(
                    "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue, NodeTypeCode.III),
                NodeTypeOption.Create(
                    "three", "four", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
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