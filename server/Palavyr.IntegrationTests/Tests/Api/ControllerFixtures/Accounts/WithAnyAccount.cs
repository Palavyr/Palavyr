﻿using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Palavyr.API.Controllers.Accounts;
using Palavyr.API.Controllers.Accounts.Setup;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AccountServices;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods.ClientExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Api.ControllerFixtures.Accounts
{
    public class WithAnyAccount : RealDatabaseIntegrationFixture
    {
        public WithAnyAccount(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        private async Task<Credentials> Create(string email, string password)
        {
            var request = new CreateNewAccountRequest
            {
                EmailAddress = email,
                Password = password
            };

            var result = await Client.PostWithContent<Credentials>(CreateNewAccountDefaultController.Route, request);
            return result;
        }

        [Fact]
        public async Task ItShouldBeCreated()
        {
            var email = A.RandomTestEmail();
            var password = A.RandomId();

            var credentials = await Create(email, password);

            credentials.EmailAddress.ShouldBe(email);
            credentials.Authenticated.ShouldBeTrue();
        }

        [Fact]
        public async Task ItShouldBeDeleted()
        {
            var email = A.RandomTestEmail();
            var password = A.RandomId();

            var credentials = await Create(email, password);
            await Delete(credentials);
        }

        private async Task Delete(Credentials credentials)
        {
            var tempClient = ConfigurableClient(credentials.SessionId);
            var result = await tempClient.PostAsync(DeleteAccountController.Route, null);
            result.EnsureSuccessStatusCode();
        }


        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockEmailVerificationService>().As<IEmailVerificationService>();
            return base.CustomizeContainer(builder);
        }

        public override async Task DisposeAsync()
        {
            await Task.CompletedTask;
            Client.DefaultRequestHeaders.Remove("Authorization");
            Client.DefaultRequestHeaders.Remove(ApplicationConstants.MagicUrlStrings.SessionId);
        }
    }

    public class MockEmailVerificationService : IEmailVerificationService
    {
        public Task<bool> ConfirmEmailAddressAsync(string authToken, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<bool> SendConfirmationTokenEmail(string emailAddress, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}