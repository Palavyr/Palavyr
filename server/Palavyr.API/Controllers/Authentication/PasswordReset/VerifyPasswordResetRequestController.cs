using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Repositories;

namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    

    public class VerifyPasswordResetRequestController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;

        public VerifyPasswordResetRequestController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [AllowAnonymous]
        [HttpPost("authentication/verify-password-reset/{token}")]
        public async Task<VerificationResponse> Post([FromRoute] string token)
        {
            // the return from this is presented in the redirect page. 
            var session = await accountRepository.GetSessionOrNull(token);
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