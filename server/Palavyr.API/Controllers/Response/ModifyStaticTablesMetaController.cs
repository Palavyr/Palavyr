using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Response
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ModifyStaticTablesMetaController : ControllerBase
    {
        private ILogger<ModifyStaticTablesMetaController> logger;
        private readonly IDashConnector dashConnector;

        public ModifyStaticTablesMetaController(
            IDashConnector dashConnector,
            ILogger<ModifyStaticTablesMetaController> logger
        )
        {
            this.dashConnector = dashConnector;
            this.logger = logger;
        }

        [HttpPut("response/configuration/{areaId}/static/tables/save")]
        public async Task<List<StaticTablesMeta>> Modify(
            string areaId,
            [FromHeader] string accountId,
            [FromBody] List<StaticTablesMeta> staticTableMetas
        )
        {
            var metasToDelete = await dashConnector.GetStaticTables(accountId, areaId);
            await dashConnector.RemoveStaticTables(metasToDelete);
            
            var clearedMetas = StaticTablesMeta.BindTemplateList(staticTableMetas, accountId);
            var area = await dashConnector.GetAreaById(accountId, areaId);
            area.StaticTablesMetas = clearedMetas;
            
            await dashConnector.CommitChangesAsync();

            var tables = await dashConnector.GetStaticTables(accountId, areaId);
            return tables;
        }
    }
}