using System.Linq;
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
    public class ModifyAreaEmailSubjectController : ControllerBase
    {

        private readonly DashContext dashContext;
        private readonly ILogger<ModifyAreaEmailSubjectController> logger;

        public ModifyAreaEmailSubjectController(
            DashContext dashContext,
            ILogger<ModifyAreaEmailSubjectController> logger
        )
        {
            this.dashContext = dashContext;
            this.logger = logger;
        }

        [HttpPut("email/subject/{areaId}")]
        public async Task<string> Modify(
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
            return subjectText.Subject;
        }
    }
}