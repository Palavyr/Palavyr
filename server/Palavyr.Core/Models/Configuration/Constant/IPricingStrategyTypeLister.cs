using System.Collections.Generic;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Models.Configuration.Constant
{
    public interface IPricingStrategyTypeLister
    {
        IEnumerable<IHaveAPrettyNameAndTableType> ListPricingStrategies();
    }
}