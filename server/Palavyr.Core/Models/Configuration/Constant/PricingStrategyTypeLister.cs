using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public class PricingStrategyTypeLister : IPricingStrategyTypeLister
    {
        public IEnumerable<IHaveAPrettyNameAndTableType> ListPricingStrategies()
        {
            var types = typeof(SelectOneFlat)
                .Assembly
                .GetTypes()
                .Where(
                    x => x
                        .GetInterfaces()
                        .Contains(typeof(IPricingStrategyTable<>)))
                .Cast<IHaveAPrettyNameAndTableType>();
            return types;
        }
    }
}