#nullable enable
using System;
using Microsoft.AspNetCore.Hosting;
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
            Action<ConvoContext>? configureConvo = null,
            Action<IWebHostBuilder>? configureTestServices = null
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
                        if (configureTestServices != null)
                        {
                            try
                            {
                                configureTestServices(builder);
                            }

                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error occurred during service setup. {ex.Message}");
                            }
                        }
                    }
                );
        }
    }
}