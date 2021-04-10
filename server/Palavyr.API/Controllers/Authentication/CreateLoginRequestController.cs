using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication
{
    public class CreateLoginRequestController : PalavyrBaseController
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

        // https://codeburst.io/jwt-auth-in-asp-net-core-148fb72bed03
        [AllowAnonymous]
        [HttpPost("authentication/login")]
        public async Task<Credentials> RequestLogin([FromBody] LoginCredentialsRequest credentialsRequest)
        {
            logger.LogDebug("Login Request Received.");
            try
            {
                return await authService.PerformLoginAction(credentialsRequest);
            }
            catch (Exception ex)
            {
                logger.LogDebug("****************************");
                logger.LogDebug(ex.Message);
                throw;
            }
        }
    }
}