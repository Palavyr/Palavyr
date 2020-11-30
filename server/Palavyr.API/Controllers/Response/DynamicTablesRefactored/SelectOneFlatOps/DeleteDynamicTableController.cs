using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables.SelectOneFlatOps
{
    public partial class SelectOneFlatController
    {
        
        [Route("tables/dynamic/SelectOneFlat/{areaId}/tableId/{tableId}")]
        public async Task<IActionResult> DeleteDynamicTable(
            [FromHeader] string accountId, 
            [FromRoute] string areaId,
            [FromRoute] string tableId
        )
        {
            dashContext
                .DynamicTableMetas
                .Remove(
                    await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<DynamicTableMeta>(dashContext
                            .DynamicTableMetas, row =>
                            row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            dashContext
                .SelectOneFlats
                .RemoveRange(
                    Queryable.Where<SelectOneFlat>(dashContext.SelectOneFlats, row =>
                        row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            await dashContext.SaveChangesAsync();

            return NoContent();
        }
    }
}