using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{

    public class GetAreaEmailSubjectController : PalavyrBaseController
    {
        private ILogger<GetAreaEmailSubjectController> logger;
        private readonly IConfigurationRepository configurationRepository;

        public GetAreaEmailSubjectController(
            ILogger<GetAreaEmailSubjectController> logger,
            IConfigurationRepository configurationRepository
        )
        {
            this.logger = logger;
            this.configurationRepository = configurationRepository;
        }

        [HttpGet("email/subject/{areaId}")]
        public async Task<string> Modify([FromRoute] string areaId, CancellationToken cancellationToken)
        {
            var area = await configurationRepository.GetAreaById(areaId);
            var subject = area.Subject;
            return subject;
        }
    }
}