using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.ResponseTypes;

namespace Palavyr.API.Controllers
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