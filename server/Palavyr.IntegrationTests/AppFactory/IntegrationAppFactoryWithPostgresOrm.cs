#nullable enable
using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.API;
using Palavyr.Data;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class IntegrationAppFactoryWithPostgresOrm
    {
        public static WebApplicationFactory<Startup> ConfigureAppFactory(
            this PostgresOrmWebApplicationFactory<Startup> factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
        )
        {
            factory.ClientOptions.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            return factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigurePostgresOrmDatabase()
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo);
                    });
        }
    }
}