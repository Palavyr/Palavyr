using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    public partial class SelectOneFlatController
    {

        [HttpGet("tables/dynamic/SelectOneFlat/tableId/{tableId}/data/{areaId}/")]
        public async Task<IActionResult> GetDynamicTableRows(
            [FromHeader] string accountId, 
            [FromRoute] string areaId, 
            [FromRoute] string tableId)
        {
            var oneFlats = dashContext.SelectOneFlats.Where(
                row => row.AccountId == accountId
                       && row.AreaIdentifier == areaId
                       && row.TableId == tableId).ToList();
            return Ok(oneFlats);
        }
    }
}