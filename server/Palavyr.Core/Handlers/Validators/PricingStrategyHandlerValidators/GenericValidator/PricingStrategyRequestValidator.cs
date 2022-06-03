using FluentValidation;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;

namespace Palavyr.Core.Handlers.Validators.PricingStrategyHandlerValidators.GenericValidator
{
    public class PricingStrategyRequestValidator<TEntity, TResource, TCompiler> : AbstractValidator<SavePricingStrategyTableRequest<TEntity, TResource, TCompiler>>
        where TEntity : class, IPricingStrategyTable<TEntity>, IEntity, new()
        where TResource : IPricingStrategyTableRowResource, new()
        where TCompiler : IPricingStrategyTableCompiler
    {
        public PricingStrategyRequestValidator(IPricingStrategyResourceValidator<TResource> pricingStrategyValidator)
        {
            RuleFor(c => c.IntentId).NotEmpty().NotNull().WithMessage("The IntentId must be provided.");
            RuleFor(c => c.TableId).NotEmpty().NotNull().WithMessage("The Table Id is required");
            RuleFor(c => c.PricingStrategyTableResource.TableTag).NotEmpty().NotNull().WithMessage("The Table Tag is required.")
            RuleFor(c => c.PricingStrategyTableResource.TableData).NotEmpty().NotNull();
            RuleFor(c => c).SetValidator(pricingStrategyValidator);

        }
    }

    public class TableValidator
    {
        public TableValidator()
        {
            
        }
        
    }
    
    
    
}