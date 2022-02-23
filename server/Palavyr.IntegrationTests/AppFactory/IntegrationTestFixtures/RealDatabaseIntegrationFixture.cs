#nullable enable
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Palavyr.Core.GlobalConstants;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public abstract class RealDatabaseIntegrationFixture : BaseIntegrationFixture
    {
        protected RealDatabaseIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
            WebHostFactory = Factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureAppConfiguration(
                                (context, configBuilder) => { configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration()); })
                            .ConfigureTestContainer<ContainerBuilder>(builder => CustomizeContainer(builder))
                            .ConfigureAndCreateRealTestDatabase()
                            .ConfigureLogging(
                                (hostingContext, logging) =>
                                {
                                    logging.ClearProviders();
                                    logging.AddConfiguration(hostingContext.Configuration.GetSection(ApplicationConstants.ConfigSections.LoggingSection));
                                    logging.SetMinimumLevel(LogLevel.Trace);
                                    logging.AddConsole();
                                    logging.AddDebug();
                                    logging.AddEventSourceLogger();
                                    logging.AddNLog();
                                    // logging.AddSeq();
                                })
                            .UseTestServer();
                    });
        }
    }
}