using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.RequestTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.API.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication
{
    [Route("api")]
    [ApiController]
    public class CreateLoginRequestController : ControllerBase
    {
        private readonly IAuthService authService;

        public CreateLoginRequestController(
            IAuthService authService
        )
        {
            this.authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("authentication/login")]
        public async Task<Credentials> RequestLogin([FromBody] LoginCredentials credentials)
        {
            return await authService.PerformLoginAction(credentials);
        }
        
    }
}