using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/" + nameof(BasicThreshold))]
    [ApiController]
    public class BasicThresholdController : PricingStrategyControllerBase<BasicThreshold, BasicThresholdResource>
    {
        public BasicThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}