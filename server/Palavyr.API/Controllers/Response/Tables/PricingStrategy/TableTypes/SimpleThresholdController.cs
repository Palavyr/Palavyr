using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy.TableTypes
{
    [Route(BaseRoute + nameof(SimpleThresholdTableRow), Order = 0)]
    [ApiController]
    public class SimpleThresholdController : PricingStrategyControllerBase<SimpleThresholdTableRow, SimpleThresholdResource, ISimpleThresholdCompiler>
    {
        public SimpleThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}