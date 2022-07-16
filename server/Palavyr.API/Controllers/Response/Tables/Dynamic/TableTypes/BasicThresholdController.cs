using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities.DynamicTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route(BaseRoute + nameof(SimpleThresholdTableRow), Order = 0)]
    [ApiController]
    public class BasicThresholdController : PricingStrategyControllerBase<SimpleThresholdTableRow, BasicThresholdResource, IBasicThresholdCompiler>
    {
        public BasicThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}