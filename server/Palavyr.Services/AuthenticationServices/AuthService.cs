#nullable enable
using System;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Common.UIDUtils;
using Palavyr.Domain.Accounts.Schemas;
using Palavyr.Domain.Resources.Requests;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.DatabaseService;

namespace Palavyr.Services.AuthenticationServices
{
    public interface IAuthService
    {
        public Task<Credentials> PerformLoginAction(LoginCredentials loginCredentials);
        public Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenId(string accessToken);
    }

    public class AuthService : IAuthService
    {
        private readonly IAccountsConnector accountsConnector;
        private readonly ILogger<AuthService> logger;
        private readonly IJwtAuthenticationService jwtAuthService;
        private IConfiguration configuration;

        private const string CouldNotFindAccount = "Could not find Account";
        private const string PasswordsDoNotMatch = "Password does not match.";
        private const string CouldNotFindAccountWithGoogle = "Could not find Account with Google";
        private const string CouldNotValidateGoogleAuthToken = "Could not validate the Google Authentication token";
        private const string DifferentAccountType = "Email is currently used with different account type.";
        private const int GracePeriod = 5;
        public AuthService(
            IAccountsConnector accountsConnector,
            ILogger<AuthService> logger,
            IJwtAuthenticationService jwtService,
            IConfiguration configuration
        )
        {
            this.accountsConnector = accountsConnector;
            this.logger = logger;

            this.configuration = configuration;
            jwtAuthService = jwtService;
        }

        private class AccountReturn
        {
            public Account Account { get; private set; }
            public string Message { get; private set; }

            public static AccountReturn Return(Account? account, string? message)
            {
                return new AccountReturn()
                {
                    Account = account,
                    Message = message
                };
            }

            public void Deconstruct(out Account? account, out string? message)
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

            var token = CreateNewJwtToken(account);
            UpdateCurrentAccountState(account);

            var session = await accountsConnector.CreateAndAddNewSession(account);
            
            await accountsConnector.CommitChangesAsync();

            logger.LogDebug("Session saved to DB. Returning auth response.");
            return Credentials.CreateAuthenticatedResponse(
                session.SessionId,
                session.ApiKey,
                token,
                account.EmailAddress);
        }

        public void UpdateCurrentAccountState(Account account)
        {
            // update the current active state
            // if the current_period_end plus a few days is in the future, then active stays true
            logger.LogDebug("Updated current active state given the subscription status.");
            var periodEndWithBuffer = account.CurrentPeriodEnd.AddDays(GracePeriod); // 5 day grace period if they don't pay.
            if (DateTime.Now > periodEndWithBuffer && account.PlanType != Account.PlanTypeEnum.Free)
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
                logger.LogDebug("Inside the try block -- attempting to validation One Time Code");
                var result =
                    await GoogleJsonWebSignature.ValidateAsync(
                        oneTimeCode,
                        new GoogleJsonWebSignature.ValidationSettings());
                return result;
            }
            catch (InvalidJwtException)
            {
                logger.LogDebug("EXCEPTION - Invalid JWT Token!");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"UNKNOWN EXCEPTION THROWN: {ex.Message}");
                return null;
            }
        }

        private async Task<AccountReturn> RequestAccount(LoginCredentials loginCredentials)
        {
            var loginType = DetermineLoginType(loginCredentials);
            return loginType switch
            {
                (LoginType.Default) => await RequestAccountViaDefault(loginCredentials),
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
            var account = await accountsConnector.GetAccountByEmailOrNull(payload.Email);
            if (account == null)
            {
                return AccountReturn.Return(null, CouldNotFindAccountWithGoogle);
            }

            if (account.AccountType != AccountType.Google)
                return AccountReturn.Return(null, "Google " + DifferentAccountType);

            return AccountReturn.Return(account, null);
        }

        private async Task<AccountReturn> RequestAccountViaDefault(LoginCredentials credentials)
        {
            var account = await accountsConnector.GetAccountByEmailOrNull(credentials.EmailAddress);
            if (account == null)
            {
                return AccountReturn.Return(account, CouldNotFindAccount);
            }

            if (account.AccountType != AccountType.Default)
                return AccountReturn.Return(null, "Default " + DifferentAccountType);


            if (!PasswordHashing.ComparePasswords(account.Password, credentials.Password))
            {
                logger.LogDebug("The provided password did not match!");
                return AccountReturn.Return(null, PasswordsDoNotMatch);
            }

            return AccountReturn.Return(account, null);
        }
        
        private string CreateNewJwtToken(Account account)
        {
            return jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
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