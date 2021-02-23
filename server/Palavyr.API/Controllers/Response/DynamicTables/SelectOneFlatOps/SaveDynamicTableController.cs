using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Domain.Configuration.Constant;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.API.Controllers.Response.DynamicTables.SelectOneFlatOps
{
    public partial class SelectOneFlatController
    {
        [HttpPut("tables/dynamic/SelectOneFlat/data/save/tableId/{tableId}/{areaId}")]
        public async Task<IActionResult> SaveDynamicTable([FromRoute] string areaId,
            [FromRoute] string tableId,
            [FromHeader] string accountId,
            [FromBody] DynamicTable dynamicTable)
        {
            dashContext.SelectOneFlats.RemoveRange(Queryable.Where<SelectOneFlat>(dashContext
                    .SelectOneFlats, row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

            var mappedTableRows = new List<SelectOneFlat>();
            foreach (var row in dynamicTable.SelectOneFlat)
            {
                var mappedRow = SelectOneFlat.CreateNew(
                    row.AccountId,
                    row.AreaIdentifier,
                    row.Option,
                    row.ValueMin,
                    row.ValueMax,
                    row.Range,
                    row.TableId
                );
                mappedTableRows.Add(mappedRow);
            }

            await dashContext.SelectOneFlats.AddRangeAsync(mappedTableRows);
            
            var meta = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<DynamicTableMeta>(dashContext.DynamicTableMetas, row => row.TableId == tableId);
            meta.TableTag = dynamicTable.TableTag;
            meta.TableType = DynamicTableTypes.CreateSelectOneFlat().TableType;
            
            await dashContext.SaveChangesAsync();

            var tables = await Queryable
                .Where<SelectOneFlat>(dashContext.SelectOneFlats, row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId)
                .ToListAsync();
            return Ok(tables);
        }
    }
}