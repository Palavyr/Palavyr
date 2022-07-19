using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;

namespace Palavyr.Core.Services.Units
{
    public interface IUnitRetriever
    {
        List<string> GetUnitTypes();
        List<UnitIdEnum> GetUnitIds();
        List<QuantityUnitResource> GetUnitDefinitions();

        List<QuantityUnitResource> GetUnitDefinitionsByType(string type);
        QuantityUnitResource GetUnitDefinitionById(UnitIdEnum idEnum);
        public UnitIdEnum ConvertToUnitId(string id);
    }

    public class UnitRetriever : IUnitRetriever
    {
        private readonly Data.Entities.Units units;

        public UnitRetriever(Data.Entities.Units units)
        {
            this.units = units;
        }

        public List<string> GetUnitTypes()
        {
            var unitTypes = units.UnitDefinitions.Select(x => x.UnitGroup).ToList();
            return unitTypes;
        }

        public List<UnitIdEnum> GetUnitIds()
        {
            var unitIds = units.UnitDefinitions.Select(x => x.UnitIdEnum).ToList();
            return unitIds;
        }

        public List<QuantityUnitResource> GetUnitDefinitions()
        {
            return units.UnitDefinitions;
        }

        public List<QuantityUnitResource> GetUnitDefinitionsByType(string type)
        {
            var types = GetUnitTypes();
            if (!types.Contains(type))
            {
                throw new DomainException($"The type: {type} was not found in our supported definitions");
            }

            var definitions = units.UnitDefinitions.Where(x => x.UnitGroup == type).ToList();
            return definitions;
        }

        public QuantityUnitResource GetUnitDefinitionById(UnitIdEnum idEnum)
        {
            if (!Enum.IsDefined(typeof(UnitIdEnum), idEnum))
            {
                throw new DomainException("The unit Id provided is not supported");
            }

            var definition = units.UnitDefinitions.Single(x => x.UnitIdEnum == idEnum);
            return definition;
        }


        public UnitIdEnum ConvertToUnitId(string id)
        {
            if (UnitIdEnum.TryParse(id, out UnitIdEnum unit))
            {
                return unit;
            }

            throw new DomainException("Could not parse the Unit Id provided");
        }
    }
}