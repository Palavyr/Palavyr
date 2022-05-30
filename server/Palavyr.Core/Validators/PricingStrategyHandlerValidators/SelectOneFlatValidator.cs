using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Validators.PricingStrategyHandlerValidators.GenericValidator;

namespace Palavyr.Core.Validators.PricingStrategyHandlerValidators
{
    
    public class BasicThresholdResourceValidator : IPricingStrategyResourceValidator<BasicThresholdResource>
    {
        public async Task Validate(List<BasicThresholdResource> pricingStrategyRequest)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
    
    
    public class CategoryNestedThresholdValidator : IPricingStrategyResourceValidator<CategoryNestedThresholdResource>
    {
        public async Task Validate(List<CategoryNestedThresholdResource> pricingStrategyRequest)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }

    public class SelectOneFlatResourceValidator : IPricingStrategyResourceValidator<SelectOneFlatRowResource>
    {
        public async Task Validate(List<SelectOneFlatRowResource> tableData)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }

    public class PercentOfThresholdResourceValidator : IPricingStrategyResourceValidator<PercentOfThresholdResource>
    {
        public async Task Validate(List<PercentOfThresholdResource> tableData)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }

    public class TwoNestedCategoryResourceValidator : IPricingStrategyResourceValidator<TwoNestedCategoryResource>
    {
        public async Task Validate(List<TwoNestedCategoryResource> pricingStrategyRequest)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}