using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.Core.Data;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public abstract class BaseIntegrationFixture : IClassFixture<IntegrationTestAutofacWebApplicationFactory>, IAsyncLifetime
    {
        public readonly string AccountId = Guid.NewGuid().ToString();
        public readonly string ApiKey = Guid.NewGuid().ToString();


        public ITestOutputHelper TestOutputHelper { get; set; }
        public readonly IntegrationTestAutofacWebApplicationFactory Factory;

        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;

        public HttpClient Client => WebHostFactory.ConfigureInMemoryClient();
        public HttpClient ClientApiKey => WebHostFactory.ConfigureInMemoryApiKeyClient(ApiKey);
        public DashContext DashContext => WebHostFactory.Services.GetService<DashContext>();

        public CancellationToken CancellationToken => new CancellationTokenSource(Timeout).Token;
        public TimeSpan Timeout => TimeSpan.FromMinutes(3);

        public IAccountRepository AccountRepository => WebHostFactory.Services.GetService<IAccountRepository>();
        public IConfigurationRepository ConfigurationRepository => WebHostFactory.Services.GetService<IConfigurationRepository>();
        public IConvoHistoryRepository ConvoHistoryRepository => WebHostFactory.Services.GetService<IConvoHistoryRepository>();

        public AccountsContext AccountsContext => WebHostFactory.Services.GetService<AccountsContext>();
        public ConvoContext ConvoContext => WebHostFactory.Services.GetService<ConvoContext>();
        public IServiceProvider Container => WebHostFactory.Services;
        public IConfiguration Configuration => TestConfiguration.GetTestConfiguration();

        protected BaseIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
        {
            TestOutputHelper = testOutputHelper;
            Factory = factory;
        }

        public virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CreateS3TempFile>().As<ICreateS3TempFile>();
            return builder;
        }

        public void ManuallySetCancellationToken()
        {
            var cts = Container.GetService<ITransportACancellationToken>();
            cts.Assign(CancellationToken);
        }


        private protected virtual async Task DeleteTestStripeCustomers()
        {
            var customerId = await AccountsContext.Accounts.Select(x => x.StripeCustomerId).Where(x => !string.IsNullOrWhiteSpace(x)).ToListAsync(CancellationToken.None);
            if (customerId.Any())
            {
                var customerService = Container.GetService<StripeCustomerService>();
                await customerService.DeleteStripeTestCustomers(customerId);
            }
        }

        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            await DeleteTestStripeCustomers();
        }
    }
}