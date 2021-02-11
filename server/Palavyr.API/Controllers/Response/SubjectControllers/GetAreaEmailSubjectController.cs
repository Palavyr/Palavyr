using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    [Route("api")]
    [ApiController]
    public class GetAreaEmailSubjectController : ControllerBase
    {

        private DashContext dashContext;
        private ILogger<GetAreaEmailSubjectController> logger;

        public GetAreaEmailSubjectController(
            ILogger<GetAreaEmailSubjectController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpGet("email/subject/{areaId}")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var area = await dashContext.Areas.SingleAsync(row => row.AreaIdentifier == areaId && row.AccountId == accountId);
            var subject = area.Subject;
            return subject;
        }
    }
}