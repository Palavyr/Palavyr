using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/" + nameof(CategoryNestedThreshold))]
    [ApiController]
    public class CategoryNestedThresholdController : PricingStrategyControllerBase<CategoryNestedThreshold, CategoryNestedThresholdResource>
    {
        public CategoryNestedThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}