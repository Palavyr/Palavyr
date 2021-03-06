using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.API.Controllers.Response.DynamicTables.BasicThresholdOps
{
    public partial class BasicThresholdController
    {
        [HttpPut("tables/dynamic/BasicThreshold/data/save/tableId/{tableId}/{areaId}")]
        public async Task<IActionResult> SaveDynamicTable(
            [FromRoute] string areaId,
            [FromRoute] string tableId,
            [FromHeader] string accountId,
            [FromBody] DynamicTable dynamicTable
        ) // TODO: Not sure this will work here- need to strongly type requests
        {
            dashContext.BasicThresholds.RemoveRange(
                dashContext.BasicThresholds.Where(
                    row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            var mappedTableRows = new List<BasicThreshold>();
            foreach (var row in dynamicTable.BasicThreshold)
            {
                var mappedRow = BasicThreshold.CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.TableId,
                    row.RowId,
                    row.Threshold,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range
                );
                mappedTableRows.Add(mappedRow);
            }

            await dashContext.BasicThresholds.AddRangeAsync(mappedTableRows);

            var meta = dashContext.DynamicTableMetas.Single(row => row.TableId == tableId);
            meta.TableTag = dynamicTable.TableTag;
            meta.TableType = DynamicTableTypes.CreateBasicThreshold().TableType;

            await dashContext.SaveChangesAsync();

            var tables = await dashContext
                .BasicThresholds
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId)
                .ToListAsync();

            return Ok(tables);
        }
    }
}