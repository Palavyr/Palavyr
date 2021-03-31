using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Authentication
{

    public class CreateLogoutRequestController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;

        public CreateLogoutRequestController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpPost("authentication/logout")]
        public async Task<bool> RequestLogout([FromBody] LogoutCredentials credentials)
        {
            await accountRepository.RemoveSession(credentials.SessionId);
            await accountRepository.CommitChangesAsync();
            return true;
        }

    }
}