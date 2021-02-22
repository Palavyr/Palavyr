using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Palavyr.Data;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ModifyAreaFallbackEmailSubjectController : ControllerBase
    {
        private readonly DashContext dashContext;
        private readonly ILogger<ModifyAreaFallbackEmailSubjectController> logger;

        public ModifyAreaFallbackEmailSubjectController(
            DashContext dashContext,
            ILogger<ModifyAreaFallbackEmailSubjectController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("email/fallback/subject/{areaId}")]
        public async Task<string> Modify([FromHeader] string accountId, string areaId, [FromBody] SubjectText subjectText)
        {
            var curArea = await dashContext
                .Areas
                .SingleAsync(row => row.AreaIdentifier == areaId && row.AccountId == accountId);

            if (subjectText.Subject != curArea.FallbackSubject)
            {
                curArea.FallbackSubject = subjectText.Subject;
                await dashContext.SaveChangesAsync();
            }

            return curArea.FallbackSubject;
        }
    }
}