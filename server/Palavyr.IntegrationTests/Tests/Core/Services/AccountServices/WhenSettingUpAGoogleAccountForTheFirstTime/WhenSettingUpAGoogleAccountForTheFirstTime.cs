#nullable enable
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.CompanyData;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AccountServices.WhenSettingUpAGoogleAccountForTheFirstTime
{
    public class WhenCreatingANewAccountAndItHasNotYetBeenActivated : BareRealDatabaseIntegrationFixture
    {
        private string testEmail = $"{A.RandomName()}@gmail.com";

        public WhenCreatingANewAccountAndItHasNotYetBeenActivated(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task EverythingLooksNormal()
        {
            var testAccount = "Test-account-123";
            var jwtToken = "jwt-token";

            var googleCredentials = new GoogleRegistrationDetails()
            {
                AccessToken = StaticGuidUtils.CreateNewId(),
                OneTimeCode = StaticGuidUtils.CreateNewId(),
                TokenId = StaticGuidUtils.CreateNewId()
            };
            var fakePayload = new GoogleJsonWebSignature.Payload()
            {
                Email = testEmail
            };

            var jwtService = Substitute.For<IJwtAuthenticationService>();
            var logger = Substitute.For<ILogger<AuthService>>();
            var emailVerificationService = Substitute.For<IEmailVerificationService>();
            var authService = Substitute.For<IAuthService>();
            var newAccountUtils = Substitute.For<INewAccountUtils>();
            var guidFinder = new GuidFinder();
            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);
            newAccountUtils.GetNewAccountId().Returns(testAccount);
            var customerService = Container.GetService<StripeCustomerService>();
            var allowedUsers = Substitute.For<IAllowedUsers>();
            allowedUsers.IsEmailAllowedToCreateAccount(testEmail).ReturnsForAnyArgs(true);

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                emailVerificationService,
                customerService,
                new GuidUtils(),
                allowedUsers
            );
            emailVerificationService.SendConfirmationTokenEmail(testEmail, testAccount, CancellationToken.None).Returns(true);
            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);

            var credentialsResult = await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            credentialsResult.Authenticated.ShouldBeTrue();
            credentialsResult.SessionId.ShouldNotBeEmpty();
            credentialsResult.ApiKey.ShouldNotBeEmpty();

            var account = (await AccountsContext.Accounts.ToListAsync(CancellationToken.None)).Single();
            account.ShouldNotBeNull();
            credentialsResult.ApiKey.ShouldBe(account.ApiKey);

            account.AccountId.ShouldBe(testAccount);
            account.AccountType.ShouldBe(AccountType.Google);
            account.Active.ShouldBeFalse();
            guidFinder.FindFirstGuidSuffix(account.ApiKey).ShouldNotBeEmpty();
            account.DefaultLocale.ShouldBe("en-AU");
            account.EmailAddress.ShouldBe(testEmail);
            account.GeneralFallbackSubject.ShouldBe("");
            account.GeneralFallbackEmailTemplate.ShouldBe("");
            account.Password.ShouldBeNull();
            account.PaymentInterval.HasValue.ShouldBeTrue();
            account.PaymentInterval?.ToString().ShouldBe("Null");
            account.PhoneNumber.ShouldBeNull();
            account.PlanType.ShouldBe(Account.PlanTypeEnum.Free);
            account.ShowSeenEnquiries.ShouldBeFalse();
            account.StripeCustomerId.ShouldBeNull();
        }
    }
}