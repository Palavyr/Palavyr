using System.Collections.Generic;
using System.Linq;
using Palavyr.Common.Utils;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.Domain
{
    public static class MissingNodeCalculator
    {
        public static string[] CalculateMissingNodes(string[] requiredDynamicNodeTypes, List<ConversationNode> conversationNodes, List<DynamicTableMeta> dynamicTableMetas, List<StaticTablesMeta> staticTablesMetas)
        {
            var allMissingNodeTypes = new List<string>();

            if (requiredDynamicNodeTypes.Length > 0)
            {
                var rawMissingDynamicNodeTypes = TreeUtils.GetMissingNodes(conversationNodes.ToArray(), requiredDynamicNodeTypes);
                var missingDynamicNodeTypes = dynamicTableMetas
                    .Where(x => rawMissingDynamicNodeTypes.Contains(TreeUtils.TransformRequiredNodeType(x)))
                    .Select(TreeUtils.TransformRequiredNodeTypeToPrettyName)
                    .ToList();

                allMissingNodeTypes.AddRange(missingDynamicNodeTypes);
            }

            var perIndividualRequiredStaticTables = staticTablesMetas
                .Select(p => p.PerPersonInputRequired)
                .Any(r => r);

            if (perIndividualRequiredStaticTables && !allMissingNodeTypes.Contains(DefaultNodeTypeOptions.TakeNumberIndividuals.StringName))
            {
                var perPersonNodeType = new[] {DefaultNodeTypeOptions.TakeNumberIndividuals.StringName};
                var missingOtherNodeTypes = TreeUtils.GetMissingNodes(conversationNodes.ToArray(), perPersonNodeType); //.SelectMany(x => StringUtils.SplitCamelCase(x)).ToArray();
                if (missingOtherNodeTypes.Length > 0)
                {
                    var asPretty = string.Join(" ", StringUtils.SplitCamelCaseAsString(missingOtherNodeTypes.First()));
                    allMissingNodeTypes.Add(asPretty);
                }
            }

            return allMissingNodeTypes.ToArray();
        }

        public static string[] GetRequiredNodes(Area area)
        {
            var allRequiredNodes = new List<string>();

            // dynamic node types are required
            var requiredDynamicNodeTypes = area
                .DynamicTableMetas
                .Select(TreeUtils.TransformRequiredNodeType)
                .ToList();

            // check static tables to see if even 1 'per individual' is set. If so, then check for this node type.
            var perIndividualRequired = area
                .StaticTablesMetas
                .Select(x => x.PerPersonInputRequired)
                .Any(p => p);

            allRequiredNodes.AddRange(requiredDynamicNodeTypes);
            if (perIndividualRequired && !allRequiredNodes.Contains(DefaultNodeTypeOptions.TakeNumberIndividuals.StringName))
            {
                allRequiredNodes.Add(DefaultNodeTypeOptions.TakeNumberIndividuals.StringName);
            }

            return allRequiredNodes.ToArray();
        }
    }
}