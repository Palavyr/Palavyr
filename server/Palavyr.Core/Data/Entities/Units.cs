#nullable disable

using System.Collections.Generic;
using Palavyr.Core.Services.Units;

namespace Palavyr.Core.Data.Entities
{
    public enum UnitIdEnum
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

        public List<QuantityUnitResource> UnitDefinitions { get; set; } = new List<QuantityUnitResource>
        {
            QuantityUnitResource.Create(Meter, Length, UnitIdEnum.Meter),
            QuantityUnitResource.Create(Foot, Length, UnitIdEnum.Foot),
            QuantityUnitResource.Create(SquareMeters, Area, UnitIdEnum.SquareMeters),
            QuantityUnitResource.Create(SquareFeet, Area, UnitIdEnum.SquareFeet),
            QuantityUnitResource.Create(Grams, Weight, UnitIdEnum.Grams),
            QuantityUnitResource.Create(KiloGrams, Weight, UnitIdEnum.KiloGrams),
            QuantityUnitResource.Create(Pounds, Weight, UnitIdEnum.Pounds),
            QuantityUnitResource.Create(Tons, Weight, UnitIdEnum.Tons),
            QuantityUnitResource.Create(Currency, Currency, UnitIdEnum.Currency)
        };
    }
}