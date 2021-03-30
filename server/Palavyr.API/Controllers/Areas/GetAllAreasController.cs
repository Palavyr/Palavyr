using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Controllers.Areas
{
    [Authorize]

    public class GetAllAreasController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private ILogger<GetAllAreasController> logger;

        public GetAllAreasController(IConfigurationRepository configurationRepository, ILogger<GetAllAreasController> logger)
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }
        
        [HttpGet("areas")]
        public async Task<List<Area>> Get([FromHeader] string accountId)
        {
            logger.LogDebug("Return all areas");
            var areas = await configurationRepository.GetAllAreasShallow(accountId);
            return areas;
        }
    }
}