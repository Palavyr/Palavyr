using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;

namespace Palavyr.Core.Services.AccountServices
{
    public class AccountSetupService : IAccountSetupService
    {
        private readonly IEntityStore<UserSession> sessionStore;
        private readonly IEntityStore<Account> accountStore;

        private readonly INewAccountUtils newAccountUtils;
        private readonly ILogger<AccountSetupService> logger;
        private readonly IJwtAuthenticationService jwtAuthService;
        private readonly IGuidUtils guidUtils;
        private readonly IAccountRegistrationMaker accountRegistrationMaker;
        private readonly IAccountIdTransport accountIdTransport;


        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string AccountAlreadyExists = "Account already exists";
        private const string EmailAddressNotFound = "Email Address Not Found";

        public AccountSetupService(
            IEntityStore<UserSession> sessionStore,
            IEntityStore<Account> accountStore,
            INewAccountUtils newAccountUtils,
            ILogger<AccountSetupService> logger,
            IJwtAuthenticationService jwtService,
            IGuidUtils guidUtils,
            IAccountRegistrationMaker accountRegistrationMaker,
            IAccountIdTransport accountIdTransport
        )
        {
            this.sessionStore = sessionStore;
            this.accountStore = accountStore;
            this.newAccountUtils = newAccountUtils;
            this.logger = logger;
            this.jwtAuthService = jwtService;
            this.guidUtils = guidUtils;
            this.accountRegistrationMaker = accountRegistrationMaker;
            this.accountIdTransport = accountIdTransport;
        }

        private string CreateNewJwtToken(Account account)
        {
            return jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
        }

        private UserSession CreateNewSession(Account account)
        {
            var sessionId = guidUtils.CreateNewId();
            logger.LogDebug("Attempting to create a new Session.");
            var session = UserSession.CreateNew(sessionId, account.AccountId, account.ApiKey);
            logger.LogDebug($"New Session created: {session.SessionId}");
            return session;
        }

        public async Task<CredentialsResource> CreateNewAccount(string emailAddress, string password, CancellationToken cancellationToken)
        {
            // confirm account doesn't already exist
            var accountExists = await AccountExists(emailAddress);
            if (accountExists)
            {
                logger.LogDebug("Account for email address {EmailAddress} already exists", emailAddress);
                return CredentialsResource.CreateUnauthenticatedResponse(AccountAlreadyExists);
            }

            // Add the new account
            logger.LogDebug("Creating a new account");
            var accountId = newAccountUtils.GetNewAccountId();
            accountIdTransport.Assign(accountId);

            var apiKey = guidUtils.CreateNewId();
            var account = Account.CreateAccount(
                emailAddress,
                PasswordHashing.CreateHashedPassword(password),
                accountId,
                apiKey
            );
            logger.LogDebug("Adding new account via DEFAULT...");
            await accountStore.Create(account);

            var introId = account.IntroIntentId;
            var ok = await accountRegistrationMaker.TryRegisterAccountAndSendEmailVerificationToken(accountId, apiKey, emailAddress, introId, cancellationToken);
            logger.LogDebug("Send Email result was " + (ok ? "OK" : " a FAIL"));

            if (!ok) return CredentialsResource.CreateUnauthenticatedResponse(EmailAddressNotFound);

            var token = CreateNewJwtToken(account);
            var session = CreateNewSession(account);
            await sessionStore.Create(session);

            return CredentialsResource.CreateAuthenticatedResponse(session.SessionId, session.ApiKey, token, account.EmailAddress);
        }

        private async Task<bool> AccountExists(string emailAddress)
        {
            var account = await accountStore.RawReadonlyQuery().SingleOrDefaultAsync(s => emailAddress == s.EmailAddress);
            return !(account is null);
        }
    }
}