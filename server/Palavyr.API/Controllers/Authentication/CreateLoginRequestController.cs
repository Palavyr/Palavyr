using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.CommonResponseTypes;
using Palavyr.API.RequestTypes;
using Palavyr.API.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication
{
    [Route("api")]
    [ApiController]
    public class CreateLoginRequestController : ControllerBase
    {
        private readonly ILogger<CreateLoginRequestController> logger;
        private readonly IAuthService authService;

        public CreateLoginRequestController(
            ILogger<CreateLoginRequestController> logger,
            IAuthService authService
        )
        {
            this.logger = logger;
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("authentication/login")]
        public async Task<Credentials> RequestLogin([FromBody] LoginCredentials credentials)
        {
            logger.LogDebug("Login Request Received.");
            return await authService.PerformLoginAction(credentials);
        }
    }
}