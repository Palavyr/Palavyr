using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Data;
using Palavyr.Services.EmailService.Verification;
using Palavyr.Services.StripeServices;

namespace Palavyr.Services.AccountServices
{
    public interface IEmailVerificationService
    {
        public Task<bool> ConfirmEmailAddressAsync(string authToken);
    }

    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly AccountsContext accountsContext;
        private readonly ILogger<EmailVerificationService> logger;
        private readonly IRequestEmailVerification requestEmailVerification;
        private StripeCustomerService stripeCustomerService;

        public EmailVerificationService(
            AccountsContext accountsContext,
            ILogger<EmailVerificationService> logger,
            StripeCustomerService stripeCustomerService,
            IRequestEmailVerification requestEmailVerification
        )
        {
            this.stripeCustomerService = stripeCustomerService;
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.requestEmailVerification = requestEmailVerification;
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
            var emailVerified = await requestEmailVerification.VerifyEmailAddressAsync(emailVerification.EmailAddress);
            
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