using Palavyr.Core.Data.Entities;
using Palavyr.Core.Models.Configuration.Constant;

namespace Palavyr.Core.Models
{
    public static class TreeUtils
    {
        public const string Separator = "-";

        public static string TransformRequiredNodeType(PricingStrategyTableMeta pricingStrategyTableMeta)
        {
            return string.Join(Separator, new[] {pricingStrategyTableMeta.TableType, pricingStrategyTableMeta.TableId});
        }

        public static string TransformRequiredNodeType(string tableType, string tableId)
        {
            return string.Join(Separator, new[] {tableType, tableId});
        }

        public static string TransformRequiredNodeType(PricingStrategyTableMeta pricingStrategyTableMeta, string prefix)
        {
            return string.Join(Separator, new[] {prefix, pricingStrategyTableMeta.TableType, pricingStrategyTableMeta.TableId});
        }

        public static string TransformRequiredNodeType(PricingStrategyTableMeta pricingStrategyTableMeta, string prefix, string suffix)
        {
            return string.Join(Separator, new[] {prefix, pricingStrategyTableMeta.TableType, pricingStrategyTableMeta.TableId, suffix});
        }

        public static string TransformRequiredNodeTypeToPrettyName(PricingStrategyTableMeta pricingStrategyTableMeta)
        {
            return string.Join(Separator, new[] {pricingStrategyTableMeta.PrettyName, pricingStrategyTableMeta.TableTag});
        }

        public static string TransformRequiredNodeTypeToPrettyName(string prettyName, string tableTag)
        {
            return string.Join(Separator, new[] {prettyName, tableTag});
        }

        public static string TransformRequiredNodeTypeToPrettyName(PricingStrategyTableMeta pricingStrategyTableMeta, string extraName)
        {
            return string.Join(Separator, new[] {pricingStrategyTableMeta.PrettyName, extraName, pricingStrategyTableMeta.TableTag});
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