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
        List<QuantUnit> GetUnitDefinitionsByType(string type);
        QuantUnit GetUnitDefinitionById(UnitIds id);
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
            var unitTypes = units.UnitDefinitions.Select(x => x.UnitType).ToList();
            return unitTypes;
        }

        public List<string> GetUnitIds()
        {
            var unitIds = units.UnitDefinitions.Select(x => x.UnitType).ToList();
            return unitIds;
        }

        public List<QuantUnit> GetUnitDefinitionsByType(string type)
        {
            var types = GetUnitTypes();
            if (!types.Contains(type))
            {
                throw new DomainException($"The type: {type} was not found in our supported definitions");
            }

            var definitions = units.UnitDefinitions.Where(x => x.UnitType == type).ToList();
            return definitions;
        }

        public QuantUnit GetUnitDefinitionById(UnitIds id)
        {
            var ids = GetUnitIds();
            if (!ids.Contains(id.ToString()))
            {
                throw new DomainException($"The id: {id} was not found in our supported definitions");
            }

            var definition = units.UnitDefinitions.Single(x => x.UnitId == id.ToString());
            return definition;
        }
    }
}