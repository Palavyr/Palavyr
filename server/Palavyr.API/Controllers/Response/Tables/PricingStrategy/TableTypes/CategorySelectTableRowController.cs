using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy.TableTypes
{
    [Route(BaseRoute + nameof(CategorySelectTableRow))]
    public class CategorySelectTableRowController : PricingStrategyControllerBase<CategorySelectTableRow, CategorySelectTableRowResource, ISelectOneFlatCompiler>
    {
        public CategorySelectTableRowController(IMediator mediator) : base(mediator)
        {
        }
    }
}