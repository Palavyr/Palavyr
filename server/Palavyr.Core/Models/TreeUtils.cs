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

        public static string TransformRequiredNodeType(DynamicTableMeta dynamicTableMeta, string prefix)
        {
            return string.Join(Separator, new[] {prefix, dynamicTableMeta.TableType, dynamicTableMeta.TableId});
        }

        public static string TransformRequiredNodeType(DynamicTableMeta dynamicTableMeta, string prefix, string suffix)
        {
            return string.Join(Separator, new[] {prefix, dynamicTableMeta.TableType, dynamicTableMeta.TableId, suffix});
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

        public static string CreateNodeChildrenString(params string[] nodeIds)
        {
            return string.Join(Delimiters.NodeChildrenStringDelimiter, nodeIds);
        }

        public static string JoinValueOptionsOnDelimiter(params string[] options)
        {
            return string.Join(Delimiters.PathOptionDelimiter, options);
        }
    }
}