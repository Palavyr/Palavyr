using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.API.Controllers.Response.DynamicTables.PercentOfThresholdOps
{
    public partial class PercentOfThresholdController
    {
        [HttpPut("tables/dynamic/PercentOfThreshold/data/save/tableId/{tableId}/{areaId}")]
        public async Task<IActionResult> SaveDynamicTable([FromRoute] string areaId,
            [FromRoute] string tableId,
            [FromHeader] string accountId,
            [FromBody] DynamicTable dynamicTable) // TODO: Not sure this will work here- need to strongly type requests
        {
            dashContext.PercentOfThresholds.RemoveRange(dashContext.PercentOfThresholds.Where(
                row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            var mappedTableRows = new List<PercentOfThreshold>();
            foreach (var row in dynamicTable.PercentOfThreshold)
            {
                var mappedRow = PercentOfThreshold.CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.TableId,
                    row.RowId,
                    row.Threshold,
                    row.Modifier,
                    row.ItemName,
                    row.ItemId,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range,
                    row.PosNeg
                );
                mappedTableRows.Add(mappedRow);
            }

            await dashContext.PercentOfThresholds.AddRangeAsync(mappedTableRows);

            var meta = dashContext.DynamicTableMetas.Single(row => row.TableId == tableId);
            meta.TableTag = dynamicTable.TableTag;
            meta.TableType = DynamicTableTypes.CreatePercentOfThreshold().TableType;
            
            await dashContext.SaveChangesAsync();

            var tables = await dashContext
                .PercentOfThresholds
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId)
                .ToListAsync();
            
            return Ok(tables);
        }
    }
}