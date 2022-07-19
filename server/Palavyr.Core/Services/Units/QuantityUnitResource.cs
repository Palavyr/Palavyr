using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Services.Units
{
    public class QuantityUnitResource
    {
        public string UnitGroup { get; set; } // length, area, weight, currency
        public string UnitPrettyName { get; set; } // m, ft, m^2...
        public UnitIdEnum UnitIdEnum { get; set; }

        public static QuantityUnitResource Create(string prettyName, string groupName, UnitIdEnum unitIdEnum)
        {
            return new QuantityUnitResource
            {
                UnitPrettyName = prettyName,
                UnitGroup = groupName,
                UnitIdEnum = unitIdEnum
            };
        }
    }
}