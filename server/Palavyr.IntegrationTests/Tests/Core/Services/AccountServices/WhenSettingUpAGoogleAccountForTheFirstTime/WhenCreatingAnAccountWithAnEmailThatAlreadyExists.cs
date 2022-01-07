using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Models.Resources.Requests;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AccountServices.WhenSettingUpAGoogleAccountForTheFirstTime
{
    public class WhenCreatingAnAccountWithAnEmailThatAlreadyExists : RealDatabaseIntegrationFixture
    {
        public WhenCreatingAnAccountWithAnEmailThatAlreadyExists(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await this.SetupProAccount();
        }

        [Fact]
        public async Task AnUnAuthenticatedResultIsReturned()
        {
            var testEmail = IntegrationConstants.EmailAddress;
            var jwtToken = "jwt-token";
            var introId = "24323";

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
            var customerService = Container.GetService<StripeCustomerService>();
            var emailVerificationStatus = Substitute.For<IEmailVerificationStatus>();
            emailVerificationStatus.CheckVerificationStatus(testEmail).Returns(true);

            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);

            var fakePayload = new GoogleJsonWebSignature.Payload()
            {
                Email = testEmail
            };
            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);
            newAccountUtils.GetNewAccountId().Returns(AccountId);

            var registrationMaker = Substitute.For<IAccountRegistrationMaker>();
            registrationMaker.TryRegisterAccountAndSendEmailVerificationToken(AccountId, "123", testEmail, introId, CancellationToken.None).ReturnsForAnyArgs(true);

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

            var result = await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            result.Authenticated.ShouldBeFalse();
            result.Message.ShouldBe("Account already exists");

            var cleanup = Container.GetService<IRequestEmailVerification>();
            await cleanup.DeleteEmailIdentityAsync(testEmail);

            var stripeCleanup = Container.GetService<StripeCustomerService>();
            var customerIds = (await stripeCleanup.GetCustomerByEmailAddress(testEmail, CancellationToken.None)).Select(x => x.Id);
            await stripeCleanup.DeleteStripeTestCustomers(customerIds.ToList());

        }
    }
}