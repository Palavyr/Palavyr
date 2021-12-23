using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

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
        public async Task<DynamicTableMeta[]> Get(string areaId)
        {
            logger.LogDebug("Retrieve Dynamic Table Metas");
            var tableTypes = await configurationRepository.GetDynamicTableMetas(areaId);
            return tableTypes.ToArray();
        }
    }
}