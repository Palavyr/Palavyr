using System.Collections.Generic;
using Palavyr.Domain;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Services.DynamicTableService
{
    public static class NodeTypeOptionsExtensions
    {
        public static List<NodeTypeOption> AddAdditionalNode(this List<NodeTypeOption> nodeTypeOptions,
            NodeTypeOption additionalNode)
        {
            nodeTypeOptions.Add(additionalNode);
            return nodeTypeOptions;
        }

        public static List<NodeTypeOption> AddAdditionalNodes(this List<NodeTypeOption> nodeTypeOptions,
            List<NodeTypeOption> additionalNodes)
        {
            nodeTypeOptions.AddRange(additionalNodes);
            return nodeTypeOptions;
        }

        public static string MakeUniqueIdentifier(this DynamicTableMeta dynamicTableMeta)
        {
            return TreeUtils.TransformRequiredNodeType(dynamicTableMeta);
        }
    }
}