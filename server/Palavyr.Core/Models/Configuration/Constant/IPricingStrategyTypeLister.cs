using System.Collections.Generic;
using Palavyr.Core.Models.Configuration.Schemas;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public interface IPricingStrategyTypeLister
    {
        IEnumerable<IHaveAPrettyNameAndTableType> ListPricingStrategies();
    }
}