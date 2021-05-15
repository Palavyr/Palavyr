using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AuthenticationServices;

namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    [Authorize(AuthenticationSchemes = AuthenticationSchemeNames.ApiKeyScheme)]

    public class ResetMyPasswordController : PalavyrBaseController
    {
        private readonly AccountsContext accountsContext;

        public ResetMyPasswordController(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }


        [HttpPost("authentication/reset-my-password")]
        public async Task<ResetMyPasswordResponse> Post([FromHeader] string accountId, [FromBody] ResetMyPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return new ResetMyPasswordResponse("Email address and password must both be set.", false);
            }

            var account = accountsContext.Accounts.Single(row => row.AccountId == accountId);
            
            // TODO: Consider password validation?
            account.Password = PasswordHashing.CreateHashedPassword(request.Password);
            await accountsContext.SaveChangesAsync();

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