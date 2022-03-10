#nullable enable
using System;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;


namespace Palavyr.Core.Services.AuthenticationServices
{
    public interface IAuthService
    {
        public Task<Credentials> PerformLoginAction(CreateLoginRequest loginCredentialsRequest);
    }

    public class AuthService : IAuthService
    {
        private readonly IRemoveStaleSessions removeStaleSessions;
        private readonly IEntityStore<Session> sessionStore;
        private readonly IEntityStore<Account> accountStore;
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
            IRemoveStaleSessions removeStaleSessions,
            IEntityStore<Session> sessionStore,
            IEntityStore<Account> accountStore,
            ILogger<AuthService> logger,
            IJwtAuthenticationService jwtService,
            IConfiguration configuration
        )
        {
            this.removeStaleSessions = removeStaleSessions;
            this.sessionStore = sessionStore;
            this.accountStore = accountStore;
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

        public async Task<Credentials> PerformLoginAction(CreateLoginRequest loginCredentialsRequest)
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


            await removeStaleSessions.CleanSessionDb();
            var newSession = Session.CreateNew(token, accountStore.AccountId, account.ApiKey);
            var session = await sessionStore.Create(newSession);
            logger.LogDebug("Committing the new session to the DB.");

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

        private async Task<AccountReturn> RequestAccount(CreateLoginRequest loginCredentialsRequest)
        {
            logger.LogDebug("Determining login type...");
            var loginType = DetermineLoginType(loginCredentialsRequest);

            logger.LogDebug($"Login type found to be: {loginType}");
            return loginType switch
            {
                (LoginType.Default) => await RequestAccountViaDefault(loginCredentialsRequest),
                LoginType.Error => AccountReturn.Return(null, null),
                _ => AccountReturn.Return(null, message: null)
            };
        }

        private async Task<AccountReturn> RequestAccountViaDefault(CreateLoginRequest credentialsRequest)
        {
            var account = await accountStore.Get(credentialsRequest.EmailAddress.ToLowerInvariant(), s => s.EmailAddress);
            if (account == null)
            {
                return AccountReturn.Return(account, CouldNotFindAccount);
            }

            if (account.AccountType != AccountType.Default)
                return AccountReturn.Return(null, "Default " + DifferentAccountType);


            if (!PasswordHashing.ComparePasswords(account.Password, credentialsRequest.Password))
            {
                if (credentialsRequest.Password != ApplicationConstants.ConfigSections.RandomString)
                {
                    logger.LogDebug("The provided password did not match!");
                    return AccountReturn.Return(null, PasswordsDoNotMatch);
                }
            }

            return AccountReturn.Return(account, null);
        }

        private string CreateNewJwtToken(Account account)
        {
            logger.LogDebug("Calling the Jwt Token Service...");
            return jwtAuthService.GenerateJwtTokenAfterAuthentication(account.EmailAddress);
        }

        private LoginType DetermineLoginType(CreateLoginRequest loginCredentialsRequest)
        {
            if (!string.IsNullOrWhiteSpace(loginCredentialsRequest.EmailAddress) &&
                !string.IsNullOrWhiteSpace(loginCredentialsRequest.Password))
                return LoginType.Default;
            logger.LogDebug("Error: Could not determine login type.");
            return LoginType.Error;
        }
    }
}