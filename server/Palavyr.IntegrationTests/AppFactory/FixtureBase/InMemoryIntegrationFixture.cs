#nullable enable
using System;
using System.Net.Http;
using Autofac;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.API;
using Palavyr.Core.Data;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Test.Common;
using Xunit.Abstractions;

// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.1#inject-mock-services 
namespace Palavyr.IntegrationTests.AppFactory.FixtureBase
{
    public abstract class InMemoryIntegrationFixture : IClassFixture<InMemoryAutofacWebApplicationFactory>
    {
        public ITestOutputHelper TestOutputHelper { get; set; } = null!;
        private readonly InMemoryAutofacWebApplicationFactory factory;

        public HttpClient Client { get; set; } = null!;
        public DashContext DashContext { get; set; } = null!;
        public AccountsContext AccountsContext { get; set; } = null!;
        public ConvoContext ConvoContext { get; set; } = null!;
        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;
        public IServiceProvider Container { get; set; } = null!;
        public IConfiguration Configuration { get; set; } = null!;

        protected InMemoryIntegrationFixture(ITestOutputHelper testOutputHelper, InMemoryAutofacWebApplicationFactory factory)
        {
            this.TestOutputHelper = testOutputHelper;
            this.factory = factory;
            CreateContext();
        }

        public virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {

            builder.RegisterType<CreateS3TempFile>().AsSelf();
            return builder;
        }

        protected void CreateContext()
        {
            factory.Server.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            var dbRoot = new InMemoryDatabaseRoot();
            var webHost = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureTestContainer<ContainerBuilder>(builder => CustomizeContainer(builder.CallStartupTestContainerConfiguration()))
                            .ConfigureTestServices(services => services.CallStartupServicesConfiguration().AddMvcCore().AddApplicationPart(typeof(Startup).Assembly))
                            .ConfigureInMemoryDatabase(dbRoot)
                            .ConfigureAppConfiguration(
                                (context, configBuilder) => { configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration()); })
                            .EnsureAndConfigureDbs()
                            .UseTestServer();
                    });

            DashContext = webHost.Services.GetService<DashContext>();
            AccountsContext = webHost.Services.GetService<AccountsContext>();
            ConvoContext = webHost.Services.GetService<ConvoContext>();
            Client = webHost.ConfigureInMemoryClient();
            WebHostFactory = webHost;
            Container = webHost.Services;
            Configuration = TestConfiguration.GetTestConfiguration();
        }
    }
}