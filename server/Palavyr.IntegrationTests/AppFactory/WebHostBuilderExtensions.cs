#nullable enable
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Palavyr.API;
using Palavyr.Data;
using Palavyr.IntegrationTests.TestAuthentication;

// We using 3.1 but...
//https://www.thinktecture.com/en/entity-framework-core/isolation-of-integration-tests-in-2-1/

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

                        var accLogger = scopedServices.GetRequiredService<ILogger<AccountsContext>>();
                        var dashLogger = scopedServices.GetRequiredService<ILogger<DashContext>>();
                        var convoLogger = scopedServices.GetRequiredService<ILogger<ConvoContext>>();

                        accountContext.Database.EnsureCreated();
                        dashContext.Database.EnsureCreated();
                        convoContext.Database.EnsureCreated();

                        DbSetupAndTeardown.ResetDbs(accountContext, dashContext, convoContext);

                        if (configureAccounts != null)
                        {
                            try
                            {
                                configureAccounts(accountContext);
                            }
                            catch (Exception ex)
                            {
                                accLogger.LogError(
                                    ex, "An error occurred setting up the " +
                                        "accounts database for the test. Error: {Message}", ex.Message);
                            }
                        }

                        if (configureDash != null)
                        {
                            try
                            {
                                configureDash(dashContext);
                            }
                            catch (Exception ex)
                            {
                                dashLogger.LogError(
                                    ex, "An error occurred setting up the " +
                                        "dash database for the test. Error: {Message}", ex.Message);
                            }
                        }

                        if (configureConvo != null)
                        {
                            try
                            {
                                configureConvo(convoContext);
                            }
                            catch (Exception ex)
                            {
                                convoLogger.LogError(
                                    ex, "An error occurred setting up the " +
                                        "convo database for the test. Error: {Message}", ex.Message);
                            }
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