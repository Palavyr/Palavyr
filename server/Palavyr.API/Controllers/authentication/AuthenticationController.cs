using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.controllers.accounts.newAccount;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.ResponseTypes;


namespace Palavyr.API.Controllers
{
    [Authorize]
    [Route("api/authentication")]
    [ApiController]
    public class Authentication : BaseController
    {
        private static ILogger<Authentication> _logger;
        private IAccountSetupService _accountSetup;
        private readonly IAuthService _authService;

        public Authentication(
            IAuthService authService,
            IAccountSetupService accountSetupService,
            ILogger<Authentication> logger, 
            AccountsContext accountContext, 
            ConvoContext convoContext,
            DashContext dashContext, 
            IWebHostEnvironment env
            ) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
            _accountSetup = accountSetupService;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Credentials> RequestLogin([FromBody] LoginCredentials credentials)
        {
            return await _authService.PerformLoginAction(credentials);
        }

        [HttpPost("logout")]
        public bool RequestLogout([FromBody] LogoutCredentials credentials)
        {
            AccountContext.Sessions.Remove(AccountContext.Sessions.SingleOrDefault(row => row.SessionId == credentials.SessionId));
            return true;
        }
        
        [HttpGet("status")]
        public bool RequestStatus()
        {
            // if you access this endpoint, you are authorized and the bearer token is active.
            return true;
        }
        
        
    }
}