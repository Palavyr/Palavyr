using DashboardServer.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.ReceiverTypes;


namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ModifyEmailTemplateController : ControllerBase
    {
        private DashContext dashContext;
        private ILogger<ModifyEmailTemplateController> logger;

        public ModifyEmailTemplateController(
            ILogger<ModifyEmailTemplateController> logger,
            DashContext dashContext
        )
        {
            this.logger = logger;
            this.dashContext = dashContext;
        }

        [HttpPut("email/{areaId}/emailTemplate")]
        public async Task<IActionResult> Modify([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] Text text)
        {
            var currentArea = dashContext
                .Areas
                .Where(row => row.AccountId == accountId)
                .Single(row => row.AreaIdentifier == areaId);
            currentArea.EmailTemplate = text.EmailTemplate;
            await dashContext.SaveChangesAsync();
            return Ok(currentArea.EmailTemplate);
        }
    }
}