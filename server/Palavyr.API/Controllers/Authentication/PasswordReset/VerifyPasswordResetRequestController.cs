using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.FileSystem.UIDUtils;
using Server.Domain.Accounts;



namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    
    [Route("api")]
    [ApiController]
    public class VerifyPasswordResetRequestController : ControllerBase
    {
        private readonly AccountsContext accountsContext;

        public VerifyPasswordResetRequestController(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }

        [AllowAnonymous]
        [HttpPost("authentication/verify-password-reset/{token}")]
        public VerificationResponse Post([FromRoute] string token)
        {
            // the return from this is presented in the redirect page. 
            
            var session = accountsContext.Sessions.SingleOrDefault(row => row.SessionId == token);
            if (session == null)
            {
                return new VerificationResponse("Not Authorized.", false, "");
            }
            if (sessionIsExpired(session.Expiration))
            {
                return new VerificationResponse("Verification link has expired. Please resubmit a password change request.", false, "");
            }
            return new VerificationResponse(
                "Successfully verified your request to reset your password.",
                true,
                session.ApiKey
            );
        }

        private bool sessionIsExpired(DateTime expiration)
        {
            return expiration < DateTime.Now;
        }

        public class VerificationResponse
        {
            public string Message { get; set; }
            public bool Status { get; set; }
            public string ApiKey { get; set; }

            public VerificationResponse(string message, bool status, string apiKey)
            {
                Message = message;
                Status = status;
                ApiKey = apiKey;
            }
        }
    }
}