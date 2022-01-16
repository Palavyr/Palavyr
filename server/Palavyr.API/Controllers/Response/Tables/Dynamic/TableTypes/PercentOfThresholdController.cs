using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;
using Palavyr.Core.Services.DynamicTableService;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.TableTypes
{
    [Route("api/tables/dynamic/PercentOfThreshold")]
    [ApiController]
    public class PercentOfThresholdController : DynamicControllerBase<PercentOfThreshold>
    {
        public PercentOfThresholdController(IDynamicTableCommandExecutor<PercentOfThreshold> executor) : base(executor) { }
    }
}