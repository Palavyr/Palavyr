using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.receiverTypes;
using Server.Domain.Configuration.schema;

namespace Palavyr.API.Controllers
{
    public partial class SelectOneFlatController
    {
        
        [HttpPut("tables/dynamic/SelectOneFlat/data/save/tableId/{tableId}/{areaId}")]
        public async Task<IActionResult> SaveDynamicTable([FromRoute] string areaId,
            [FromRoute] string tableId,
            [FromHeader] string accountId,
            [FromBody] DynamicTable dynamicTable)
        {
            dashContext.SelectOneFlats.RemoveRange(dashContext
                .SelectOneFlats
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId));

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
                    row.TableId,
                    row.TableTag
                );
                mappedTableRows.Add(mappedRow);
            }

            await dashContext.SelectOneFlats.AddRangeAsync(mappedTableRows);
            
            var meta = await dashContext.DynamicTableMetas.SingleOrDefaultAsync(row => row.TableId == tableId);
            meta.TableTag = dynamicTable.TableTag;

            await dashContext.SaveChangesAsync();

            var tables = dashContext.SelectOneFlats
                .Where(row => row.AccountId == accountId && row.AreaIdentifier == areaId && row.TableId == tableId)
                .ToList();
            return Ok(tables);
        }
    }
}