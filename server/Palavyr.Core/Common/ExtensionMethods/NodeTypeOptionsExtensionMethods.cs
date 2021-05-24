using System.Collections.Generic;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class NodeTypeOptionsExtensionMethods
    {
        public static List<NodeTypeOption> AddAdditionalNode(
            this List<NodeTypeOption> nodeTypeOptions,
            NodeTypeOption additionalNode)
        {
            nodeTypeOptions.Add(additionalNode);
            return nodeTypeOptions;
        }

        public static List<NodeTypeOption> AddAdditionalNodes(
            this List<NodeTypeOption> nodeTypeOptions,
            List<NodeTypeOption> additionalNodes)
        {
            nodeTypeOptions.AddRange(additionalNodes);
            return nodeTypeOptions;
        }

        public static string MakeUniqueIdentifier(this DynamicTableMeta dynamicTableMeta)
        {
            return TreeUtils.TransformRequiredNodeType(dynamicTableMeta);
        }

        public static string MakeUniqueIdentifier(this DynamicTableMeta dynamicTableMeta, string prefix)
        {
            return TreeUtils.TransformRequiredNodeType(dynamicTableMeta, prefix);
        }

        public static string ConvertToPrettyName(this DynamicTableMeta dynamicTableMeta)
        {
            return TreeUtils.TransformRequiredNodeTypeToPrettyName(dynamicTableMeta);
        }

        public static string ConvertToPrettyName(this DynamicTableMeta dynamicTableMeta, string extraName)
        {
            return TreeUtils.TransformRequiredNodeTypeToPrettyName(dynamicTableMeta, extraName);
        }

        public static string JoinValueOptionsOnDelimiter(this List<string> valueOptions)
        {
            return string.Join(Delimiters.ValueOptionDelimiter, valueOptions);
        }
    }
}