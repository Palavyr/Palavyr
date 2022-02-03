using System;
using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Services.Units
{
    public interface IUnitRetriever
    {
        List<string> GetUnitTypes();
        List<string> GetUnitIds();
        List<QuantUnit> GetUnitDefinitions();
        
        List<QuantUnit> GetUnitDefinitionsByType(string type);
        QuantUnit GetUnitDefinitionById(UnitIds id);
        public UnitIds ConvertToUnitId(string id);

    }

    public class UnitRetriever : IUnitRetriever
    {
        private readonly Models.Configuration.Schemas.Units units;

        public UnitRetriever(Models.Configuration.Schemas.Units units)
        {
            this.units = units;
        }

        public List<string> GetUnitTypes()
        {
            var unitTypes = units.UnitDefinitions.Select(x => x.UnitGroup).ToList();
            return unitTypes;
        }

        public List<string> GetUnitIds()
        {
            var unitIds = units.UnitDefinitions.Select(x => x.UnitPrettyName).ToList();
            return unitIds;
        }

        public List<QuantUnit> GetUnitDefinitions()
        {
            return units.UnitDefinitions;
        }

        public List<QuantUnit> GetUnitDefinitionsByType(string type)
        {
            var types = GetUnitTypes();
            if (!types.Contains(type))
            {
                throw new DomainException($"The type: {type} was not found in our supported definitions");
            }

            var definitions = units.UnitDefinitions.Where(x => x.UnitGroup == type).ToList();
            return definitions;
        }

        public QuantUnit GetUnitDefinitionById(UnitIds id)
        {
            var unitIdStringName = Enum.GetName(typeof(UnitIds), id);

            if (unitIdStringName == null) throw new DomainException("The unit Id provided is not supported");

            return GetUnitDefinitionById(unitIdStringName.ToLowerInvariant());
        }

        public QuantUnit GetUnitDefinitionById(string id)
        {
            var ids = GetUnitIds();
            if (!ids.Contains(id.ToString()))
            {
                throw new DomainException($"The id: {id} was not found in our supported definitions");
            }

            var definition = units.UnitDefinitions.Single(x => x.UnitPrettyName == id.ToString());
            return definition;
        }

        public UnitIds ConvertToUnitId(string id)
        {
            if (UnitIds.TryParse(id, out UnitIds unit))
            {
                return unit;
            }

            throw new DomainException("Could not parse the Unit Id provided");
        }
    }
}