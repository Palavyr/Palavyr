using System.Threading.Tasks;
using Palavyr.Core.Handlers.PricingStrategyHandlers;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices;
using Shouldly;

namespace Palavyr.Core.Validators.PricingStrategyHandlerValidators.GenericValidator
{
    public class PricingStrategyRequestValidator<T, TR, TCompiler>
        : IRequestValidator<SavePricingStrategyTableRequest<T, TR, TCompiler>, SavePricingStrategyTableResponse<TR>>
        where T : class, IPricingStrategyTable<T>, IEntity, new()
        where TR : IPricingStrategyTableRowResource
        where TCompiler : IPricingStrategyTableCompiler
    {
        private readonly IPricingStrategyResourceValidator<TR> pricingStrategyResourceValidator;

        public PricingStrategyRequestValidator(IPricingStrategyResourceValidator<TR> pricingStrategyResourceValidator)
        {
            this.pricingStrategyResourceValidator = pricingStrategyResourceValidator;
        }

        public async Task Validate(SavePricingStrategyTableRequest<T, TR, TCompiler> request)
        {
            request.IntentId.ShouldNotBeNullOrEmpty();
            request.TableId.ShouldNotBeNullOrEmpty();
            request.PricingStrategyTableResource.TableTag.ShouldNotBeNullOrEmpty();
            request.PricingStrategyTableResource.TableData.Count.ShouldBeGreaterThan(0);

            await pricingStrategyResourceValidator.Validate(request.PricingStrategyTableResource.TableData);
        }
    }
}