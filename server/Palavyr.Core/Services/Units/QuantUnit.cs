namespace Palavyr.Core.Services.Units
{
    public class QuantUnit
    {
        public string UnitType { get; set; } // length, area, weight, currency
        public string UnitId { get; set; } // m, ft, m^2...

        public static QuantUnit Create(string type, string id)
        {
            return new QuantUnit
            {
                UnitId = id,
                UnitType = type
            };
        }
    }
}