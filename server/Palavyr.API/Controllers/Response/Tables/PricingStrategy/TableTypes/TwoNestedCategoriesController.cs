using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy.TableTypes
{
    [Route(BaseRoute + nameof(TwoNestedSelectTableRow))]
    [ApiController]
    public class TwoNestedCategoriesController : PricingStrategyControllerBase<TwoNestedSelectTableRow, TwoNestedCategoryResource, ITwoNestedCategoryCompiler>
    {
        public TwoNestedCategoriesController(IMediator mediator) : base(mediator)
        {
        }
    }
}