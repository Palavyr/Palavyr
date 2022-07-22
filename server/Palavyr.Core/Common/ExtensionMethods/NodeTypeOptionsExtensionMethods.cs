using System.Collections.Generic;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Common.ExtensionMethods
{
    public static class NodeTypeOptionsExtensionMethods
    {
        public static List<NodeTypeOptionResource> AddAdditionalNode(
            this List<NodeTypeOptionResource> nodeTypeOptions,
            NodeTypeOptionResource additionalNode)
        {
            nodeTypeOptions.Add(additionalNode);
            return nodeTypeOptions;
        }

        public static List<NodeTypeOptionResource> AddAdditionalNodes(
            this List<NodeTypeOptionResource> nodeTypeOptions,
            List<NodeTypeOptionResource> additionalNodes)
        {
            nodeTypeOptions.AddRange(additionalNodes);
            return nodeTypeOptions;
        }

        public static string MakeUniqueIdentifier(this PricingStrategyTableMeta pricingStrategyTableMeta)
        {
            return TreeUtils.TransformRequiredNodeType(pricingStrategyTableMeta);
        }

        public static string MakeUniqueIdentifier(this PricingStrategyTableMeta pricingStrategyTableMeta, string prefix)
        {
            return TreeUtils.TransformRequiredNodeType(pricingStrategyTableMeta, prefix);
        }

        public static string ConvertToPrettyName(this PricingStrategyTableMeta pricingStrategyTableMeta)
        {
            return TreeUtils.TransformRequiredNodeTypeToPrettyName(pricingStrategyTableMeta);
        }

        public static string ConvertToPrettyName(this PricingStrategyTableMeta pricingStrategyTableMeta, string extraName)
        {
            return TreeUtils.TransformRequiredNodeTypeToPrettyName(pricingStrategyTableMeta, extraName);
        }

        public static string JoinValueOptionsOnDelimiter(this List<string> valueOptions)
        {
            return string.Join(Delimiters.ValueOptionDelimiter, valueOptions);
        }
    }
}