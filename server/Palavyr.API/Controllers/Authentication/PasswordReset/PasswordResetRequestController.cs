using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using EmailService.ResponseEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Common.UIDUtils;
using Server.Domain.Accounts;

namespace Palavyr.API.Controllers.Authentication.PasswordReset
{
    [Route("api")]
    [ApiController]
    public class PasswordResetRequestController : ControllerBase
    {
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail client;


        public PasswordResetRequestController(
            AccountsContext accountsContext,
            ISesEmail client
        )
        {
            this.accountsContext = accountsContext;
            this.client = client;
        }

        [AllowAnonymous]
        [HttpPost("authentication/password-reset-request")]
        public async Task<ResetEmailResponse> Post([FromBody] ResetEmailRequest request)
        {
            var ambiguousMessage = "An email was sent to this address if an account for it exists.";

            var account = accountsContext.Accounts.SingleOrDefault(row => row.EmailAddress == request.EmailAddress);
            if (account == null)
            {
                return new ResetEmailResponse(ambiguousMessage, false);
            }

            if (account.AccountType != AccountType.Default)
            {
                return new ResetEmailResponse("This account does use password sign-in.", false);
            }

            var token = string.Join("-", new[] {GuidUtils.CreateNewId(), GuidUtils.CreateNewId(), GuidUtils.CreateNewId(), GuidUtils.CreateNewId()}).Replace("-", "");
            var accountId = account.AccountId;
            var apiKey = account.ApiKey;

            var session = Session.CreateNew(token, accountId, apiKey);
            await accountsContext.Sessions.AddAsync(session);
            await accountsContext.SaveChangesAsync();

            var link = request.ResetPasswordLinkTemplate + token;

            var subject = "Reset your Palavyr Password";
            var htmlBody = $"<h2>Click on the following link to reset your password:</h2><p>{link}</p>";
            var textBody = $"Click on the following link to reset your password: {link}";
            var ok = await client.SendEmail(EmailConstants.PalavyrMainEmailAddress, request.EmailAddress, subject, htmlBody, textBody);
            if (!ok)
            {
                return new ResetEmailResponse("That doesn't seem to be a real email address. Maybe check your spelling?", false);
            }

            return new ResetEmailResponse(ambiguousMessage, true);
        }
    }

    public class ResetEmailResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }

        public ResetEmailResponse(string message, bool status)
        {
            Message = message;
            Status = status;
        }
    }

    public class ResetEmailRequest
    {
        public string EmailAddress { get; set; }
        public string ResetPasswordLinkTemplate { get; set; }
    }
}