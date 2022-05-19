using System.Collections.Generic;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Services.PricingStrategyTableServices
{
    public class PricingStrategyTableDataResource<TResource> where TResource : IPricingStrategyTableRowResource
    {
        public IEnumerable<TResource> TableRows { get; set; }
        public bool IsInUse { get; set; }
    }
}