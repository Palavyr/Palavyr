using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators.GenericValidator;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators
{
    
    public class BasicThresholdResourceValidator : IPricingStrategyResourceValidator<BasicThresholdResource>
    {
        public BasicThresholdResourceValidator()
        {
            
        }
        // public async Task Validate(List<BasicThresholdResource> pricingStrategyRequest)
        // {
        //     await Task.CompletedTask;
        //     throw new NotImplementedException();
        // }
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