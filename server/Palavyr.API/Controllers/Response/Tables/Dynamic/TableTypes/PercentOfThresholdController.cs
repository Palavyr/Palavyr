using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/" + nameof(PercentOfThreshold))]
    [ApiController]
    public class PercentOfThresholdController : PricingStrategyControllerBase<PercentOfThreshold, PercentOfThresholdResource>
    {
        public PercentOfThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}