using System.Collections.Generic;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public enum UnitIds
    {
        Meter = 1,
        Foot = 2,
        SquareMeters = 3,
        SquareFeet = 4,
        Grams = 5,
        KiloGrams = 6,
        Pounds = 7,
        Tons = 8,
        Currency = 9
    }

    public class Units
    {
        public const string Length = "length";
        public const string Area = "area";
        public const string Weight = "weight";
        public const string Currency = "currency";

        
        public const string Meter = "m";
        public const string Foot = "ft";
        public const string SquareMeters = "m^2";
        public const string SquareFeet = "f^2";
        public const string Grams = "g";
        public const string KiloGrams = "kg";
        public const string Pounds = "lbs";
        public const string Tons = "tons";

        public List<QuantUnit> UnitDefinitions { get; set; } = new List<QuantUnit>()
        {
            QuantUnit.Create(Length, Meter),
            QuantUnit.Create(Length, Foot),
            QuantUnit.Create(Area, SquareMeters),
            QuantUnit.Create(Area, SquareFeet),
            QuantUnit.Create(Weight, Grams),
            QuantUnit.Create(Weight, KiloGrams),
            QuantUnit.Create(Weight, Pounds),
            QuantUnit.Create(Weight, Tons),
            QuantUnit.Create(Currency, Currency)
        };
    }
}