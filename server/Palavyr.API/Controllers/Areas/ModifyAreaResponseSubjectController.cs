using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;

namespace Palavyr.API.Controllers.Areas
{
    
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ModifyAreaResponseSubjectController : ControllerBase
    {

        private readonly DashContext dashContext;
        private readonly ILogger<ModifyAreaResponseSubjectController> logger;

        public ModifyAreaResponseSubjectController(
            DashContext dashContext,
            ILogger<ModifyAreaResponseSubjectController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("areas/update/subject/{areaId}")]
        public async Task<NoContentResult> Modify(
            [FromHeader] string accountId,
            [FromBody] SubjectText subjectText,
            string areaId
        )
        {
            var curArea = await dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .SingleAsync(row => row.AreaIdentifier == areaId);

            if (subjectText.Subject != curArea.Subject)
            {
                curArea.Subject = subjectText.Subject;
                await dashContext.SaveChangesAsync();
            }

            return NoContent();

        }
    }
}