﻿using System.Net;
using System.Threading.Tasks;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.ExtensionMethods;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.Client;
using Palavyr.Core.Handlers.ControllerHandler;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests.Api.ControllerFixtures.Accounts.Settings
{
    public class WhenGettingAnApiKeyForAnAccountThatDoesNotExist : IntegrationTest
    {
        public WhenGettingAnApiKeyForAnAccountThatDoesNotExist(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        [Fact]
        public async Task GetApiKeyFails()
        {
            var client = new PalavyrClient(WebHostFactory.ConfigureInMemoryApiKeyClient(A.RandomId()));
            var result = await client.GetHttp<GetApiKeyRequest>(CancellationToken);
            result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }
    }
}