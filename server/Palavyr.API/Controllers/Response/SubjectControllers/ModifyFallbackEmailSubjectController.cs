using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Domain.Resources.Requests;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    [Route("api")]
    [ApiController]
    public class ModifyFallbackEmailSubjectController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<ModifyFallbackEmailSubjectController> logger;

        public ModifyFallbackEmailSubjectController(
            ILogger<ModifyFallbackEmailSubjectController> logger,
            AccountsContext accountsContext
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }

        [HttpPut("email/fallback/default-subject")]
        public async Task<string> Modify([FromHeader] string accountId, [FromRoute] string areaId, [FromBody] SubjectText request)
        {
            var account = await accountsContext.Accounts.SingleAsync(row => row.AccountId == accountId);
            account.GeneralFallbackSubject = request.Subject;
            await accountsContext.SaveChangesAsync();
            return account.GeneralFallbackSubject;
        }
    }

    public class EmailSubjectRequest
    {
        public string EmailSubject { get; set; }
    }
}