﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication
{
    public class GetAuthenticationStatusController : PalavyrBaseController
    {
        public const string Route = "authentication/status";
        private ILogger<GetAuthenticationStatusController> logger;

        public GetAuthenticationStatusController(
            IAuthService authService,
            ILogger<GetAuthenticationStatusController> logger
        )
        {
            this.logger = logger;
        }

        [HttpGet(Route)]
        public bool Get()
        {
            // if you access this endpoint, you are authorized and the bearer token is active.
            return true;
        }
    }
}