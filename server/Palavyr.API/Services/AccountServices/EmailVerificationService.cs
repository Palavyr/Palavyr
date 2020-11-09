using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService.verification;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.controllers.accounts.newAccount
{
    public interface IEmailVerificationService
    {
        public Task<bool> ConfirmEmailAddressAsync(string authToken);
    }

    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly AccountsContext _accountsContext;
        private readonly ILogger<EmailVerificationService> _logger;
        public AccountsContext accountContext { get; set; }
        private SenderVerification Verifier { get; set; }
        private readonly IStripeClient _stripeClient;
        
        public EmailVerificationService(
            AccountsContext accountsContext,
            ILogger<EmailVerificationService> logger,
            IAmazonSimpleEmailService SESClient
        )
        {
            _stripeClient = new StripeClient();
            _accountsContext = accountsContext;
            _logger = logger;
            Verifier = new SenderVerification(logger, SESClient);
        }

        public async Task<bool> ConfirmEmailAddressAsync(string authToken)
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
                var options = new CustomerCreateOptions
                {
                    Description = "Customer automatically added via the dashboard.",
                    Email = account.EmailAddress,
                };
                var service = new CustomerService();
                var customer = await service.CreateAsync(options);
                account.StripeCustomerId = customer.Id;
                
                await _accountsContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}