#nullable enable
using System;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.AuthenticationServices
{
    public interface IAuthService
    {
        public Task<Credentials> PerformLoginAction(LoginCredentialsRequest loginCredentialsRequest);
        public Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenId(string? accessToken);
    }

    public class AuthService : IAuthService
    {
        private readonly IAccountRepository accountRepository;
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
            IAccountRepository accountRepository,
            ILogger<AuthService> logger,
            IJwtAuthenticationService jwtService,
            IConfiguration configuration
        )
        {
            this.accountRepository = accountRepository;
            this.logger = logger;

            this.configuration = configuration;
            jwtAuthService = jwtService;
        }

        private class AccountReturn
        {
            public Account? Account { get; private set; }
            public string? Message { get; private set; }

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

        public async Task<Credentials> PerformLoginAction(LoginCredentialsRequest loginCredentialsRequest)
        {
            logger.LogDebug("Requesting account using login credentials...");
            var (account, message) = await RequestAccount(loginCredentialsRequest);
            if (account == null)
            {
                logger.LogDebug("Login failed -- could not find account ");
                return Credentials.CreateUnauthenticatedResponse(message);
            }

            logger.LogDebug("Successfully authenticated using the login credentials...");
            var token = CreateNewJwtToken(account);

            logger.LogDebug("Updating the current account state (regarding subscriptions)...");
            UpdateCurrentAccountState(account);

            logger.LogDebug("Creating and adding a new session...");
            var session = await accountRepository.CreateAndAddNewSession(account);

            logger.LogDebug("Committing the new session to the DB.");
            await accountRepository.CommitChangesAsync();

            logger.LogDebug("Session saved to DB. Returning auth response.");
            return Credentials.CreateAuthenticatedResponse(
                session.SessionId,
                session.ApiKey,
                token,
                account.EmailAddress.ToLowerInvariant());
        }

        public void UpdateCurrentAccountState(Account account)
        {
            // update the current active state
            // if the current_period_end plus a few days is in the future, then active stays true
            logger.LogDebug("Updated current active state given the subscription status.");
            var periodEndWithBuffer = account.CurrentPeriodEnd.AddDays(GracePeriod); // 5 day grace period if they don't pay.
            if (account.PlanType != Account.PlanTypeEnum.Free && DateTime.Now > periodEndWithBuffer)
            {
                account.PlanType = Account.PlanTypeEnum.Free;
            }
        }

        public async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleTokenId(string? oneTimeCode)
        {
            try
            {
                logger.LogDebug("Inside the try block -- attempting to validate Google One Time Code");
                var result =
                    await GoogleJsonWebSignature.ValidateAsync(
                        oneTimeCode,
                        new GoogleJsonWebSignature.ValidationSettings());
                if (result == null) logger.LogError("RESULT WAS NULL");
                return result;
            }
            catch (InvalidJwtException)
            {
                logger.LogDebug("Failed to validate the Google One Time Token - Invalid JWT Token!");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogDebug($"UNKNOWN EXCEPTION THROWN: {ex.Message}");
                return null;
            }
        }

        private async Task<AccountReturn> RequestAccount(LoginCredentialsRequest loginCredentialsRequest)
        {
            logger.LogDebug("Determining login type...");
            var loginType = DetermineLoginType(loginCredentialsRequest);

            logger.LogDebug($"Login type found to be: {loginType}");
            return loginType switch
            {
                (LoginType.Default) => await RequestAccountViaDefault(loginCredentialsRequest),
                (LoginType.Google) => await RequestAccountViaGoogle(loginCredentialsRequest),
                LoginType.Error => AccountReturn.Return(null, null),
                _ => AccountReturn.Return(null, message: null)
            };
        }

        private async Task<AccountReturn> RequestAccountViaGoogle(LoginCredentialsRequest credentialRequest)
        {
            logger.LogDebug("Requesting account via Google...");
            var payload = await ValidateGoogleTokenId(credentialRequest.OneTimeCode);
            if (payload == null)
            {
                logger.LogError(CouldNotValidateGoogleAuthToken);
                logger.LogError($"OneTimeCode: {credentialRequest.OneTimeCode}");
                
                return AccountReturn.Return(null, CouldNotValidateGoogleAuthToken);
            }

            if (payload.Subject != credentialRequest.TokenId)
            {
                return AccountReturn.Return(null, CouldNotValidateGoogleAuthToken);
            }

            // now verify the user exists in the Accounts database
            var account = await accountRepository.GetAccountByEmailOrNull(payload.Email);
            if (account == null)
            {
                return AccountReturn.Return(null, CouldNotFindAccountWithGoogle);
            }

            if (account.AccountType != AccountType.Google)
                return AccountReturn.Return(null, "Google " + DifferentAccountType);

            return AccountReturn.Return(account, null);
        }

        private async Task<AccountReturn> RequestAccountViaDefault(LoginCredentialsRequest credentialsRequest)
        {
            var account = await accountRepository.GetAccountByEmailOrNull(credentialsRequest.EmailAddress.ToLowerInvariant());
            if (account == null)
            {
                return AccountReturn.Return(account, CouldNotFindAccount);
            }

            if (account.AccountType != AccountType.Default)
                return AccountReturn.Return(null, "Default " + DifferentAccountType);


            if (!PasswordHashing.ComparePasswords(account.Password, credentialsRequest.Password))
            {
                logger.LogDebug("The provided password did not match!");
                return AccountReturn.Return(null, PasswordsDoNotMatch);
            }

            return AccountReturn.Return(account, null);
        }

        private string CreateNewJwtToken(Account account)
        {
            logger.LogDebug("Calling the Jwt Token Service...");
            return jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
        }

        private LoginType DetermineLoginType(LoginCredentialsRequest loginCredentialsRequest)
        {
            if (!string.IsNullOrWhiteSpace(loginCredentialsRequest.OneTimeCode) &&
                !string.IsNullOrWhiteSpace(loginCredentialsRequest.TokenId))
                return LoginType.Google;
            if (!string.IsNullOrWhiteSpace(loginCredentialsRequest.EmailAddress) &&
                !string.IsNullOrWhiteSpace(loginCredentialsRequest.Password))
                return LoginType.Default;
            logger.LogDebug("Error: Could not determine login type.");
            return LoginType.Error;
        }
    }
}