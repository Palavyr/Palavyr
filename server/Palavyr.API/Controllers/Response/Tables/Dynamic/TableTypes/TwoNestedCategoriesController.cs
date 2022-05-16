using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/" + nameof(TwoNestedCategory))]
    [ApiController]
    public class TwoNestedCategoriesController : PricingStrategyControllerBase<TwoNestedCategory, TwoNestedCategoryResource>
    {
        public TwoNestedCategoriesController(IMediator mediator) : base(mediator)
        {
        }
    }
}