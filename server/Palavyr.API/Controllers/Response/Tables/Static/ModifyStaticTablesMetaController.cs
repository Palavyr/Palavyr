using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Response.Tables.Static
{
    public class ModifyStaticTablesMetaController : PalavyrBaseController
    {
        private ILogger<ModifyStaticTablesMetaController> logger;
        private readonly IHoldAnAccountId accountIdHolder;
        private readonly IConfigurationRepository configurationRepository;

        public ModifyStaticTablesMetaController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyStaticTablesMetaController> logger,
            IHoldAnAccountId accountIdHolder
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
            this.accountIdHolder = accountIdHolder;
        }

        [HttpPut("response/configuration/{areaId}/static/tables/save")]
        public async Task<List<StaticTablesMeta>> Modify(
            string areaId,
            [FromBody] List<StaticTablesMeta> staticTableMetas
        )
        {
            var metasToDelete = await configurationRepository.GetStaticTables(areaId);
            await configurationRepository.RemoveStaticTables(metasToDelete);

            var clearedMetas = StaticTablesMeta.BindTemplateList(staticTableMetas, accountIdHolder.AccountId);
            var area = await configurationRepository.GetAreaById(areaId);
            area.StaticTablesMetas = clearedMetas;

            await configurationRepository.CommitChangesAsync();

            var tables = await configurationRepository.GetStaticTables(areaId);
            return tables;
        }
    }
}