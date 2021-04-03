using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Constant;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models
{
    public static class TreeUtils
    {
        public const string Separator = "-";
        
        public static string TransformRequiredNodeType(DynamicTableMeta dynamicTableMeta)
        {
            return string.Join(Separator, new[] {dynamicTableMeta.TableType, dynamicTableMeta.TableId});
        }

        public static string TransformRequiredNodeType(string tableType, string tableId)
        {
            return string.Join(Separator, new[] {tableType, tableId});
        }

        public static string TransformRequiredNodeType(DynamicTableMeta dynamicTableMeta, string extraName)
        {
            return string.Join(Separator, new[] {dynamicTableMeta.TableType, extraName, dynamicTableMeta.TableId});
        }

        public static string TransformRequiredNodeTypeToPrettyName(DynamicTableMeta dynamicTableMeta)
        {
            return string.Join(Separator, new[] {dynamicTableMeta.PrettyName, dynamicTableMeta.TableTag});
        }

        public static string TransformRequiredNodeTypeToPrettyName(string prettyName, string tableTag)
        {
            return string.Join(Separator, new[] {prettyName, tableTag});
        }

        public static string TransformRequiredNodeTypeToPrettyName(DynamicTableMeta dynamicTableMeta, string extraName)
        {
            return string.Join(Separator, new[] {dynamicTableMeta.PrettyName, extraName, dynamicTableMeta.TableTag});
        }
        
        public static int TraverseTheTreeFromTheTop(ConversationNode[] nodeList, ConversationNode node)
        {
            var count = 0;
            if (node.IsTerminalType)
            {
                return 1;
            }

            var children = node.NodeChildrenString.Split(",");
            foreach (var child in children)
            {
                var childNode = nodeList.SingleOrDefault(row => row.NodeId == child);
                if (childNode != null)
                {
                    count += TraverseTheTreeFromTheTop(nodeList, childNode);
                }
            }

            return count;
        }

        public static List<ConversationNode> TraverseTheTreeFromTheTopAsNodeArray(ConversationNode[] nodeList, ConversationNode node)
        {
            var foundNodes = new List<ConversationNode>();
            if (node.IsTerminalType)
            {
                return new List<ConversationNode>(){node};
            }

            var children = node.NodeChildrenString.Split(",");
            foreach (var child in children)
            {
                var childNode = nodeList.SingleOrDefault(row => row.NodeId == child);
                if (childNode != null)
                {
                    foundNodes.AddRange(TraverseTheTreeFromTheTopAsNodeArray(nodeList, childNode));
                }
            }

            return foundNodes;
        }
        
        public static int GetNumTerminal(ConversationNode[] nodeList)
        {
            return nodeList.Count(node => node.IsTerminalType);
        }

        public static string CreateNodeChildrenString(params string[] nodeIds)
        {
            return string.Join(Delimiters.NodeChildrenStringDelimiter, nodeIds);
        }

        public static string CreateValueOptions(params string[] options)
        {
            return string.Join(Delimiters.PathOptionDelimiter, options);
        }
    }
}