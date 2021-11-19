using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
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
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Services.AccountServices.WhenSettingUpAGoogleAccountForTheFirstTime
{
    public class WhenAnAccountIsCreatedAndTheOneTimeGoogleTokenIsNotValid : BareRealDatabaseIntegrationFixture
    {
        private string testEmail = $"{A.RandomName()}@gmail.com";

        public WhenAnAccountIsCreatedAndTheOneTimeGoogleTokenIsNotValid(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        // [Fact]
        public async Task AnUnAuthenticatedResultIsReturned()
        {
            // should check the actual test stripe account that we only have once instance of this email in the test data. Then don't forget to delete the 
            var testAccount = "Test-account-123";
            var jwtToken = "jwt-token";
            var testConfirmationToken = "123456";
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
            var guidUtils = Substitute.For<IGuidUtils>();
            var emailClient = Substitute.For<ISesEmail>();
            var customerService = Container.GetService<StripeCustomerService>();
            var emailVerificationStatus = Substitute.For<IEmailVerificationStatus>();
            emailVerificationStatus.CheckVerificationStatus(testEmail).Returns(true);

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

            var registrationMaker = Substitute.For<IAccountRegistrationMaker>();
            registrationMaker.TryRegisterAccountAndSendEmailVerificationToken(testAccount, "123", testEmail, introId, CancellationToken.None).ReturnsForAnyArgs(true);

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
            result.Message.ShouldBe("Could not validate the Google Authentication token");
            
            var cleanup = Container.GetService<IRequestEmailVerification>();
            await cleanup.DeleteEmailIdentityAsync(testEmail);
            
            var stripeCleanup = Container.GetService<StripeCustomerService>();
            var customerIds = (await stripeCleanup.GetCustomerByEmailAddress(testEmail, CancellationToken.None)).Select(x => x.Id);
            stripeCleanup.DeleteStripeTestCustomers(customerIds.ToList());
        }
    }
}