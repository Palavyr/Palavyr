using System.Collections.Generic;

namespace Server.Domain.Configuration.Constant
{
    public abstract class DynamicType
    {
        public string TableType { get; set; }
        public string PrettyName { get; set; }
    }

    public static class DynamicTableTypes
    {
        public static DynamicType DefaultTable = new SelectOneFlat();
        public static SelectOneFlat CreateSelectOneFlat() => new SelectOneFlat();
        public static PercentOfThreshold CreatePercentOfThreshold() => new PercentOfThreshold();

        public static List<DynamicType> GetDynamicTableTypes()
        {
            return new List<DynamicType>
            {
                new SelectOneFlat(),
                new PercentOfThreshold()
            };
        }

        public class SelectOneFlat : DynamicType
        {
            public SelectOneFlat()
            {
                PrettyName = "Select One Flat";
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

        // We can define new DynamicTypes here
    }
}