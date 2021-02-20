using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables.PercentOfThresholdOps
{
    public partial class PercentOfThresholdController
    {
        [HttpGet("tables/dynamic/SelectOneFlat/data/template/{areaId}/{tableId}")]
        public async Task<IActionResult> GetDynamicRowTemplate(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromRoute] string tableId)
        {
            var template = PercentOfThreshold.CreateTemplate(
                accountId, 
                areaId, 
                tableId, 
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString());
            return Ok(template);
        }
    }
}