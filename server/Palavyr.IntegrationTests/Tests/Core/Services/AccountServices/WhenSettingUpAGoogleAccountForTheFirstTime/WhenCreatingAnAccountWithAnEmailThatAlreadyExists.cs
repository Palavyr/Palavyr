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
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AccountServices.WhenSettingUpAGoogleAccountForTheFirstTime
{
    public class WhenCreatingAnAccountWithAnEmailThatAlreadyExists : ProPlanIntegrationFixture
    {
        public WhenCreatingAnAccountWithAnEmailThatAlreadyExists(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
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
            var customerService = Container.GetService<StripeCustomerService>();
            var emailVerificationStatus = Substitute.For<IEmailVerificationStatus>();
            emailVerificationStatus.CheckVerificationStatus(testEmail).Returns(true);

            jwtService.GenerateJwtTokenAfterAuthentication(testEmail).Returns(jwtToken);

            var fakePayload = new GoogleJsonWebSignature.Payload()
            {
                Email = testEmail
            };
            authService.ValidateGoogleTokenId(googleCredentials.OneTimeCode).Returns(fakePayload);
            newAccountUtils.GetNewAccountId().Returns(testAccount);

            var registrationMaker = Substitute.For<IAccountRegistrationMaker>();
            registrationMaker.TryRegisterAccountAndSendEmailVerificationToken(testAccount, "123", testEmail, CancellationToken.None).ReturnsForAnyArgs(true);

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
        }
    }
}