using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Palavyr.API.Controllers.Response.DynamicTables.SelectOneFlatOps
{
    public partial class SelectOneFlatController
    {

        [HttpGet("tables/dynamic/SelectOneFlat/tableId/{tableId}/data/{areaId}/")]
        public async Task<IActionResult> GetDynamicTableRows(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromRoute] string tableId)
        { 
            var oneFlats = await Queryable
                .Where(
                    dashContext.SelectOneFlats,
                    row => row.AccountId == accountId
                           && row.AreaIdentifier == areaId
                           && row.TableId == tableId)
                .ToListAsync();
            return Ok(oneFlats);
        }
    }
}