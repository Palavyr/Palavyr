using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response
{
    [Route("api")]
    [ApiController]
    public class GetDefaultFallbackEmailTemplateController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<GetDefaultFallbackEmailTemplateController> logger;

        public GetDefaultFallbackEmailTemplateController(
            ILogger<GetDefaultFallbackEmailTemplateController> logger,
            AccountsContext accountsContext
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }

        [HttpGet("email/default-fallback-email-template")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var account = await accountsContext.Accounts.SingleAsync(row => row.AccountId == accountId);
            var currentDefaultEmailTemplate = account.GeneralFallbackEmailTemplate;
            return currentDefaultEmailTemplate;
        }
    }
}
