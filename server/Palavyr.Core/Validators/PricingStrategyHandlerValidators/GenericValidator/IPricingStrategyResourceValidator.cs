using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.Core.Validators.PricingStrategyHandlerValidators.GenericValidator
{
    public interface IPricingStrategyResourceValidator<TR>
        where TR : IPricingStrategyTableRowResource
    {
        Task Validate(List<TR> pricingStrategyRequest);  
    }
}

