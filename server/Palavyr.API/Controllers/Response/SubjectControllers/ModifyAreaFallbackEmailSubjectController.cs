using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    [Authorize]

    public class ModifyAreaFallbackEmailSubjectController : PalavyrBaseController
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILogger<ModifyAreaFallbackEmailSubjectController> logger;

        public ModifyAreaFallbackEmailSubjectController(
            IConfigurationRepository configurationRepository,
            ILogger<ModifyAreaFallbackEmailSubjectController> logger
        )
        {
            this.configurationRepository = configurationRepository;
            this.logger = logger;
        }

        [HttpPut("email/fallback/subject/{areaId}")]
        public async Task<string> Modify(string areaId, [FromBody] SubjectText subjectText, CancellationToken cancellationToken)
        {
            var curArea = await configurationRepository.GetAreaById(areaId);

            if (subjectText.Subject != curArea.FallbackSubject)
            {
                curArea.FallbackSubject = subjectText.Subject;
                await configurationRepository.CommitChangesAsync();
            }

            return curArea.FallbackSubject;
        }
    }
}