using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Services.Units
{
    public class QuantityUnitResource
    {
        public string UnitGroup { get; set; } // length, area, weight, currency
        public string UnitPrettyName { get; set; } // m, ft, m^2...
        public UnitIds UnitId { get; set; }

        public static QuantityUnitResource Create(string prettyName, string groupName, UnitIds unitId)
        {
            return new QuantityUnitResource
            {
                UnitPrettyName = prettyName,
                UnitGroup = groupName,
                UnitId = unitId
            };
        }
    }
}