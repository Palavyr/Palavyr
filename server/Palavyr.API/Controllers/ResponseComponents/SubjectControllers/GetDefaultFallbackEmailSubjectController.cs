using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    [Route("api")]
    [ApiController]
    public class GetDefaultFallbackEmailSubjectController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<GetDefaultFallbackEmailSubjectController> logger;

        public GetDefaultFallbackEmailSubjectController(
            ILogger<GetDefaultFallbackEmailSubjectController> logger,
            AccountsContext accountsContext
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }

        [HttpGet("email/default-fallback-subject")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId)
        {
            var account = await accountsContext.Accounts.SingleAsync(row => row.AccountId == accountId);
            var subject = account.GeneralFallbackSubject;
            return subject;
        }
    }
}