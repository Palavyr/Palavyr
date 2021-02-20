using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables.PercentOfThresholdOps
{
    public partial class PercentOfThresholdController
    {
        [Route("tables/dynamic/PercentOfThreshold/{areaId}/tableId/{tableId}")]
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
                .PercentOfThresholds
                .RemoveRange(
                    Queryable.Where<PercentOfThreshold>(dashContext.PercentOfThresholds, row =>
                        row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            await dashContext.SaveChangesAsync();

            return NoContent();

            
        }

    }
}