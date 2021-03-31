#nullable enable
using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.Core.Data;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class IntegrationAppFactoryWithInMemoryDb
    {
        public static HttpClient CreateInMemAuthedClient(
            this InMemoryWebApplicationFactory factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
            )
        {
            factory.ClientOptions.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            var dbRoot = new InMemoryDatabaseRoot();
            var client = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureInMemoryDatabase(dbRoot)
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo)
                            .UseTestServer();
                    })
                .CreateClient();
            return client;
        }

        public static HttpClient ConfigureUnauthenticatedClientWithInMemContext(
            this InMemoryWebApplicationFactory factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
        )
        {
            factory.ClientOptions.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            var dbRoot = new InMemoryDatabaseRoot();
            var client = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureInMemoryDatabase(dbRoot)
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo)
                            .UseTestServer();
                    })
                .CreateClient();
            return client;
        }
    }
}