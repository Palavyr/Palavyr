using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService.VerificationRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.StripeServices;

namespace Palavyr.API.Services.AccountServices
{
    public interface IEmailVerificationService
    {
        public Task<bool> ConfirmEmailAddressAsync(string authToken);
    }

    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly AccountsContext accountsContext;
        private readonly ILogger<EmailVerificationService> logger;
        public AccountsContext accountContext { get; set; }
        private SenderVerification Verifier { get; set; }
        private IStripeCustomerService stripeCustomerService;

        public EmailVerificationService(
            AccountsContext accountsContext,
            ILogger<EmailVerificationService> logger,
            IAmazonSimpleEmailService sesClient,
            IStripeCustomerService stripeCustomerService
        )
        {
            this.stripeCustomerService = stripeCustomerService;
            this.accountsContext = accountsContext;
            this.logger = logger;
            Verifier = new SenderVerification(logger, sesClient);
        }

        public async Task<bool> ConfirmEmailAddressAsync(string authToken)
        {
            logger.LogDebug("Attempting to confirm email via auth Token.");
            var emailVerification = await accountsContext
                .EmailVerifications
                .SingleOrDefaultAsync(row => row.AuthenticationToken == authToken.Trim());
            if (emailVerification == null)
            {
                return false;
            }

            logger.LogDebug("Email Address found.");
            var accountId = emailVerification.AccountId;
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.AccountId == accountId);
            if (account == null)
            {
                return false;
            }

            account.Active = true;
            accountsContext.EmailVerifications.Remove(emailVerification);

            logger.LogDebug("Verifying email address. Already verified using an authtoken, so this is okay");
            var emailVerified = await Verifier.VerifyEmailAddressAsync(emailVerification.EmailAddress);
            
            if (emailVerified)
            {
                var customer = await stripeCustomerService.CreateNewStripeCustomer(account.EmailAddress);
                
                account.StripeCustomerId = customer.Id;
                
                await accountsContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}