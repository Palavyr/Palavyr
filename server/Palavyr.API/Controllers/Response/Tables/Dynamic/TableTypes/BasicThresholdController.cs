using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route(BaseRoute + nameof(BasicThreshold))]
    [ApiController]
    public class BasicThresholdController : PricingStrategyControllerBase<BasicThreshold, BasicThresholdResource, IBasicThresholdCompiler>
    {
        public BasicThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}