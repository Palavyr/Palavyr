using System.Collections.Generic;

namespace Palavyr.Domain.Configuration.Constant
{
    public abstract class DynamicType
    {
        public string TableType { get; set; }
        public string PrettyName { get; set; }
        public string RequiredNodeTypes { get; set; }
    }

    public static class DynamicTableTypes
    {
        public static DynamicType DefaultTable = new SelectOneFlat();
        public static SelectOneFlat CreateSelectOneFlat() => new SelectOneFlat();
        public static PercentOfThreshold CreatePercentOfThreshold() => new PercentOfThreshold();
        public static BasicThreshold CreateBasicThreshold() => new BasicThreshold();
        public static List<DynamicType> GetDynamicTableTypes()
        {
            return new List<DynamicType>
            {
                new SelectOneFlat(),
                new PercentOfThreshold(),
                new BasicThreshold()
            };
        }

        public class SelectOneFlat : DynamicType
        {
            public SelectOneFlat()
            {
                PrettyName = "Select One Flat";
                TableType = nameof(SelectOneFlat);
                RequiredNodeTypes = $"{nameof(SelectOneFlat)}";
            }
        }

        public class PercentOfThreshold : DynamicType
        {
            public PercentOfThreshold()
            {
                PrettyName = "Percent Of Threshold";
                TableType = nameof(PercentOfThreshold);
                RequiredNodeTypes = $"{nameof(PercentOfThreshold)}";
            }
        }

        public class BasicThreshold : DynamicType
        {
            public BasicThreshold()
            {
                PrettyName = "Basic Threshold";
                TableType = nameof(BasicThreshold);
                RequiredNodeTypes = $"{nameof(BasicThreshold)}";
            }
        }
        // We can define new DynamicTypes here
    }
}