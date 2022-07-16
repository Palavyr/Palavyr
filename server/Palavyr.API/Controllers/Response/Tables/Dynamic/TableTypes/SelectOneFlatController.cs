using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route(BaseRoute + nameof(CategorySelectTableRow))]
    public class SelectOneFlatController : PricingStrategyControllerBase<CategorySelectTableRow, SelectOneFlatResource, ISelectOneFlatCompiler>
    {
        public SelectOneFlatController(IMediator mediator) : base(mediator)
        {
        }
    }
}