using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Palavyr.API.Controllers
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
                    await dashContext
                        .DynamicTableMetas
                        .SingleOrDefaultAsync(row =>
                            row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            dashContext
                .SelectOneFlats
                .RemoveRange(
                    dashContext.SelectOneFlats.Where(row =>
                        row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            await dashContext.SaveChangesAsync();

            return NoContent();
        }
    }
}