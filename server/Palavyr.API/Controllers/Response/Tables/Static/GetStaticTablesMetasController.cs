using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response
{

    public class GetStaticTablesMetasController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetStaticTablesMetasController> logger;

        public GetStaticTablesMetasController(IConfigurationRepository configurationRepository, ILogger<GetStaticTablesMetasController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }
        
        [HttpGet("response/configuration/{areaId}/static/tables")]
        public async Task<List<StaticTablesMeta>> GetStaticTablesMetas([FromHeader] string accountId, string areaId)
        {
            var staticTables = await configurationRepository.GetStaticTables(accountId, areaId);
            return staticTables;
        }
    }
}