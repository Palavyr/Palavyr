using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Services.DatabaseService;

namespace Palavyr.API.Controllers.Authentication
{

    public class CreateLogoutRequestController : PalavyrBaseController
    {
        private readonly IAccountsConnector accountsConnector;

        public CreateLogoutRequestController(IAccountsConnector accountsConnector)
        {
            this.accountsConnector = accountsConnector;
        }

        [HttpPost("authentication/logout")]
        public async Task<bool> RequestLogout([FromBody] LogoutCredentials credentials)
        {
            await accountsConnector.RemoveSession(credentials.SessionId);
            await accountsConnector.CommitChangesAsync();
            return true;
        }

    }
}