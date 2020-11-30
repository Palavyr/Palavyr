using System.Collections.Generic;

namespace Server.Domain.Configuration.Constant
{
    public static class DynamicTableTypes
    {
        public static DynamicType DefaultTable = new SelectOneFlat();
        public static SelectOneFlat CreateSelectOneFlat() => new SelectOneFlat();

        public static List<DynamicType> GetDynamicTableTypes()
        {
            return new List<DynamicType>
            {
                new SelectOneFlat()
            };
        }

        public abstract class DynamicType
        {
            public string TableType { get; set; }
            public string PrettyName { get; set; }
        }

        public class SelectOneFlat : DynamicType
        {
            public SelectOneFlat()
            {
                PrettyName = "Select One Flat";
                TableType = nameof(SelectOneFlat);
            }
        }

        // We can define new DynamicTypes here
    }
}