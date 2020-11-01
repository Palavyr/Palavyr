using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService;
using EmailService.verification;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.controllers.accounts.seedData;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.Constants;
using Palavyr.Common.uniqueIdentifiers;
using Server.Domain.AccountDB;
using Server.Domain.Accounts;

namespace Palavyr.API.controllers.accounts.newAccount
{

    public interface IAccountSetupService
    {
        Task<Credentials> CreateNewAccountViaDefaultAsync(AccountDetails newAccountDetails);
        Task<Credentials> CreateNewAccountViaGoogleAsync(GoogleRegistrationDetails registrationDetails);
    }
    
    public class AccountSetupService : IAccountSetupService
    {
        private readonly DashContext _dashContext;
        private readonly AccountsContext _accountsContext;
        private readonly ILogger<AuthService> _logger;
        private readonly IAuthService _authService;
        private SenderVerification Verifier { get; set; }
        private SESEmail Client { get; set; }
        private readonly IJwtAuthenticationService _jwtAuthService;

        private const string CouldNotFindAccount = "Could not find Account";
        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string PasswordsDoNotMatch = "Password does not match.";
        private const string DifferentAccountType = " (Email is currently used with different account type).";
        private const string AccountAlreadyExists = "Account already exists";
        private const string EmailAddressNotFound = "Email Address Not Found";
        
        public AccountSetupService(
            IAuthService authService,
            DashContext dashContext,
            AccountsContext accountsContext,
            ILogger<AuthService> logger,
            IAmazonSimpleEmailService SESClient,
            IJwtAuthenticationService jwtService
        )
        {
            _dashContext = dashContext;
            _accountsContext = accountsContext;
            _logger = logger;
            _authService = authService;
            _jwtAuthService = jwtService;
            Client = new SESEmail(logger, SESClient);
            Verifier = new SenderVerification(logger, SESClient);
        }
        
        public async Task<Credentials> CreateNewAccountViaGoogleAsync(GoogleRegistrationDetails googleRegistration)
        {
            
            _logger.LogDebug("Creating an account using Google registration.");
            _logger.LogDebug("Attempting to authenticate the google onetime code.");

            var payload = await _authService.ValidateGoogleTokenId(googleRegistration.OneTimeCode);
            if (payload == null)
            {
                _logger.LogDebug("Failed to authenticate the onetime code.");
                _logger.LogDebug($"OneTimeCode: {googleRegistration.OneTimeCode}");
                return Credentials.CreateUnauthenticatedResponse(CouldNotValidateGoogleAuthToken);
            }
            
            _logger.LogDebug("Checking if Email already exists as non-google account.");
            if (AccountExists(payload.Email))
            {
                _logger.LogDebug($"Account for email address {payload.Email} already exists");
                return Credentials.CreateUnauthenticatedResponse(AccountAlreadyExists);
            }
            
            _logger.LogDebug("Creating New Account Details...");
            var accountId = NewAccountUtils.GetNewAccountId();
            var newUserId = Guid.NewGuid().ToString(); // TODO: decide how to make use of the username concept here
            var apiKey = Guid.NewGuid().ToString();
            _logger.LogDebug($"New Account Details--Account: {accountId} -- user: {newUserId} -- apiKey: {apiKey}");
            
            var account = UserAccount.CreateGoogleAccount(newUserId, apiKey, payload.Email, accountId, payload.Locale);
            _logger.LogDebug("Adding new account via GOOGLE...");
            await _accountsContext.Accounts.AddAsync(account);
            
            var ok = await RegisterAccount(accountId, apiKey, payload.Email);
            _logger.LogDebug("Send Email result was " + (ok ? "OK" : " a FAIL"));

            if (!ok) return Credentials.CreateUnauthenticatedResponse(EmailAddressNotFound);
            
            var token = CreateNewJwtToken(account);
            var session = CreateNewSession(account);
            await _accountsContext.Sessions.AddAsync(session);

            await _accountsContext.SaveChangesAsync();
            await _dashContext.SaveChangesAsync();
            return Credentials.CreateAuthenticatedResponse(session.SessionId, session.ApiKey, token, account.EmailAddress);
        }

