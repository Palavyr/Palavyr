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
            WebHostFactory = Factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureAppConfiguration(
                                (context, configBuilder) => { configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration()); })
                            // .ConfigureTestServices(services => services.CallStartupServicesConfiguration().AddMvcCore().AddApplicationPart(typeof(Startup).Assembly))
                            .ConfigureTestContainer<ContainerBuilder>(builder =>
                            {
                                CustomizeContainer(builder);
                            })
                            .ConfigureAndCreateRealTestDatabase()
                            .UseTestServer();
                    });
            // WebHostFactory.Server.BaseAddress =  new Uri(IntegrationConstants.BaseUri);

        }

        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            await DeleteTestStripeCustomers(); // needs to be run BEFORE we delete the databases
            await AccountsContext.Database.EnsureDeletedAsync();
            await DashContext.Database.EnsureDeletedAsync();
            await ConvoContext.Database.EnsureDeletedAsync();
        }
    }
}