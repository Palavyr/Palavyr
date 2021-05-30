#nullable enable
using System;
using Autofac;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Xunit.Abstractions;

// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#inject-mock-services 
namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public abstract class InMemoryIntegrationFixture : BaseIntegrationFixture
    {
        protected InMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
            CreateContext();
        }

        protected sealed override void CreateContext()
        {
            Factory.Server.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            var dbRoot = new InMemoryDatabaseRoot();
            WebHostFactory = Factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureTestContainer<ContainerBuilder>(builder => CustomizeContainer(builder.CallStartupTestContainerConfiguration()))
                            .ConfigureTestServices(services => services.CallStartupServicesConfiguration().AddMvcCore().AddApplicationPart(typeof(Startup).Assembly))
                            .ConfigureInMemoryDatabase(dbRoot)
                            .ConfigureAppConfiguration(
                                (context, configBuilder) => { configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration()); })
                            .UseTestServer();
                    });
        }
    }
}