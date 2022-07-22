using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities.PricingStrategyTables;
using Palavyr.Core.Resources.PricingStrategyResources;
using Palavyr.Core.Services.PricingStrategyTableServices.Compilers;

namespace Palavyr.API.Controllers.Response.Tables.PricingStrategy.TableTypes
{
    [Route(BaseRoute + nameof(PercentOfThresholdTableRow))]
    [ApiController]
    public class PercentOfThresholdController : PricingStrategyControllerBase<PercentOfThresholdTableRow, PercentOfThresholdResource, IPercentOfThresholdCompiler>
    {
        public PercentOfThresholdController(IMediator mediator) : base(mediator)
        {
        }
    }
}