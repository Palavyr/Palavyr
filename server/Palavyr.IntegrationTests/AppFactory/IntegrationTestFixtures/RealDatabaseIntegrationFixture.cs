﻿#nullable enable
using Autofac;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
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
                            .UseTestServer();
                    });
        }
    }
}