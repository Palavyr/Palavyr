using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Services.Units
{
    public class QuantUnit
    {
        public string UnitGroup { get; set; } // length, area, weight, currency
        public string UnitPrettyName { get; set; } // m, ft, m^2...
        public UnitIds UnitId { get; set; }

        public static QuantUnit Create(string prettyName, string groupName, UnitIds unitId)
        {
            return new QuantUnit
            {
                UnitPrettyName = prettyName,
                UnitGroup = groupName,
                UnitId = unitId
            };
        }
    }
}