using System;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.Core.Data;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Test.Common;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public abstract class BaseIntegrationFixture : IAssemblyFixture<IntegrationTestAutofacWebApplicationFactory>
    {
        public ITestOutputHelper TestOutputHelper { get; set; }
        public readonly IntegrationTestAutofacWebApplicationFactory Factory;

        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;

        public HttpClient Client => WebHostFactory.ConfigureInMemoryClient();
        public DashContext DashContext => WebHostFactory.Services.GetService<DashContext>();
        public AccountsContext AccountsContext => WebHostFactory.Services.GetService<AccountsContext>();
        public ConvoContext ConvoContext => WebHostFactory.Services.GetService<ConvoContext>();
        public IServiceProvider Container => WebHostFactory.Services;
        public IConfiguration Configuration => TestConfiguration.GetTestConfiguration();

        protected BaseIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
        {
            TestOutputHelper = testOutputHelper;
            Factory = factory;
        }

        protected abstract void CreateContext();

        public virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CreateS3TempFile>().As<ICreateS3TempFile>();
            return builder;
        }
        

        private protected virtual async Task DeleteTestStripeCustomers()
        {
            var customerService = Container.GetService<StripeCustomerService>();
            await customerService.DeleteStripeTestCustomers();
        }
    }
}