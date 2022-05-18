using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Controllers.Authentication
{
    public class GetAuthenticationStatusController : PalavyrBaseController
    {
        public const string Route = "authentication/status";
        private readonly IAccountIdTransport accountIdTransport;
        private ILogger<GetAuthenticationStatusController> logger;

        public GetAuthenticationStatusController(
            IAccountIdTransport accountIdTransport,
            ILogger<GetAuthenticationStatusController> logger
        )
        {
            this.accountIdTransport = accountIdTransport;
            this.logger = logger;
        }

        [HttpGet(Route)]
        public bool Get()
        {
            logger.LogWarning($"Attempt to check authorization for account: {accountIdTransport.AccountId}");
            // if you access this endpoint, you are authorized and the bearer token is active.
            return true;
        }
    }
}