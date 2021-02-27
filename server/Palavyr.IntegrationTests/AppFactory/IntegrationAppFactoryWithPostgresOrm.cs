using System;
using System.Net.Http;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.API;
using Palavyr.Common.RequestsTools;
using Palavyr.Data;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class IntegrationAppFactoryWithPostgresOrm
    {
        public static HttpClient ConfigureAuthenticatedClientWithOrmContext(
            this ClaimInjectorWebApplicationFactory<Startup> factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
        )
        {
            var client = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureAuthentication()
                            .ConfigurePostgresOrmDatabase()
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo);
                        
                    })
                .CreateClient();
            client.DefaultRequestHeaders.Add(MagicUrlStrings.Action, MagicUrlStrings.SessionAction);
            return client;
        }
    }
}