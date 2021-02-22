using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.RequestTypes;
using Palavyr.Data.Abstractions;

namespace Palavyr.API.Controllers.Authentication
{
    [Route("api")]
    [ApiController]
    public class CreateLogoutRequestController : ControllerBase
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
            await accountsConnector.CommitChanges();
            return true;
        }

    }
}