        private string CreateNewJwtToken(UserAccount account)
        {
            return _jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
        }

        private Session CreateNewSession(UserAccount account)
        {
            var sessionId = Guid.NewGuid().ToString();
            _logger.LogDebug("Attempting to create a new Session.");
            var session = Session.CreateNew(sessionId, account.AccountId, account.ApiKey);
            _logger.LogDebug($"New Session created: {session.SessionId}"); 
            return session;
        }
        
        public async Task<Credentials> CreateNewAccountViaDefaultAsync(AccountDetails newAccountDetails)
        {
            // confirm account doesn't already exist
            if (AccountExists(newAccountDetails.EmailAddress))
            {
                _logger.LogDebug($"Account for email address {newAccountDetails.EmailAddress} already exists");
                return Credentials.CreateUnauthenticatedResponse(AccountAlreadyExists);
            }
            
            // Add the new account
            _logger.LogDebug("Creating a new account");
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
            _logger.LogDebug("Adding new account via DEFAULT...");
            await _accountsContext.Accounts.AddAsync(account);
            var ok = await RegisterAccount(accountId, apiKey, newAccountDetails.EmailAddress);
            _logger.LogDebug("Send Email result was " + (ok ? "OK" : " a FAIL"));

            if (!ok) return Credentials.CreateUnauthenticatedResponse(EmailAddressNotFound);
            
            var token = CreateNewJwtToken(account);
            var session = CreateNewSession(account);
            await _accountsContext.Sessions.AddAsync(session);

            await _accountsContext.SaveChangesAsync();
            await _dashContext.SaveChangesAsync();
            return Credentials.CreateAuthenticatedResponse(session.SessionId, session.ApiKey, token, account.EmailAddress);
        }

        private bool AccountExists(string emailAddress)
        {
            var account = _accountsContext.Accounts.SingleOrDefault(row => row.EmailAddress == emailAddress);
            return account != null;
        }

        private async Task<bool> RegisterAccount(string accountId, string apiKey, string emailAddress)
        {
            
            // Add the default subscription (free with 2 areas)
            _logger.LogDebug($"Add default subscription for {accountId}");
            var newSubscription = Subscription.CreateNew(accountId, apiKey, SubscriptionConstants.DefaultNumAreas);
            await _accountsContext.Subscriptions.AddAsync(newSubscription);
            
            // install seed Data
            _logger.LogDebug("Install new account seed data.");
            var seeData = new SeedData(accountId, emailAddress);
            await _dashContext.Areas.AddRangeAsync(seeData.Areas);
            await _dashContext.Groups.AddRangeAsync(seeData.Groups);
            await _dashContext.WidgetPreferences.AddAsync(seeData.WidgetPreference);
            await _dashContext.SelectOneFlats.AddRangeAsync(seeData.DefaultDynamicTables);
            await _dashContext.DynamicTableMetas.AddRangeAsync(seeData.DefaultDynamicTableMetas);

            // prepare the account confirmation email
            _logger.LogDebug("Provide an account setup confirmation token");
            var confirmationToken = Guid.NewGuid().ToString().Split("-")[0];
            await _accountsContext.EmailVerifications.AddAsync(EmailVerification.CreateNew(confirmationToken, emailAddress, accountId));
            
            _logger.LogDebug($"Sending emails from {EmailConstants.PalavyrMain}");
            var htmlBody = EmailConfirmationHTML.GetConfirmationEmailBody(emailAddress, confirmationToken);
            var textBody = EmailConfirmationHTML.GetConfirmationEmailBodyText(emailAddress, confirmationToken);

            var sendEmailOk = await Client.SendEmail(EmailConstants.PalavyrMain, emailAddress, EmailConstants.PalavyrSubject, htmlBody, textBody);
            return sendEmailOk;
        }
    }
}