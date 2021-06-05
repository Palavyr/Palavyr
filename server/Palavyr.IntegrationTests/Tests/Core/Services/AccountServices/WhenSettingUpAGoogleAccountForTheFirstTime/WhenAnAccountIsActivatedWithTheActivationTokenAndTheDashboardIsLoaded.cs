﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data.CompanyData;
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

            var emailVerificationStatus = Substitute.For<IEmailVerificationStatus>();
            emailVerificationStatus.CheckVerificationStatus(testEmail).Returns(true);
            var allowedUsers = Substitute.For<IAllowedUsers>();
            allowedUsers.IsEmailAllowedToCreateAccount(testEmail).ReturnsForAnyArgs(true);

            var accountSetupService = new AccountSetupService(
                DashContext,
                AccountsContext,
                newAccountUtils,
                logger,
                authService,
                jwtService,
                new EmailVerificationService(AccountsContext, verifyLogger, customerService, requestVerification, emailClient, guidUtils, emailVerificationStatus),
                customerService,
                new GuidUtils(),
                allowedUsers
            );

            await accountSetupService.CreateNewAccountViaGoogleAsync(googleCredentials, CancellationToken.None);
            AccountsContext.Accounts.Single(x => x.AccountId == testAccount).Active.ShouldBeFalse();

            // populate stripe id before it is set
            var ensureRoute = "configure-conversations/ensure-db-valid";
            var ensureResult = await Client.PostAsyncWithoutContent(ensureRoute);
            ensureResult.EnsureSuccessStatusCode();

            // Send token
            var route = $"account/confirmation/{testConfirmationToken}/action/setup";
            var result = await Client.PostAsyncWithoutContent(route);
            result.EnsureSuccessStatusCode();


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
}