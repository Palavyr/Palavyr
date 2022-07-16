﻿using System.Threading.Tasks;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.GlobalConstants;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests.Api.ControllerFixtures.Accounts
{
    public class WithAnyAccount : IntegrationTest<DbTypes.Real>
    {
        public WithAnyAccount(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        private async Task<Credentials> Create(string email, string password)
        {
            var request = new CreateNewAccountRequest
            {
                EmailAddress = email,
                Password = password
            };

            var result = await Client.Post<CreateNewAccountRequest, Credentials>(request, CancellationToken);
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
            var accountStore = ResolveStore<Account>();
            var current = await accountStore
                .RawReadonlyQuery()
                .SingleOrDefaultAsync(x => x.EmailAddress == credentials.EmailAddress, CancellationToken);
            current.ShouldNotBeNull();

            await Delete(credentials);

            var result = await accountStore
                .RawReadonlyQuery()
                .SingleOrDefaultAsync(x => x.EmailAddress == credentials.EmailAddress, CancellationToken);

            result.ShouldBeNull();
        }

        private async Task Delete(Credentials credentials)
        {
            var tempClient = ConfigurableClient(credentials.SessionId);
            await tempClient.Delete<DeleteAccountRequest>(CancellationToken);
        }

        public override async Task DisposeAsync()
        {
            await Task.CompletedTask;
            Client.DefaultRequestHeaders.Remove("Authorization");
            Client.DefaultRequestHeaders.Remove(ApplicationConstants.MagicUrlStrings.SessionId);
        }
    }
}