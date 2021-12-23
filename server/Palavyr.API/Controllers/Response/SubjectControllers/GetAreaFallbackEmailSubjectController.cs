using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{

    public class GetAreaFallbackEmailSubjectController : PalavyrBaseController
    {
        private ILogger<GetAreaFallbackEmailSubjectController> logger;
        private readonly IConfigurationRepository configurationRepository;

        public GetAreaFallbackEmailSubjectController(
            ILogger<GetAreaFallbackEmailSubjectController> logger,
            IConfigurationRepository configurationRepository
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
        }

        [HttpGet("email/fallback/subject/{areaId}")]
        public async Task<string> Modify([FromRoute] string areaId)
        {
            var area = await configurationRepository.GetAreaById(areaId);
            var subject = area.FallbackSubject;
            return subject;
        }
    }
}