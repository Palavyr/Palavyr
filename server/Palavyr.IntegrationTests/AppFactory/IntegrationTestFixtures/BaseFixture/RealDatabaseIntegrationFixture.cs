#nullable enable
using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public abstract class RealDatabaseIntegrationFixture : BaseIntegrationFixture, IAsyncLifetime
    {
        protected RealDatabaseIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
            CreateContext();
        }

        protected sealed override void CreateContext()
        {
            Factory.Server.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            WebHostFactory = Factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureTestContainer<ContainerBuilder>(builder => CustomizeContainer(builder.CallStartupTestContainerConfiguration()))
                            .ConfigureTestServices(services => services.CallStartupServicesConfiguration().AddMvcCore().AddApplicationPart(typeof(Startup).Assembly))
                            .ConfigureAndCreateRealTestDatabase()
                            .ConfigureAppConfiguration(
                                (context, configBuilder) => { configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration()); })
                            .UseTestServer();
                    });
        }

        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            AccountsContext.Database.EnsureDeleted();
            DashContext.Database.EnsureDeleted();
            ConvoContext.Database.EnsureDeleted();
            
            await Task.CompletedTask;
        }
        

    }
}