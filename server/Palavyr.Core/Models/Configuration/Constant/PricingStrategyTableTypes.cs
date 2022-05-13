using System.Collections.Generic;
using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public abstract class PricingStrategyType
    {
        public readonly string tableType;

        public string TableType { get; set; }
        public string PrettyName { get; set; }

        public DynamicResponseParts CreateDynamicResponseParts(string nodeId, string responseValue)
        {
            return new DynamicResponseParts
            {
                new DynamicResponsePart() {{nodeId, responseValue}}
            };
        }
    }

    public static class PricingStrategyTableTypes
    {
        // TODO: Deprecate these. Probs don't need them.
        public static PricingStrategyType DefaultTable = new SelectOneFlat();
        public static SelectOneFlat CreateSelectOneFlat() => new SelectOneFlat();
        public static PercentOfThreshold CreatePercentOfThreshold() => new PercentOfThreshold();
        public static BasicThreshold CreateBasicThreshold() => new BasicThreshold();
        public static TwoNestedCategory CreateTwoNestedCategory() => new TwoNestedCategory();
        public static CategoryNestedThreshold CreateCategoryNestedThreshold() => new CategoryNestedThreshold();

        public static List<PricingStrategyType> GetDynamicTableTypes()
        {
            return new List<PricingStrategyType>
            {
                // TODO: List these using reflection checking for IDynamic Table implementations
                new SelectOneFlat(),
                new PercentOfThreshold(),
                new BasicThreshold(),
                new TwoNestedCategory(),
                new CategoryNestedThreshold()
            };
        }

        public class CategoryNestedThreshold : PricingStrategyType
        {
            public CategoryNestedThreshold()
            {
                PrettyName = "Category with Nested Threshold";
                TableType = nameof(CategoryNestedThreshold);
            }
        }

        // TODO: Define these next to the compilers
        public class TwoNestedCategory : PricingStrategyType
        {
            public TwoNestedCategory()
            {
                PrettyName = "Two Nested Categories";
                TableType = nameof(TwoNestedCategory);
            }
        }

        public class SelectOneFlat : PricingStrategyType
        {
            public SelectOneFlat()
            {
                PrettyName = "Simple Select One Option";
                TableType = nameof(SelectOneFlat);
            }
        }

        public class PercentOfThreshold : PricingStrategyType
        {
            public PercentOfThreshold()
            {
                PrettyName = "Percent Of Threshold";
                TableType = nameof(PercentOfThreshold);
            }
        }

        public class BasicThreshold : PricingStrategyType
        {
            public static readonly string tableType = nameof(BasicThreshold);

            public BasicThreshold()
            {
                PrettyName = "Simple Threshold";
                TableType = tableType;
            }

            // We can define new DynamicTypes here
        }
    }
}