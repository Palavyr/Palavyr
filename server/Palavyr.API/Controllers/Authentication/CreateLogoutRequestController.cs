using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.API.ReceiverTypes;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CreateLogoutRequestController : ControllerBase
    {
        private AccountsContext accountsContext;

        public CreateLogoutRequestController(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }

        [HttpPost("authentication/logout")]
        public async Task<IActionResult> RequestLogout([FromBody] LogoutCredentials credentials)
        {
            accountsContext.Sessions.Remove(
                await accountsContext
                    .Sessions
                    .SingleOrDefaultAsync(row => row.SessionId == credentials.SessionId));
            return Ok(true);
        }

    }
}