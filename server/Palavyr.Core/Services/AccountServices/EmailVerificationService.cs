using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Data;
using Palavyr.Core.Data.Setup.WelcomeEmail;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.Core.Services.AccountServices
{
    public interface IEmailVerificationService
    {
        Task<bool> ConfirmEmailAddressAsync(string authToken, CancellationToken cancellationToken);
        Task<bool> SendConfirmationTokenEmail(string emailAddress, string accountId, CancellationToken cancellationToken);
    }

    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly AccountsContext accountsContext;
        private readonly ILogger<EmailVerificationService> logger;
        private readonly IRequestEmailVerification requestEmailVerification;
        private readonly ISesEmail emailClient;
        private StripeCustomerService stripeCustomerService;

        public EmailVerificationService(
            AccountsContext accountsContext,
            ILogger<EmailVerificationService> logger,
            StripeCustomerService stripeCustomerService,
            IRequestEmailVerification requestEmailVerification,
            ISesEmail emailClient
        )
        {
            this.stripeCustomerService = stripeCustomerService;
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.requestEmailVerification = requestEmailVerification;
            this.emailClient = emailClient;
        }

        public async Task<bool> ConfirmEmailAddressAsync(string authToken, CancellationToken cancellationToken)
        {
            logger.LogDebug("Attempting to confirm email via auth Token.");
            var emailVerification = await accountsContext
                .EmailVerifications
                .SingleOrDefaultAsync(row => row.AuthenticationToken == authToken.Trim(), cancellationToken);
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

        public async Task<bool> SendConfirmationTokenEmail(string emailAddress, string accountId, CancellationToken cancellationToken)
        {
            // prepare the account confirmation email
            logger.LogDebug("Provide an account setup confirmation token");
            var confirmationToken = GuidUtils.CreateShortenedGuid(1);
            await accountsContext.EmailVerifications.AddAsync(EmailVerification.CreateNew(confirmationToken, emailAddress, accountId));
            await accountsContext.SaveChangesAsync(cancellationToken);

            logger.LogDebug($"Sending emails from {EmailConstants.PalavyrMainEmailAddress}");
            var htmlBody = EmailConfirmationHtml.GetConfirmationEmailBody(emailAddress, confirmationToken);
            var textBody = EmailConfirmationHtml.GetConfirmationEmailBodyText(emailAddress, confirmationToken);

            var sendEmailOk = await emailClient.SendEmail(EmailConstants.PalavyrMainEmailAddress, emailAddress, EmailConstants.PalavyrSubject, htmlBody, textBody);
            return sendEmailOk;
        }
    }
}