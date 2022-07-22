using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy.TableTypes
{
    [Route(BaseRoute + nameof(SelectWithNestedSelectTableRow))]
    [ApiController]
    public class TwoNestedCategoriesController : PricingStrategyControllerBase<SelectWithNestedSelectTableRow, SelectWithNestedSelectResource, ITwoNestedCategoryCompiler>
    {
        public TwoNestedCategoriesController(IMediator mediator) : base(mediator)
        {
        }
    }
}