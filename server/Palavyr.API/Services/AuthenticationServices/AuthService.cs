#nullable enable
using System;
using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.API.RequestTypes;
using Palavyr.API.ResponseTypes;
using Palavyr.FileSystem.UIDUtils;
using Server.Domain.Accounts;

namespace Palavyr.API.Services.AuthenticationServices
{
    public interface IAuthService
    {
        public Task<Credentials> PerformLoginAction(LoginCredentials loginCredentials);
        public Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenId(string accessToken);
    }

    public class AuthService : IAuthService
    {
        private DashContext _dashContext;
        private readonly AccountsContext _accountsContext;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtAuthenticationService _jwtAuthService;
        private IConfiguration _configuration;

        private const string CouldNotFindAccount = "Could not find Account";
        private const string PasswordsDoNotMatch = "Password does not match.";
        private const string CouldNotFindAccountWithGoogle = "Could not find Account with Google";
        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string DifferentAccountType = "Email is currently used with different account type.";
        private const int GracePeriod = 5;
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

            public void Deconstruct(out UserAccount? account, out string? message)
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

        public async Task<Credentials> PerformLoginAction(LoginCredentials loginCredentials)
        {
            var (account, message) = await RequestAccount(loginCredentials);
            if (account == null)
            {
                return Credentials.CreateUnauthenticatedResponse(message);
            }

            var session = CreateNewSession(account);
            var token = CreateNewJwtToken(account);
            UpdateCurrentAccountState(account);

            await _accountsContext.Sessions.AddAsync(session);
            
            await _accountsContext.SaveChangesAsync();

            _logger.LogDebug("Session saved to DB. Returning auth response.");
            return Credentials.CreateAuthenticatedResponse(
                session.SessionId,
                session.ApiKey,
                token,
                account.EmailAddress);
        }

        public async Task UpdateCurrentAccountState(UserAccount account)
        {
            // update the current active state
            // if the current_period_end plus a few days is in the future, then active stays true
            _logger.LogDebug("Updated current active state given the subscription status.");
            var periodEndWithBuffer = account.CurrentPeriodEnd.AddDays(GracePeriod); // 5 day grace period if they don't pay.
            if (DateTime.Now > periodEndWithBuffer && account.PlanType != UserAccount.PlanTypeEnum.Free)
            {
                account.Active = false;
            }
            else
            {
                account.Active = true;
            }
        }

        public async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenId(string oneTimeCode)
        {
            try
            {
                _logger.LogDebug("Inside the try block -- attempting to validation One Time Code");
                var result =
                    await GoogleJsonWebSignature.ValidateAsync(
                        oneTimeCode,
                        new GoogleJsonWebSignature.ValidationSettings());
                return result;
            }
            catch (InvalidJwtException)
            {
                _logger.LogDebug("EXCEPTION - Invalid JWT Token!");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"UNKNOWN EXCEPTION THROWN: {ex.Message}");
                return null;
            }
        }

        private async Task<AccountReturn> RequestAccount(LoginCredentials loginCredentials)
        {
            var loginType = DetermineLoginType(loginCredentials);
            return loginType switch
            {
                (LoginType.Default) => RequestAccountViaDefault(loginCredentials),
                (LoginType.Google) => await RequestAccountViaGoogle(loginCredentials),
                LoginType.Error => AccountReturn.Return(null, null),
                _ => AccountReturn.Return(null, message: null)
            };
        }

        private async Task<AccountReturn> RequestAccountViaGoogle(LoginCredentials credential)
        {
            var payload = await ValidateGoogleTokenId(credential.OneTimeCode);
            if (payload == null)
                return AccountReturn.Return(null, CouldNotValidateGoogleAuthToken);

            if (payload.Subject != credential.TokenId)
            {
                return AccountReturn.Return(null, CouldNotValidateGoogleAuthToken);
            }

            // now verify the user exists in the Accounts database
            var account = _accountsContext.Accounts.SingleOrDefault(row => row.EmailAddress == payload.Email);
            if (account == null)
            {
                return AccountReturn.Return(null, CouldNotFindAccountWithGoogle);
            }

            if (account.AccountType != AccountType.Google)
                return AccountReturn.Return(null, "Google " + DifferentAccountType);

            return AccountReturn.Return(account, null);
        }

        private AccountReturn RequestAccountViaDefault(LoginCredentials credentials)
        {
            _logger.LogDebug("Attempting to retrieve credentials.");
            var byUsername = _accountsContext.Accounts.SingleOrDefault(row => row.UserName == credentials.Username);
            var byEmail =
                _accountsContext.Accounts.SingleOrDefault(row => row.EmailAddress == credentials.EmailAddress);

            var userAccount = byUsername ?? byEmail;
            if (userAccount == null)
            {
                return AccountReturn.Return(userAccount, CouldNotFindAccount);
            }

            if (userAccount.AccountType != AccountType.Default)
                return AccountReturn.Return(null, "Default " + DifferentAccountType);


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
            if (!string.IsNullOrWhiteSpace(loginCredentials.OneTimeCode) &&
                !string.IsNullOrWhiteSpace(loginCredentials.TokenId))
                return LoginType.Google;
            if (!string.IsNullOrWhiteSpace(loginCredentials.EmailAddress) &&
                !string.IsNullOrWhiteSpace(loginCredentials.Password))
                return LoginType.Default;
            return LoginType.Error;
        }
    }
}