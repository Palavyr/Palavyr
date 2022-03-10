using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Authentication
{
    public class CreateLogoutRequestController : PalavyrBaseController
    {
        private readonly IEntityStore<Session> sessionStore;

        public CreateLogoutRequestController(IEntityStore<Session> sessionStore)
        {
            this.sessionStore = sessionStore;
        }

        [HttpPost("authentication/logout")]
        public async Task<bool> RequestLogout([FromBody] LogoutCredentials credentials)
        {
            await sessionStore.Delete(credentials.SessionId, s => s.SessionId);
            return true;
        }
    }
}