#nullable enable
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Data;
using Palavyr.IntegrationTests.TestAuthentication;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder EnsureAndConfigureDbs(
            this IWebHostBuilder builder,
            Action<AccountsContext>? configureAccounts,
            Action<DashContext>? configureDash,
            Action<ConvoContext>? configureConvo
            )
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        var sp = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
                        using var scope = sp.CreateScope();
                        var scopedServices = scope.ServiceProvider;

                        var accountContext = scopedServices.GetRequiredService<AccountsContext>();
                        var dashContext = scopedServices.GetRequiredService<DashContext>();
                        var convoContext = scopedServices.GetRequiredService<ConvoContext>();

                        accountContext.Database.EnsureCreated();
                        dashContext.Database.EnsureCreated();
                        convoContext.Database.EnsureCreated();

                        DbSetupAndTeardown.ResetDbs(accountContext, dashContext, convoContext);

                        if (configureAccounts != null)
                        {
                            configureAccounts(accountContext);
                        }

                        if (configureDash != null)
                        {
                            configureDash(dashContext);
                        }

                        if (configureConvo != null)
                        {
                            configureConvo(convoContext);
                        }
                    });
        }

        public static IWebHostBuilder ConfigureAuthentication(this IWebHostBuilder builder)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        services
                            .AddAuthentication("Test")
                            .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>(
                                "Test",
                                opt => { }
                            );
                    });
        }

        public static IWebHostBuilder ConfigureInMemoryDatabase(this IWebHostBuilder builder, InMemoryDatabaseRoot dbRoot)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        services.ClearDescriptors();
                        services.ConfigureInMemoryDatabases(dbRoot);
                    });
        }

        public static IWebHostBuilder ConfigurePostgresOrmDatabase(this IWebHostBuilder builder)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        services.ClearDescriptors();
                        services.ConfigureRealPostgresTestDatabases();
                    });
        }
    }
}