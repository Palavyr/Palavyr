using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{

    public class ModifyFallbackEmailSubjectController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<ModifyFallbackEmailSubjectController> logger;
        private readonly IAccountRepository accountRepository;

        public ModifyFallbackEmailSubjectController(
            ILogger<ModifyFallbackEmailSubjectController> logger,
            IAccountRepository accountRepository
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
        }

        [HttpPut("email/fallback/default-subject")]
        public async Task<string> Modify([FromRoute] string areaId, [FromBody] SubjectText request)
        {
            var account = await accountRepository.GetAccount();
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