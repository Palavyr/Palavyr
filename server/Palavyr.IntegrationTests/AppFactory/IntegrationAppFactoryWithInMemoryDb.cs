using System;
using System.Net.Http;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.API;
using Palavyr.Common.RequestsTools;
using Palavyr.Data;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class IntegrationAppFactoryWithInMemoryDb
    {
        public static HttpClient ConfigureAuthenticatedClientWithInMemContext(
            this ClaimInjectorWebApplicationFactory<Startup> factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
            )
        {
            var dbRoot = new InMemoryDatabaseRoot();
            var client = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureAuthentication()
                            .ConfigureInMemoryDatabase(dbRoot)
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo);
                        
                    })
                .CreateClient();
            client.DefaultRequestHeaders.Add(MagicUrlStrings.Action, MagicUrlStrings.SessionAction);
            client.DefaultRequestHeaders.Add(MagicUrlStrings.SessionId, IntegrationConstants.SessionId);
            return client;
        }

        public static HttpClient ConfigureUnauthenticatedClientWithInMemContext(
            this ClaimInjectorWebApplicationFactory<Startup> factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
        )
        {
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
            client.DefaultRequestHeaders.Add(MagicUrlStrings.Action, MagicUrlStrings.SessionAction);
            return client;
        }
    }
}