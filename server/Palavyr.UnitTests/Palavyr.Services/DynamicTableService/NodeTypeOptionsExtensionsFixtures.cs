using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Services.DynamicTableService;

namespace PalavyrServer.Tests.Palavyr.Services.DynamicTableService
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
                NodeTypeOption.Create("one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice),
                NodeTypeOption.Create("three", "four", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice)
            };
        }

        [Test]
        public void AddAdditionalNode_addsNode()
        {
            var newNode = NodeTypeOption.Create("one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice);
            NodeList.AddAdditionalNode(newNode);
            NodeList.Should().Contain(newNode);
            NodeList.Count.Should().Be(3);
        }

        [Test]
        public void AddAdditionalNode_addsNodes()
        {
            var node1 = NodeTypeOption.Create("one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice);
            var node2 = NodeTypeOption.Create("one", "two", new List<string>(), new List<string>(), true, true, true, NodeTypeOption.MultipleChoice);

            var newNodes = new List<NodeTypeOption>()
            {
                node1, node2
            };

            NodeList.AddAdditionalNodes(newNodes);

            NodeList.Count.Should().Be(4);
            NodeList.Should().Contain(node1);
            NodeList.Should().Contain(node2);
        }
    }
}