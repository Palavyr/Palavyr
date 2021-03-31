using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{

    public class ModifyFallbackEmailSubjectController : PalavyrBaseController
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