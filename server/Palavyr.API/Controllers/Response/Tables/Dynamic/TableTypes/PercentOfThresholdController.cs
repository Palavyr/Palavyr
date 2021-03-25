using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/PercentOfThreshold")]
    [ApiController]
    public class PercentOfThresholdController : DynamicControllerBase<PercentOfThreshold>
    {
        public PercentOfThresholdController(IDynamicTableCommandHandler<PercentOfThreshold> handler) : base(handler) { }
    }
}