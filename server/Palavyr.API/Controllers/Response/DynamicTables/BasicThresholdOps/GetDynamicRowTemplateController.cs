using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables.BasicThresholdOps
{
    public partial class BasicThresholdController
    {
        [HttpGet("tables/dynamic/BasicThreshold/data/template/{areaId}/{tableId}")]
        public async Task<IActionResult> GetDynamicRowTemplate(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromRoute] string tableId)
        {
            var template = BasicThreshold.CreateTemplate(
                accountId, 
                areaId, 
                tableId, 
                Guid.NewGuid().ToString());
            return Ok(template);
        }
    }
}