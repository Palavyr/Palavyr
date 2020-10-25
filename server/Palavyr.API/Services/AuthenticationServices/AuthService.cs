#nullable enable
using System;
using System.Linq;
using DashboardServer.Data;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.API.ReceiverTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.Common.uniqueIdentifiers;
using Server.Domain.Accounts;

namespace Palavyr.API.Controllers
{
    public interface IAuthService
    {
        public Credentials PerformLoginAction(LoginCredentials loginCredentials);
        public GoogleJsonWebSignature.Payload? ValidateGoogleTokenId(string accessToken);
    }

    public class AuthService : IAuthService
    {
        private DashContext _dashContext;
        private readonly AccountsContext _accountsContext;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtAuthenticationService _jwtAuthService;
        private IConfiguration _configuration;

        private const string CouldNotFindAccount = "Could not find Account";
        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string PasswordsDoNotMatch = "Password does not match.";
        private const string DifferentAccountType = " (Email is currently used with different account type).";
        public AuthService(
            DashContext dashContext,
            AccountsContext accountsContext,
            ILogger<AuthService> logger,
            IJwtAuthenticationService jwtService,
            IConfiguration configuration
        )
        {
            _dashContext = dashContext;
            _accountsContext = accountsContext;
            _logger = logger;
            
            _configuration = configuration;
            _jwtAuthService = jwtService;
        }

        private class AccountReturn
        {
            public UserAccount Account { get; private set; }
            public string Message { get; private set; }

            public static AccountReturn Return(UserAccount? account, string? message)
            {
                return new AccountReturn()
                {
                    Account = account,
                    Message = message
                };
            }

            public void Deconstruct(out UserAccount account, out string message)
            {
                account = Account;
                message = Message;
            }
        }

        private enum LoginType
        {
            Default,
            Google,
            Error
        }

        public enum ErrorType
        {
            CouldNotFindAccount,
            CouldNot
        }

        public Credentials PerformLoginAction(LoginCredentials loginCredentials)
        {
            var (account, message) = RequestAccount(loginCredentials);
            if (account == null)
                return Credentials.CreateUnauthenticatedResponse(message);

            var session = CreateNewSession(account);
            var token = CreateNewJwtToken(account);
            
            _accountsContext.Sessions.Add(session);
            _accountsContext.SaveChanges();

            _logger.LogDebug("Session saved to DB. Returning auth response.");
            return Credentials.CreateAuthenticatedResponse(session.SessionId, session.ApiKey, token, account.EmailAddress);
        }
        

        public GoogleJsonWebSignature.Payload? ValidateGoogleTokenId(string oneTimeCode)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                return GoogleJsonWebSignature
                    .ValidateAsync(oneTimeCode, new GoogleJsonWebSignature.ValidationSettings()).Result;
            }
            catch (InvalidJwtException)
            {
                return null;
            }
            catch (Exception ex)
            {
                
                return null;
            }
        }

        private AccountReturn RequestAccount(LoginCredentials loginCredentials)
        {
            var loginType = DetermineLoginType(loginCredentials);
            return loginType switch
            {
                (LoginType.Default) => RequestAccountViaDefault(loginCredentials),
                (LoginType.Google) => RequestAccountViaGoogle(loginCredentials),
                LoginType.Error => AccountReturn.Return(null, null),
                _ => AccountReturn.Return(null, message: null)
            };
        }

        private AccountReturn RequestAccountViaGoogle(LoginCredentials credential)
        {
            var payload = ValidateGoogleTokenId(credential.OneTimeCode);
            if (payload == null)
                return AccountReturn.Return(null, CouldNotValidateGoogleAuthToken);

            // compare incoming Accesstoken and tokenId to the ones returned by google
            
            
            // now verify the user exists in the Accounts database
            var account = _accountsContext.Accounts.SingleOrDefault(row => row.EmailAddress == payload.Email);
            if (account.AccountType != AccountType.Google)
                return AccountReturn.Return(null, CouldNotFindAccount + DifferentAccountType);
            var message = (account == null) ? null : CouldNotFindAccount;
            return AccountReturn.Return(account, message);
        }

        private AccountReturn RequestAccountViaDefault(LoginCredentials credentials)
        {
            _logger.LogDebug("Attempting to retrieve credentials.");
            var byUsername = _accountsContext.Accounts.SingleOrDefault(row => row.UserName == credentials.Username);
            var byEmail =
                _accountsContext.Accounts.SingleOrDefault(row => row.EmailAddress == credentials.EmailAddress);

            var userAccount = byUsername ?? byEmail;
            
            if (userAccount.AccountType != AccountType.Default)
                return AccountReturn.Return(null, CouldNotFindAccount + DifferentAccountType);
            
            if (userAccount == null)
            {
                return AccountReturn.Return(userAccount, CouldNotFindAccount);
            }
            if (!PasswordHashing.ComparePasswords(userAccount.Password, credentials.Password))
            {
                _logger.LogDebug("The provided password did not match!");
                return AccountReturn.Return(null, PasswordsDoNotMatch);
            }
            
            return AccountReturn.Return(userAccount, null);
        }

        private Session CreateNewSession(UserAccount account)
        {

            var sessionId = Guid.NewGuid().ToString();
            _logger.LogDebug("Attempting to create a new Session.");
            var newSession = Session.CreateNew(sessionId, account.AccountId, account.ApiKey);
            
            _logger.LogDebug($"New Session created: {newSession.SessionId}");
            return newSession;
  
        }

        private string CreateNewJwtToken(UserAccount account)
        {
            return _jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
        }
        
        private static LoginType DetermineLoginType(LoginCredentials loginCredentials)
        {
            if (!string.IsNullOrWhiteSpace(loginCredentials.AccessToken) && !string.IsNullOrWhiteSpace(loginCredentials.OneTimeCode) && !string.IsNullOrWhiteSpace(loginCredentials.TokenId))
                return LoginType.Google;
            if (!string.IsNullOrWhiteSpace(loginCredentials.EmailAddress) && !string.IsNullOrWhiteSpace(loginCredentials.Password))
                return LoginType.Default;
            return LoginType.Error;
        }
    }
}