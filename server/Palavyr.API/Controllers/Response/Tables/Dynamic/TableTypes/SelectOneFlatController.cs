using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/" + nameof(SelectOneFlat))]
    public class SelectOneFlatController : PricingStrategyControllerBase<SelectOneFlat, SelectOneFlatRowResource>
    {
        public SelectOneFlatController(IMediator mediator) : base(mediator)
        {
        }
    }
}