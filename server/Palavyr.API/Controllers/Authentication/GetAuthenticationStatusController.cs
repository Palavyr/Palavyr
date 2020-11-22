﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class GetAuthenticationStatusController : ControllerBase
    {
        private ILogger<GetAuthenticationStatusController> logger;
        
        public GetAuthenticationStatusController(
            IAuthService authService,
            ILogger<GetAuthenticationStatusController> logger 
            )
        {
            this.logger = logger;
        }

        [HttpGet("authentication/status")]
        public bool Get()
        {
            // if you access this endpoint, you are authorized and the bearer token is active.
            return true;
        }
    }
}