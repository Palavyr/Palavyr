using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Requests.Registration;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.Core.Services.AccountServices
{
    public class AccountSetupService : IAccountSetupService
    {
        private readonly DashContext dashContext;
        private readonly AccountsContext accountsContext;
        private readonly INewAccountUtils newAccountUtils;
        private readonly ILogger<AuthService> logger;
        private readonly IAuthService authService;
        private readonly IJwtAuthenticationService jwtAuthService;
        private readonly StripeCustomerService stripeCustomerService;
        private readonly IGuidUtils guidUtils;
        private readonly IAccountRegistrationMaker accountRegistrationMaker;


        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string AccountAlreadyExists = "Account already exists";
        private const string EmailAddressNotFound = "Email Address Not Found";

        // private const string AccountNotAllowed = "Account not allowed in this environment.";

        public AccountSetupService(
            DashContext dashContext,
            AccountsContext accountsContext,
            INewAccountUtils newAccountUtils,
            ILogger<AuthService> logger,
            IAuthService authService,
            IJwtAuthenticationService jwtService,
            StripeCustomerService stripeCustomerService,
            IGuidUtils guidUtils,
            IAccountRegistrationMaker accountRegistrationMaker
        )
        {
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.newAccountUtils = newAccountUtils;
            this.logger = logger;
            this.authService = authService;
            jwtAuthService = jwtService;
            this.stripeCustomerService = stripeCustomerService;
            this.guidUtils = guidUtils;
            this.accountRegistrationMaker = accountRegistrationMaker;
        }

        private string CreateNewJwtToken(Account account)
        {
            return jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
        }

        private Session CreateNewSession(Account account)
        {
            var sessionId = guidUtils.CreateNewId();
            logger.LogDebug("Attempting to create a new Session.");
            var session = Session.CreateNew(sessionId, account.AccountId, account.ApiKey);
            logger.LogDebug($"New Session created: {session.SessionId}");
            return session;
        }

        public async Task<Credentials> CreateNewAccountViaDefaultAsync(AccountDetails newAccountDetails, CancellationToken cancellationToken)
        {
            // confirm account doesn't already exist
            if (AccountExists(newAccountDetails.EmailAddress))
            {
                logger.LogDebug($"Account for email address {newAccountDetails.EmailAddress} already exists");
                return Credentials.CreateUnauthenticatedResponse(AccountAlreadyExists);
            }

            // Add the new account
            logger.LogDebug("Creating a new account");
            var accountId = newAccountUtils.GetNewAccountId();
            var apiKey = guidUtils.CreateNewId();
            var account = Account.CreateAccount(
                newAccountDetails.EmailAddress,
                PasswordHashing.CreateHashedPassword(newAccountDetails.Password),
                accountId,
                apiKey,
                AccountType.Default
            );
            logger.LogDebug("Adding new account via DEFAULT...");
            await accountsContext.Accounts.AddAsync(account);

            var introId = account.IntroductionId;
            var ok = await accountRegistrationMaker.TryRegisterAccountAndSendEmailVerificationToken(accountId, apiKey, newAccountDetails.EmailAddress,  introId, cancellationToken);
            logger.LogDebug("Send Email result was " + (ok ? "OK" : " a FAIL"));

            if (!ok) return Credentials.CreateUnauthenticatedResponse(EmailAddressNotFound);

            var token = CreateNewJwtToken(account);
            var session = CreateNewSession(account);
            await accountsContext.Sessions.AddAsync(session);

            await accountsContext.SaveChangesAsync();
            await dashContext.SaveChangesAsync();
            return Credentials.CreateAuthenticatedResponse(session.SessionId, session.ApiKey, token, account.EmailAddress);
        }

        private bool AccountExists(string emailAddress)
        {
            var account = accountsContext.Accounts.SingleOrDefault(row => row.EmailAddress == emailAddress);
            return account != null;
        }
    }
}