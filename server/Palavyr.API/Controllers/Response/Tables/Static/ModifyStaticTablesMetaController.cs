using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    [Authorize]
    public class ModifyStaticTablesMetaController : PalavyrBaseController
    {
        private ILogger<ModifyStaticTablesMetaController> logger;
        private readonly IConfigurationRepository configurationRepository;

        public ModifyStaticTablesMetaController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyStaticTablesMetaController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("response/configuration/{areaId}/static/tables/save")]
        public async Task<List<StaticTablesMeta>> Modify(
            string areaId,
            [FromHeader] string accountId,
            [FromBody] List<StaticTablesMeta> staticTableMetas
        )
        {
            var metasToDelete = await configurationRepository.GetStaticTables(accountId, areaId);
            await configurationRepository.RemoveStaticTables(metasToDelete);

            var clearedMetas = StaticTablesMeta.BindTemplateList(staticTableMetas, accountId);
            var area = await configurationRepository.GetAreaById(accountId, areaId);
            area.StaticTablesMetas = clearedMetas;

            await configurationRepository.CommitChangesAsync();

            var tables = await configurationRepository.GetStaticTables(accountId, areaId);
            return tables;
        }
    }
}