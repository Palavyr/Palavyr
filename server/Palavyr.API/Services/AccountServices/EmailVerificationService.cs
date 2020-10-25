using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService.verification;
using Microsoft.Extensions.Logging;

namespace Palavyr.API.controllers.accounts.newAccount
{
    public interface IEmailVerificationService
    {
        public Task<bool> ConfirmEmailAddress(string authToken);
    }
    
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly AccountsContext _accountsContext;
        private ILogger<EmailVerificationService> _logger;
        public AccountsContext accountContext { get; set; }
        private SenderVerification Verifier { get; set; }

        public EmailVerificationService(
            AccountsContext accountsContext,
            ILogger<EmailVerificationService> logger,
            IAmazonSimpleEmailService SESClient
            )
        {
            _accountsContext = accountsContext;
            _logger = logger;
            Verifier = new SenderVerification(logger, SESClient);
        }

        public async Task<bool> ConfirmEmailAddress(string authToken)
        {
            _logger.LogDebug("Attempting to confirm email via auth Token.");
            var emailVerification =
                _accountsContext.EmailVerifications.SingleOrDefault(row => row.AuthenticationToken == authToken.Trim());
            if (emailVerification == null)
                return false;

            _logger.LogDebug("Email Address found.");
            var accountId = emailVerification.AccountId;
            var account = _accountsContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            if (account == null)
                return false;

            account.Active = true;
            _accountsContext.EmailVerifications.Remove(emailVerification);

            _logger.LogDebug("Verifying email address. Already verified using an authtoken, so this is okay");
            var emailVerified = await Verifier.VerifyEmailAddress(emailVerification.EmailAddress);

            if (emailVerified)
            {
                await _accountsContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}