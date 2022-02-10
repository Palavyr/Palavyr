using System.Collections.Generic;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Models.Configuration.Schemas
{
    public enum UnitIds
    {
        Currency = 0,
        Meter = 1,
        Foot = 2,
        SquareMeters = 3,
        SquareFeet = 4,
        Grams = 5,
        KiloGrams = 6,
        Pounds = 7,
        Tons = 8
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

        public List<QuantUnit> UnitDefinitions { get; set; } = new List<QuantUnit>
        {
            QuantUnit.Create(Meter, Length, UnitIds.Meter),
            QuantUnit.Create(Foot, Length, UnitIds.Foot),
            QuantUnit.Create(SquareMeters, Area, UnitIds.SquareMeters),
            QuantUnit.Create(SquareFeet, Area, UnitIds.SquareFeet),
            QuantUnit.Create(Grams, Weight, UnitIds.Grams),
            QuantUnit.Create(KiloGrams, Weight, UnitIds.KiloGrams),
            QuantUnit.Create(Pounds, Weight, UnitIds.Pounds),
            QuantUnit.Create(Tons, Weight, UnitIds.Tons),
            QuantUnit.Create(Currency, Currency, UnitIds.Currency)
        };
    }
}