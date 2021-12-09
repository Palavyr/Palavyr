using System.Collections.Generic;
using Palavyr.Core.Models.Aliases;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public abstract class DynamicType
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

    public static class DynamicTableTypes
    {
        // TODO: Deprecate these. Probs don't need them.
        public static DynamicType DefaultTable = new SelectOneFlat();
        public static SelectOneFlat CreateSelectOneFlat() => new SelectOneFlat();
        public static PercentOfThreshold CreatePercentOfThreshold() => new PercentOfThreshold();
        public static BasicThreshold CreateBasicThreshold() => new BasicThreshold();
        public static TwoNestedCategory CreateTwoNestedCategory() => new TwoNestedCategory();
        public static CategoryNestedThreshold CreateCategoryNestedThreshold() => new CategoryNestedThreshold();

        public static List<DynamicType> GetDynamicTableTypes()
        {
            return new List<DynamicType>
            {
                // TODO: List these using reflection checking for IDynamic Table implementations
                new SelectOneFlat(),
                new PercentOfThreshold(),
                new BasicThreshold(),
                new TwoNestedCategory(),
                new CategoryNestedThreshold()
            };
        }

        public class CategoryNestedThreshold : DynamicType
        {
            public CategoryNestedThreshold()
            {
                PrettyName = "Category with Nested Threshold";
                TableType = nameof(CategoryNestedThreshold);
            }
        }

        // TODO: Define these next to the compilers
        public class TwoNestedCategory : DynamicType
        {
            public TwoNestedCategory()
            {
                PrettyName = "Two Nested Categories";
                TableType = nameof(TwoNestedCategory);
            }
        }

        public class SelectOneFlat : DynamicType
        {
            public SelectOneFlat()
            {
                PrettyName = "Simple Select One Option";
                TableType = nameof(SelectOneFlat);
            }
        }

        public class PercentOfThreshold : DynamicType
        {
            public PercentOfThreshold()
            {
                PrettyName = "Percent Of Threshold";
                TableType = nameof(PercentOfThreshold);
            }
        }

        public class BasicThreshold : DynamicType
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