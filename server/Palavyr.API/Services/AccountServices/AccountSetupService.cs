using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.API.CommonResponseTypes;
using Palavyr.API.Controllers.Accounts.Setup.SeedData;
using Palavyr.API.Controllers.Accounts.Setup.WelcomeEmail;
using Palavyr.API.RequestTypes;
using Palavyr.API.RequestTypes.Registration;
using Palavyr.API.Services.AuthenticationServices;
using Palavyr.Common.GlobalConstants;
using Palavyr.Common.UIDUtils;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;
using Palavyr.Services.EmailService.ResponseEmailTools;

namespace Palavyr.API.Services.AccountServices
{

    public interface IAccountSetupService
    {
        Task<Credentials> CreateNewAccountViaDefaultAsync(AccountDetails newAccountDetails);
        Task<Credentials> CreateNewAccountViaGoogleAsync(GoogleRegistrationDetails registrationDetails);
    }
    
    public class AccountSetupService : IAccountSetupService
    {
        private readonly DashContext dashContext;
        private readonly AccountsContext accountsContext;
        private readonly ILogger<AuthService> logger;
        private readonly IAuthService authService;
        private readonly IJwtAuthenticationService jwtAuthService;
        private readonly ISesEmail emailClient;

        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string AccountAlreadyExists = "Account already exists";
        private const string EmailAddressNotFound = "Email Address Not Found";
        
        public AccountSetupService(
            IAuthService authService,
            DashContext dashContext,
            AccountsContext accountsContext,
            ILogger<AuthService> logger,
            IJwtAuthenticationService jwtService,
            ISesEmail emailClient
        )
        {
            this.dashContext = dashContext;
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.authService = authService;
            jwtAuthService = jwtService;
            this.emailClient = emailClient;
        }
        
        public async Task<Credentials> CreateNewAccountViaGoogleAsync(GoogleRegistrationDetails googleRegistration)
        {
            
            logger.LogDebug("Creating an account using Google registration.");
            logger.LogDebug("Attempting to authenticate the google onetime code.");

            var payload = await authService.ValidateGoogleTokenId(googleRegistration.OneTimeCode);
            if (payload == null)
            {
                logger.LogDebug("Failed to authenticate the onetime code.");
                logger.LogDebug($"OneTimeCode: {googleRegistration.OneTimeCode}");
                return Credentials.CreateUnauthenticatedResponse(CouldNotValidateGoogleAuthToken);
            }
            
            logger.LogDebug("Checking if Email already exists as non-google account.");
            if (AccountExists(payload.Email))
            {
                logger.LogDebug($"Account for email address {payload.Email} already exists");
                return Credentials.CreateUnauthenticatedResponse(AccountAlreadyExists);
            }
            
            logger.LogDebug("Creating New Account Details...");
            var accountId = NewAccountUtils.GetNewAccountId();
            var newUserId = Guid.NewGuid().ToString(); // TODO: decide how to make use of the username concept here
            var apiKey = Guid.NewGuid().ToString();
            logger.LogDebug($"New Account Details--Account: {accountId} -- user: {newUserId} -- apiKey: {apiKey}");
            
            var account = UserAccount.CreateGoogleAccount(newUserId, apiKey, payload.Email, accountId, payload.Locale);
            logger.LogDebug("Adding new account via GOOGLE...");
            await accountsContext.Accounts.AddAsync(account);
            
            var ok = await RegisterAccount(accountId, apiKey, payload.Email);
            logger.LogDebug("Send Email result was " + (ok ? "OK" : " a FAIL"));

            if (!ok) return Credentials.CreateUnauthenticatedResponse(EmailAddressNotFound);
            
            var token = CreateNewJwtToken(account);
            var session = CreateNewSession(account);
            await accountsContext.Sessions.AddAsync(session);

            await accountsContext.SaveChangesAsync();
            await dashContext.SaveChangesAsync();
            return Credentials.CreateAuthenticatedResponse(session.SessionId, session.ApiKey, token, account.EmailAddress);
        }

        private string CreateNewJwtToken(UserAccount account)
        {
            return jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
        }

        private Session CreateNewSession(UserAccount account)
        {
            var sessionId = Guid.NewGuid().ToString();
            logger.LogDebug("Attempting to create a new Session.");
            var session = Session.CreateNew(sessionId, account.AccountId, account.ApiKey);
            logger.LogDebug($"New Session created: {session.SessionId}"); 
            return session;
        }
        
        public async Task<Credentials> CreateNewAccountViaDefaultAsync(AccountDetails newAccountDetails)
        {
            // confirm account doesn't already exist
            if (AccountExists(newAccountDetails.EmailAddress))
            {
                logger.LogDebug($"Account for email address {newAccountDetails.EmailAddress} already exists");
                return Credentials.CreateUnauthenticatedResponse(AccountAlreadyExists);
            }
            
            // Add the new account
            logger.LogDebug("Creating a new account");
            var accountId = NewAccountUtils.GetNewAccountId();
            var newUserId = Guid.NewGuid().ToString(); // TODO: decide how to make use of the username concept here
            var apiKey = Guid.NewGuid().ToString();
            var account = UserAccount.CreateAccount(
                newUserId,
                newAccountDetails.EmailAddress,
                PasswordHashing.CreateHashedPassword(newAccountDetails.Password),
                accountId,
                apiKey,
                AccountType.Default
            );
            logger.LogDebug("Adding new account via DEFAULT...");
            await accountsContext.Accounts.AddAsync(account);
            var ok = await RegisterAccount(accountId, apiKey, newAccountDetails.EmailAddress);
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

        private async Task<bool> RegisterAccount(string accountId, string apiKey, string emailAddress)
        {
            
            // Add the default subscription (free with 2 areas)
            logger.LogDebug($"Add default subscription for {accountId}");
            var newSubscription = Subscription.CreateNew(accountId, apiKey, SubscriptionConstants.DefaultNumAreas);
            await accountsContext.Subscriptions.AddAsync(newSubscription);
            
            // install seed Data
            logger.LogDebug("Install new account seed data.");
            var seeData = new SeedData(accountId, emailAddress);
            await dashContext.Areas.AddRangeAsync(seeData.Areas);
            await dashContext.Groups.AddRangeAsync(seeData.Groups);
            await dashContext.WidgetPreferences.AddAsync(seeData.WidgetPreference);
            await dashContext.SelectOneFlats.AddRangeAsync(seeData.DefaultDynamicTables);
            await dashContext.DynamicTableMetas.AddRangeAsync(seeData.DefaultDynamicTableMetas);

            // prepare the account confirmation email
            logger.LogDebug("Provide an account setup confirmation token");
            var confirmationToken = Guid.NewGuid().ToString().Split("-")[0];
            await accountsContext.EmailVerifications.AddAsync(EmailVerification.CreateNew(confirmationToken, emailAddress, accountId));
            
            logger.LogDebug($"Sending emails from {EmailConstants.PalavyrMainEmailAddress}");
            var htmlBody = EmailConfirmationHTML.GetConfirmationEmailBody(emailAddress, confirmationToken);
            var textBody = EmailConfirmationHTML.GetConfirmationEmailBodyText(emailAddress, confirmationToken);

            var sendEmailOk = await emailClient.SendEmail(EmailConstants.PalavyrMainEmailAddress, emailAddress, EmailConstants.PalavyrSubject, htmlBody, textBody);
            return sendEmailOk;
        }
    }
}