using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route(BaseRoute + nameof(CategoryNestedThreshold))]
    [ApiController]
    public class CategoryNestedThresholdController : PricingStrategyControllerBase<CategoryNestedThreshold, CategoryNestedThresholdResource, ICategoryNestedThresholdCompiler>
    {
        public CategoryNestedThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}