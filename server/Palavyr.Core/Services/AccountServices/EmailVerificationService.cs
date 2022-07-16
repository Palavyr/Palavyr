using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Setup.WelcomeEmail;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.AccountServices
{
    public interface IEmailVerificationService
    {
        Task<bool> ConfirmEmailAddressAsync(string authToken, CancellationToken cancellationToken);
        Task<bool> SendConfirmationTokenEmail(string emailAddress, CancellationToken cancellationToken);
    }

    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly ILogger<EmailVerificationService> logger;
        private readonly IRequestEmailVerification requestEmailVerification;
        private readonly ISesEmail emailClient;
        private readonly IGuidUtils guidUtils;
        private readonly IEmailVerificationStatus emailVerificationStatus;
        private readonly IAccountIdTransport accountIdTransport;
        private readonly IEntityStore<Account> accountStore;
        private readonly IEntityStore<EmailVerification> emailVerificationsStore;
        private IStripeCustomerService stripeCustomerService;

        public EmailVerificationService(
            IEntityStore<Account> accountStore,
            IEntityStore<EmailVerification> emailVerificationsStore,
            ILogger<EmailVerificationService> logger,
            IStripeCustomerService stripeCustomerService,
            IRequestEmailVerification requestEmailVerification,
            ISesEmail emailClient,
            IGuidUtils guidUtils,
            IEmailVerificationStatus emailVerificationStatus,
            IAccountIdTransport accountIdTransport
        )
        {
            this.accountStore = accountStore;
            this.emailVerificationsStore = emailVerificationsStore;
            this.stripeCustomerService = stripeCustomerService;
            this.logger = logger;
            this.requestEmailVerification = requestEmailVerification;
            this.emailClient = emailClient;
            this.guidUtils = guidUtils;
            this.emailVerificationStatus = emailVerificationStatus;
            this.accountIdTransport = accountIdTransport;
        }

        public async Task<bool> ConfirmEmailAddressAsync(string authToken, CancellationToken cancellationToken)
        {
            logger.LogDebug("Attempting to confirm email via auth Token.");
            var emailVerification = await emailVerificationsStore.GetOrNull(authToken.Trim(), s => s.AuthenticationToken);

            if (emailVerification == null)
            {
                return false;
            }

            logger.LogDebug("Email Address found.");

            var account = await accountStore.GetOrNull(emailVerification.AccountId, s => s.AccountId);
            if (account == null)
            {
                return false;
            }

            account.Active = true;

            await emailVerificationsStore.Delete(emailVerification);

            logger.LogDebug("Verifying email address. Already verified using an authtoken, so this is okay");

            bool emailVerified;
            var alreadyVerified = await emailVerificationStatus.CheckVerificationStatus(account.EmailAddress);
            if (alreadyVerified)
            {
                emailVerified = true;
            }
            else
            {
                emailVerified = await requestEmailVerification.VerifyEmailAddressAsync(emailVerification.EmailAddress);
            }

            if (emailVerified)
            {
                var customer = await stripeCustomerService.CreateNewStripeCustomer(account.EmailAddress, cancellationToken);

                account.StripeCustomerId = customer.Id;

                return true;
            }

            return false;
        }

        public async Task<bool> SendConfirmationTokenEmail(string emailAddress, CancellationToken cancellationToken)
        {
            // prepare the account confirmation email
            logger.LogDebug("Provide an account setup confirmation token");
            var confirmationToken = guidUtils.CreateShortenedGuid(1);
            await emailVerificationsStore.Create(EmailVerification.CreateNew(confirmationToken, emailAddress, accountIdTransport.AccountId));

            logger.LogDebug($"Sending emails from {EmailConstants.PalavyrMainEmailAddress}");
            var htmlBody = EmailConfirmationHtml.GetConfirmationEmailBody(emailAddress, confirmationToken);
            var textBody = EmailConfirmationHtml.GetConfirmationEmailBodyText(emailAddress, confirmationToken);

            var sendEmailOk = await emailClient.SendEmail(EmailConstants.PalavyrMainEmailAddress, emailAddress, EmailConstants.PalavyrSubject, htmlBody, textBody);
            return sendEmailOk;
        }
    }
}