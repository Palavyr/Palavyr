using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Configuration.Schemas;

namespace Palavyr.API.Controllers.Response
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ModifyStaticTablesMetaController : ControllerBase
    {
        private ILogger<ModifyStaticTablesMetaController> logger;
        private DashContext dashContext;

        public ModifyStaticTablesMetaController(
            DashContext dashContext,
            ILogger<ModifyStaticTablesMetaController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("response/configuration/{areaId}/static/tables/save")]
        public async Task<IActionResult> Modify(
            string areaId,
            [FromHeader] string accountId,
            [FromBody] List<StaticTablesMeta> staticTableMetas
        )
        {
            var metasToDelete = await dashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .Include(x => x.StaticTableRows)
                .ThenInclude(x => x.Fee)
                .ToListAsync();
    
            foreach (var meta in metasToDelete)
            {
                foreach (var row in meta.StaticTableRows)
                {
                    dashContext.StaticFees.Remove(await dashContext.StaticFees.FindAsync(row.Fee.Id));
                    dashContext.StaticTablesRows.Remove(await dashContext.StaticTablesRows.FindAsync(row.Id));
                }

                dashContext.StaticTablesMetas.Remove(await dashContext.StaticTablesMetas.FindAsync(meta.Id));
            }


            var clearedMetas = StaticTablesMeta.BindTemplateList(staticTableMetas, accountId);
            var currentArea = await dashContext.Areas.Where(row => row.AccountId == accountId)
                .SingleOrDefaultAsync(row => row.AreaIdentifier == areaId);
            currentArea.StaticTablesMetas = clearedMetas;
            await dashContext.SaveChangesAsync();

            var tables = await dashContext
                .StaticTablesMetas
                .Where(row => row.AccountId == accountId)
                .Where(row => row.AreaIdentifier == areaId)
                .ToListAsync();
            return Ok(tables);
        }
    }
}