using System.Collections.Generic;

namespace Server.Domain
{
    public static class DynamicTableTypes
    {
        public const string DefaultTable = SelectOneFlat;
        public const string DefaultPrettyName = SelectOneFlatPretty;
        
        public const string SelectOneFlat = "SelectOneFlat";
        public const string SelectOneFlatPretty = "Select One Flat";
        
        public const string None = "None";
        
        public static List<string> GetAvailableTableTypes()
        {
            return new List<string>()
            {
                SelectOneFlat
            };
        }

        public static List<string> GetAvailableTablePrettyNames()
        {
            return new List<string>()
            {
                SelectOneFlatPretty
            };
        }
    }
}