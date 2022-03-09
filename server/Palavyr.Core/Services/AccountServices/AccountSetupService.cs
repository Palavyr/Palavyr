using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Sessions;

namespace Palavyr.Core.Services.AccountServices
{
    public class AccountSetupService : IAccountSetupService
    {
        private readonly IConfigurationEntityStore<Session> sessionStore;
        private readonly IConfigurationEntityStore<Account> accountStore;

        private readonly INewAccountUtils newAccountUtils;
        private readonly ILogger<AuthService> logger;
        private readonly IJwtAuthenticationService jwtAuthService;
        private readonly IGuidUtils guidUtils;
        private readonly IAccountRegistrationMaker accountRegistrationMaker;
        private readonly IAccountIdTransport accountIdTransport;


        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string AccountAlreadyExists = "Account already exists";
        private const string EmailAddressNotFound = "Email Address Not Found";

        public AccountSetupService(
            IConfigurationEntityStore<Session> sessionStore,
            IConfigurationEntityStore<Account> accountStore,
            INewAccountUtils newAccountUtils,
            ILogger<AuthService> logger,
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
            jwtAuthService = jwtService;
            this.guidUtils = guidUtils;
            this.accountRegistrationMaker = accountRegistrationMaker;
            this.accountIdTransport = accountIdTransport;
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

        public async Task<Credentials> CreateNewAccountViaDefaultAsync(string emailAddress, string password, CancellationToken cancellationToken)
        {
            // confirm account doesn't already exist
            var accountExists = await AccountExists(emailAddress);
            if (accountExists)
            {
                logger.LogDebug($"Account for email address {emailAddress} already exists");
                return Credentials.CreateUnauthenticatedResponse(AccountAlreadyExists);
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
                apiKey,
                AccountType.Default
            );
            logger.LogDebug("Adding new account via DEFAULT...");
            await accountStore.Create(account);

            var introId = account.IntroductionId;
            var ok = await accountRegistrationMaker.TryRegisterAccountAndSendEmailVerificationToken(accountId, apiKey, emailAddress, introId, cancellationToken);
            logger.LogDebug("Send Email result was " + (ok ? "OK" : " a FAIL"));

            if (!ok) return Credentials.CreateUnauthenticatedResponse(EmailAddressNotFound);

            var token = CreateNewJwtToken(account);
            var session = CreateNewSession(account);
            await sessionStore.Create(session);

            return Credentials.CreateAuthenticatedResponse(session.SessionId, session.ApiKey, token, account.EmailAddress);
        }

        private async Task<bool> AccountExists(string emailAddress)
        {
            var account = await accountStore.Get(emailAddress, s => s.EmailAddress);
            return account != null;
        }
    }
}