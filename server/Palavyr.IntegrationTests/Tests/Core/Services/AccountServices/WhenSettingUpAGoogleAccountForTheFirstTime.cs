#nullable enable
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Setup.WelcomeEmail;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AccountServices
{
    
    public class WhenAnAccountIsActivatedWithTheActivationTokenAndTheDashboardIsLoaded : BareRealDatabaseIntegrationFixture
    {
        private string testEmail = $"{A.RandomName()}@gmail.com";

        public WhenAnAccountIsActivatedWithTheActivationTokenAndTheDashboardIsLoaded(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task OnlyOneStripeAccountWithThisEmailIsCreated()
        {
            // should check the actual test stripe account that we only have once instance of this email in the test data. Then don't forget to delete the 
            var testAccount = "Test-account-123";
            var jwtToken = "jwt-token";
            var testConfirmationToken = "123456";

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
            var authService = Substitute.For<IAuthService>();
            var newAccountUtils = Substitute.For<INewAccountUtils>();
            var guidUtils = Substitute.For<IGuidUtils>();
            var emailClient = Substitute.For<ISesEmail>();
            var requestVerification = Substitute.For<IRequestEmailVerification>();
            var verifyLogger = Substitute.For<ILogger<EmailVerificationService>>();
            var customerService = Container.GetService<StripeCustomerService>();
             
            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);
            guidUtils.CreateShortenedGuid(1).Returns(testConfirmationToken);

            emailClient.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                testEmail,
                EmailConstants.PalavyrSubject,
                EmailConfirmationHtml.GetConfirmationEmailBody(testEmail, testConfirmationToken),
                EmailConfirmationHtml.GetConfirmationEmailBodyText(testEmail, testConfirmationToken)
            ).Returns(true);

            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);
            newAccountUtils.GetNewAccountId().Returns(testAccount);

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                new EmailVerificationService(AccountsContext, verifyLogger, new StripeCustomerService(), requestVerification, emailClient, guidUtils),
                customerService
            );

            await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            AccountsContext.Accounts.Single(x => x.AccountId == testAccount).Active.ShouldBeFalse();

            // Send token
            var route = $"account/confirmation/{testConfirmationToken}/action/setup";
            var result = await Client.PostAsyncWithoutContent(route);
            result.EnsureSuccessStatusCode();

            var ensureRoute = "configure-conversations/ensure-db-valid";
            var ensureResult = await Client.PostAsyncWithoutContent(ensureRoute);
            ensureResult.EnsureSuccessStatusCode();

            var account = AccountsContext.Accounts.Single(x => x.AccountId == testAccount);
            await AccountsContext.Entry(account).ReloadAsync();
            account.Active.ShouldBeTrue();
            account.StripeCustomerId.ShouldNotBeEmpty();

            // confirm that only one account exists with this email address on stripe
            var customers = await customerService.ListCustomers(CancellationToken.None);
            customers.Where(x => x.Id == account.StripeCustomerId).Count().ShouldBe(1);
        }


        public override async Task DisposeAsync()
        {
            var customerService = Container.GetService<StripeCustomerService>();
            await customerService.DeleteStripeTestCustomerByEmailAddress(testEmail);
            await base.DisposeAsync();
        }
    }
    
    
    public class WhenAnEmailAddressIsSuppliedButBouncesBack : BareRealDatabaseIntegrationFixture
    {
        public WhenAnEmailAddressIsSuppliedButBouncesBack(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public async Task AnUnauthenticatedResultIsReturned()
        {
            var testEmail = IntegrationConstants.EmailAddress;
            var testAccount = IntegrationConstants.AccountId;
            var jwtToken = "jwt-token";

            var googleCredentials = new GoogleRegistrationDetails()
            {
                AccessToken = StaticGuidUtils.CreateNewId(),
                OneTimeCode = StaticGuidUtils.CreateNewId(),
                TokenId = StaticGuidUtils.CreateNewId()
            };

            var jwtService = Substitute.For<IJwtAuthenticationService>();
            var logger = Substitute.For<ILogger<AuthService>>();
            var authService = Substitute.For<IAuthService>();
            var newAccountUtils = Substitute.For<INewAccountUtils>();
            var guidUtils = Substitute.For<IGuidUtils>();
            var emailClient = Substitute.For<ISesEmail>();
            var requestVerification = Substitute.For<IRequestEmailVerification>();
            var verifyLogger = Substitute.For<ILogger<EmailVerificationService>>();
            var customerService = Container.GetService<StripeCustomerService>();

            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);

            var fakePayload = new GoogleJsonWebSignature.Payload()
            {
                Email = testEmail
            };
            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);
            newAccountUtils.GetNewAccountId().Returns(testAccount);
            requestVerification.VerifyEmailAddressAsync(testEmail).Returns(false);

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                new EmailVerificationService(AccountsContext, verifyLogger, new StripeCustomerService(), requestVerification, emailClient, guidUtils),
                customerService
            );

            var result = await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            result.Authenticated.ShouldBeFalse();
            result.Message.ShouldBe("Email Address Not Found");
        }
    }
    
    public class WhenAnAccountWithThatEmailAlreadyExists : DefaultRealDatabaseIntegrationFixture
    {
        public WhenAnAccountWithThatEmailAlreadyExists(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public async Task AnUnAuthenticatedResultIsReturned()
        {
            var testEmail = IntegrationConstants.EmailAddress;
            var testAccount = IntegrationConstants.AccountId;
            var jwtToken = "jwt-token";

            var googleCredentials = new GoogleRegistrationDetails()
            {
                AccessToken = StaticGuidUtils.CreateNewId(),
                OneTimeCode = StaticGuidUtils.CreateNewId(),
                TokenId = StaticGuidUtils.CreateNewId()
            };

            var jwtService = Substitute.For<IJwtAuthenticationService>();
            var logger = Substitute.For<ILogger<AuthService>>();
            var authService = Substitute.For<IAuthService>();
            var newAccountUtils = Substitute.For<INewAccountUtils>();
            var guidUtils = Substitute.For<IGuidUtils>();
            var emailClient = Substitute.For<ISesEmail>();
            var requestVerification = Substitute.For<IRequestEmailVerification>();
            var verifyLogger = Substitute.For<ILogger<EmailVerificationService>>();
            var customerService = Container.GetService<StripeCustomerService>();

            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);

            var fakePayload = new GoogleJsonWebSignature.Payload()
            {
                Email = testEmail
            };
            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);
            newAccountUtils.GetNewAccountId().Returns(testAccount);

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                new EmailVerificationService(AccountsContext, verifyLogger, new StripeCustomerService(), requestVerification, emailClient, guidUtils),
                customerService
            );

            var result = await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            result.Authenticated.ShouldBeFalse();
            result.Message.ShouldBe("Account already exists");
        }
    }


    public class WhenAnAccountIsCreatedAndTheOneTimeGoogleTokenIsNotValid : BareRealDatabaseIntegrationFixture
    {
        private string testEmail = $"{A.RandomName()}@gmail.com";

        public WhenAnAccountIsCreatedAndTheOneTimeGoogleTokenIsNotValid(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task AnUnAuthenticatedResultIsReturned()
        {
            // should check the actual test stripe account that we only have once instance of this email in the test data. Then don't forget to delete the 
            var testAccount = "Test-account-123";
            var jwtToken = "jwt-token";
            var testConfirmationToken = "123456";

            var googleCredentials = new GoogleRegistrationDetails()
            {
                AccessToken = StaticGuidUtils.CreateNewId(),
                OneTimeCode = StaticGuidUtils.CreateNewId(),
                TokenId = StaticGuidUtils.CreateNewId()
            };

            var jwtService = Substitute.For<IJwtAuthenticationService>();
            var logger = Substitute.For<ILogger<AuthService>>();
            var authService = Substitute.For<IAuthService>();
            var newAccountUtils = Substitute.For<INewAccountUtils>();
            var guidUtils = Substitute.For<IGuidUtils>();
            var emailClient = Substitute.For<ISesEmail>();
            var requestVerification = Substitute.For<IRequestEmailVerification>();
            var verifyLogger = Substitute.For<ILogger<EmailVerificationService>>();
            var customerService = Container.GetService<StripeCustomerService>();

            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);
            guidUtils.CreateShortenedGuid(1).Returns(testConfirmationToken);

            emailClient.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                testEmail,
                EmailConstants.PalavyrSubject,
                EmailConfirmationHtml.GetConfirmationEmailBody(testEmail, testConfirmationToken),
                EmailConfirmationHtml.GetConfirmationEmailBodyText(testEmail, testConfirmationToken)
            ).Returns(true);

            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).ReturnsNull();
            newAccountUtils.GetNewAccountId().Returns(testAccount);

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                new EmailVerificationService(AccountsContext, verifyLogger, new StripeCustomerService(), requestVerification, emailClient, guidUtils),
                customerService
            );

            var result = await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            result.Authenticated.ShouldBeFalse();
            result.Message.ShouldBe("Could not validate the Google Authentication token");
        }
    }


    public class WhenAnAccountIsActivatedWithTheActivationToken : BareRealDatabaseIntegrationFixture
    {
        private string testEmail = $"{A.RandomName()}@gmail.com";

        public WhenAnAccountIsActivatedWithTheActivationToken(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task EverythingLooksNormal()
        {
            // should check the actual test stripe account that we only have once instance of this email in the test data. Then don't forget to delete the 
            var testAccount = "Test-account-123";
            var jwtToken = "jwt-token";
            var testConfirmationToken = "123456";

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
            var authService = Substitute.For<IAuthService>();
            var newAccountUtils = Substitute.For<INewAccountUtils>();
            var guidUtils = Substitute.For<IGuidUtils>();
            var emailClient = Substitute.For<ISesEmail>();
            var requestVerification = Substitute.For<IRequestEmailVerification>();
            var verifyLogger = Substitute.For<ILogger<EmailVerificationService>>();
            var customerService = Container.GetService<StripeCustomerService>();

            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);
            guidUtils.CreateShortenedGuid(1).Returns(testConfirmationToken);

            emailClient.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                testEmail,
                EmailConstants.PalavyrSubject,
                EmailConfirmationHtml.GetConfirmationEmailBody(testEmail, testConfirmationToken),
                EmailConfirmationHtml.GetConfirmationEmailBodyText(testEmail, testConfirmationToken)
            ).Returns(true);

            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);
            newAccountUtils.GetNewAccountId().Returns(testAccount);

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                new EmailVerificationService(AccountsContext, verifyLogger, new StripeCustomerService(), requestVerification, emailClient, guidUtils),
                customerService
            );

            await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            AccountsContext.Accounts.Single(x => x.AccountId == testAccount).Active.ShouldBeFalse();

            // Send token
            var route = $"account/confirmation/{testConfirmationToken}/action/setup";
            var result = await Client.PostAsyncWithoutContent(route);
            result.StatusCode.ShouldBe(HttpStatusCode.OK);

            var account = AccountsContext.Accounts.Single(x => x.AccountId == testAccount);
            await AccountsContext.Entry(account).ReloadAsync();
            account.Active.ShouldBeTrue();
            account.StripeCustomerId.ShouldNotBeEmpty();

            // confirm that only one account exists with this email address on stripe
            var customers = await customerService.ListCustomers(CancellationToken.None);
            customers.Where(x => x.Id == account.StripeCustomerId).Count().ShouldBe(1);
        }


        public override async Task DisposeAsync()
        {
            var customerService = Container.GetService<StripeCustomerService>();
            await customerService.DeleteStripeTestCustomerByEmailAddress(testEmail);
            await base.DisposeAsync();
        }
    }

    public class WhenCreatingANewAccountAndItHasNotYetBeenActivated : BareRealDatabaseIntegrationFixture
    {
        private string testEmail = $"{A.RandomName()}@gmail.com";

        public WhenCreatingANewAccountAndItHasNotYetBeenActivated(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task WhenSettingUpAGoogleAccount_AndWeDontYetActivate_EverythingLooksNormal()
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

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                emailVerificationService,
                customerService
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