using System.Collections.Generic;
using NUnit.Framework;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Models.Configuration.Constant;
using Shouldly;

namespace PalavyrServer.UnitTests.Core.ExtensionMethods
{
    [TestFixture]
    public class NodeTypeOptionsExtensionsFixtures
    {
        public List<NodeTypeOption> NodeList { get; set; }

        [SetUp]
        public void Setup()
        {
            NodeList = new List<NodeTypeOption>()
            {
                NodeTypeOption.Create(
                    "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue),
                NodeTypeOption.Create(
                    "three", "four", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                    DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue)
            };
        }

        [Test]
        public void AddAdditionalNode_addsNode()
        {
            var newNode = NodeTypeOption.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue);
            NodeList.AddAdditionalNode(newNode);
            NodeList.ShouldContain(newNode);
            NodeList.Count.ShouldBe(3);
        }

        [Test]
        public void AddAdditionalNode_addsNodes()
        {
            var node1 = NodeTypeOption.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue);
            var node2 = NodeTypeOption.Create(
                "one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice,
                DefaultNodeTypeOptions.NodeComponentTypes.MultipleChoiceContinue);

            var newNodes = new List<NodeTypeOption>()
            {
                node1, node2
            };

            NodeList.AddAdditionalNodes(newNodes);

            NodeList.Count.ShouldBe(4);
            NodeList.ShouldContain(node1);
            NodeList.ShouldContain(node2);
        }
    }
}