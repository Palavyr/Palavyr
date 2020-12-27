using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.FileSystem.UIDUtils;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response.DynamicTables.PercentOfThresholdOps
{
    public partial class PercentOfThresholdController
    {
        [HttpGet("tables/dynamic/PercentOfThreshold/tableId/{tableId}/data/{areaId}/")]
        public async Task<IActionResult> GetDynamicTableRows(
            [FromHeader] string accountId,
            [FromRoute] string areaId,
            [FromRoute] string tableId)
        {
            // TODO: Need to actually use SingleOrDefault, and then check for null. Return template or default if
            // the table doesnt exist (case is - we add a new table, selectoneflat is given as default and now exists
            // but then we switch the selector to percent of threshold, and this method now fails since the table ID 
            // doesn't exists. The Key here is that the same tableID is given to each group of possible tables for a 
            // table position. So if we add a new dynamic table, then the default table is selectoneflat and it has
            // a table ID. That table ID is also used to identify the Percentofthreshold table for that component and 
            // we keep track of it in a unique database for that table type. Its just that we track which table type
            // is currently specified in the DynamicTableMetas table. Whichever tableTYPE is inidicated in that table
            // is the table that will be queried for the associated tableID. If we delete the table in the UI (with
            // its tableID) then we delete all record with that tableID from all dynamic table tables.


            var currentTable = await dashContext
                .PercentOfThresholds
                .Where(row => row.AccountId == accountId
                              && row.AreaIdentifier == areaId
                              && row.TableId == tableId)
                .ToListAsync();

            if (currentTable.Count() == 0)
            {
                currentTable = new List<PercentOfThreshold>()
                {
                    PercentOfThreshold.CreateTemplate(
                        accountId,
                        areaId,
                        tableId,
                        GuidUtils.CreateNewId(),
                        GuidUtils.CreateNewId())
                };
            }

            return Ok(currentTable);
        }
    }
}