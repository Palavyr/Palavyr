using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables.BasicThresholdOps
{
    public partial class BasicThresholdController
    {
        [Route("tables/dynamic/BasicThreshold/{areaId}/tableId/{tableId}")]
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
                .BasicThresholds
                .RemoveRange(
                    Queryable.Where<BasicThreshold>(dashContext.BasicThresholds, row =>
                        row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            await dashContext.SaveChangesAsync();

            return NoContent();
        }
    }
}