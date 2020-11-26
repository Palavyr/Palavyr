using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables.SelectOneFlatOps
{
    public partial class SelectOneFlatController
    {

        [Route("tables/dynamic/SelectOneFlat/data/template/{areaId}/{tableId}")]
        public async Task<IActionResult> GetDynamicRowTemplate(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromRoute] string tableId)
        {
            var template = SelectOneFlat.CreateTemplate(accountId, areaId, tableId);
            return Ok(template);
        }
    }
}