using System.Collections.Generic;
using System.Linq;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.PricingStrategyTables;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public class PricingStrategyTypeLister : IPricingStrategyTypeLister
    {
        public IEnumerable<IHaveAPrettyNameAndTableType> ListPricingStrategies()
        {
            var types = typeof(CategorySelectTableRow)
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