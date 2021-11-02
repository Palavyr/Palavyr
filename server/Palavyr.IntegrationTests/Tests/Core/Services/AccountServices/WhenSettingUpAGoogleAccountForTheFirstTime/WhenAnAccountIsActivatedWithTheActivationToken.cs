using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.Setup.WelcomeEmail;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AccountServices.WhenSettingUpAGoogleAccountForTheFirstTime
{
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

            var logger = Substitute.For<ILogger<AuthService>>();

            var fakePayload = new GoogleJsonWebSignature.Payload()
            {
                Email = testEmail
            };
            var authService = Substitute.For<IAuthService>();
            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);

            var jwtService = Substitute.For<IJwtAuthenticationService>();
            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);

            var guidUtils = Substitute.For<IGuidUtils>();
            guidUtils.CreateShortenedGuid(1).Returns(testConfirmationToken);

            var newAccountUtils = Substitute.For<INewAccountUtils>();
            newAccountUtils.GetNewAccountId().Returns(testAccount);

            var emailClient = Substitute.For<ISesEmail>();
            emailClient.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                testEmail,
                EmailConstants.PalavyrSubject,
                EmailConfirmationHtml.GetConfirmationEmailBody(testEmail, testConfirmationToken),
                EmailConfirmationHtml.GetConfirmationEmailBodyText(testEmail, testConfirmationToken)
            ).Returns(true);
            var requestVerification = Substitute.For<IRequestEmailVerification>();
            var verifyLogger = Substitute.For<ILogger<EmailVerificationService>>();
            var customerService = Container.GetService<StripeCustomerService>();
            var emailVerificationStatus = Substitute.For<IEmailVerificationStatus>();
            emailVerificationStatus.CheckVerificationStatus(testEmail).Returns(true);
            var emailVerificationService = new EmailVerificationService(AccountsContext, verifyLogger, customerService, requestVerification, emailClient, guidUtils, emailVerificationStatus);

            var accessChecker = Substitute.For<IPalavyrAccessChecker>();
            var accessLogger = Substitute.For<ILogger<AccountRegistrationMaker>>();

            var registrationMaker = new AccountRegistrationMaker(accessLogger, AccountsContext, DashContext, emailVerificationService, accessChecker);
            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                customerService,
                new GuidUtils(),
                registrationMaker
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

            var cleanup = Container.GetService<IRequestEmailVerification>();
            await cleanup.DeleteEmailIdentityAsync(testEmail);
        }
    }
}