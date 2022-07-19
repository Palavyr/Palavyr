using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Requests;
using Palavyr.Core.Stores;

namespace Palavyr.API.Controllers.Authentication
{
    public class CreateLogoutRequestController : PalavyrBaseController
    {
        private readonly IEntityStore<UserSession> sessionStore;

        public CreateLogoutRequestController(IEntityStore<UserSession> sessionStore)
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