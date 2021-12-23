using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Repositories;

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
        public async Task<List<Area>> Get()
        {
            logger.LogDebug("Return all areas");
            var areas = await configurationRepository.GetAllAreasShallow();
            return areas;
        }
    }
}