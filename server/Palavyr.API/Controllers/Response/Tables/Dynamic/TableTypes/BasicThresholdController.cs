using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/BasicThreshold")]
    [ApiController]
    public class BasicThresholdController : DynamicControllerBase<BasicThreshold>
    {
        public BasicThresholdController(IDynamicTableCommandHandler<BasicThreshold> handler) : base(handler)
        {
        }
    }
}