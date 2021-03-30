using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Response.Tables.Dynamic.Meta
{
    public class GetDynamicTableMetasController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetDynamicTableMetasController> logger;

        public GetDynamicTableMetasController(
            IConfigurationRepository configurationRepository,
            ILogger<GetDynamicTableMetasController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpGet("tables/dynamic/type/{areaId}")]
        public async Task<DynamicTableMeta[]> Get([FromHeader] string accountId, string areaId)
        {
            logger.LogDebug("Retrieve Dynamic Table Metas");
            var tableTypes = await configurationRepository.GetDynamicTableMetas(accountId, areaId);
            return tableTypes.ToArray();
        }
    }
}