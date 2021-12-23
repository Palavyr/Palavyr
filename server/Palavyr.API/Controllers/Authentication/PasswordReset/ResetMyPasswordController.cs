using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class ResetMyPasswordController : PalavyrBaseController
    {
        private readonly IAccountRepository accountRepository;

        public ResetMyPasswordController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }


        [HttpPost("authentication/reset-my-password")]
        public async Task<ResetMyPasswordResponse> Post([FromBody] ResetMyPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new ResetMyPasswordResponse("Email address and password must both be set.", false);
            }

            var account = await accountRepository.GetAccount();
            
            // TODO: Consider password validation?
            account.Password = PasswordHashing.CreateHashedPassword(request.Password);
            await accountRepository.CommitChangesAsync();
            
            return new ResetMyPasswordResponse("Successfully reset your password. Return to the homepage to login with your new password.", true);
        }

        public class ResetMyPasswordResponse
        {
            public string Message { get; set; }
            public bool Status { get; set; }

            public ResetMyPasswordResponse(string message, bool status)
            {
                Message = message;
                Status = status;
            }
        }
        
        public class ResetMyPasswordRequest
        {
            public string Password { get; set; }
        }
    }
}