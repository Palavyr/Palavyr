using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    [Route("api")]
    [ApiController]
    public class GetAreaFallbackEmailSubjectController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<GetAreaFallbackEmailSubjectController> logger;

        public GetAreaFallbackEmailSubjectController(
            ILogger<GetAreaFallbackEmailSubjectController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpGet("email/fallback/subject/{areaId}")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var area = await dashContext.Areas.SingleAsync(row => row.AreaIdentifier == areaId && row.AccountId == accountId);
            var subject = area.FallbackSubject;
            return subject;
        }
    }
}