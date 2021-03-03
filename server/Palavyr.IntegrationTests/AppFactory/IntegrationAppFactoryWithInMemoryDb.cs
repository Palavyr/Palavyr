#nullable enable
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.API;
using Palavyr.Data;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class IntegrationAppFactoryWithInMemoryDb
    {
        public static HttpClient CreateInMemAuthedClient(
            this WebApplicationFactory<Startup> factory,
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
                            // .UseStartup<Startup>()
                            // .UseEnvironment("Test")
                            .ConfigureAuthentication()
                            .ConfigureInMemoryDatabase(dbRoot)
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo);
                    })
                .CreateClient();
            return client;
        }

        public static HttpClient ConfigureUnauthenticatedClientWithInMemContext(
            this WebApplicationFactory<Startup> factory,
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
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo);
                    })
                .CreateClient();
            return client;
        }
    }
}