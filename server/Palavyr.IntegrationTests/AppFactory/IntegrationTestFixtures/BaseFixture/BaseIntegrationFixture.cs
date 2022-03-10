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
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.API;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Contracts;
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
        public readonly string StripeCustomerId = Guid.NewGuid().ToString();

        public readonly Lazy<DashContext> dashContext;
        public readonly Lazy<AccountsContext> accountsContext;
        public readonly Lazy<ConvoContext> convoContext;
        public readonly Lazy<IServiceProvider> serviceProvider;


        public ITestOutputHelper TestOutputHelper { get; set; }
        public readonly IntegrationTestAutofacWebApplicationFactory Factory;

        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;

        public HttpClient Client => WebHostFactory.ConfigureInMemoryClient();
        public HttpClient ClientApiKey => WebHostFactory.ConfigureInMemoryApiKeyClient(ApiKey);

        public CancellationToken CancellationToken => new CancellationTokenSource(Timeout).Token;
        public TimeSpan Timeout => TimeSpan.FromMinutes(3);

        public ILogger Logger => WebHostFactory.Services.GetService<ILogger>();

        public IEntityStore<TEntity> ResolveStore<TEntity>() where TEntity : class, IEntity
        {
            return (IEntityStore<TEntity>)WebHostFactory.Services.GetService(typeof(IEntityStore<TEntity>));
        }

        public AccountsContext AccountsContext => accountsContext.Value;
        public DashContext DashContext => dashContext.Value;
        public ConvoContext ConvoContext => convoContext.Value;
        public IServiceProvider Container => serviceProvider.Value;
        public IConfiguration Configuration => TestConfiguration.GetTestConfiguration();

        protected BaseIntegrationFixture(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
        {
            TestOutputHelper = testOutputHelper;
            Factory = factory;

            serviceProvider = new Lazy<IServiceProvider>(
                () => { return WebHostFactory.Services; });

            dashContext = new Lazy<DashContext>(
                () => { return WebHostFactory.Services.GetService<DashContext>(); });
            accountsContext = new Lazy<AccountsContext>(
                () => { return WebHostFactory.Services.GetService<AccountsContext>(); });
            convoContext = new Lazy<ConvoContext>(
                () => { return WebHostFactory.Services.GetService<ConvoContext>(); });
        }

        public virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CreateS3TempFile>().As<ICreateS3TempFile>();
            return builder;
        }

        public void UseFakeStripeCustomerService(ContainerBuilder builder)
        {
            builder.Register(
                ctx =>
                {
                    var sub = Substitute.For<IStripeCustomerService>();
                    sub.DeleteStripeTestCustomers(default);
                    return sub;
                });
        }


        private protected virtual async Task DeleteTestStripeCustomers()
        {
            var customerId = await AccountsContext.Accounts.Select(x => x.StripeCustomerId).Where(x => !string.IsNullOrWhiteSpace(x)).ToListAsync(CancellationToken.None);
            if (customerId.Any())
            {
                var customerService = Container.GetService<IStripeCustomerService>();
                await customerService.DeleteStripeTestCustomers(customerId);
            }
        }

        public void SetAccountId()
        {
            var accountId = Container.GetService<IAccountIdTransport>();
            accountId.Assign(AccountId);
        }

        public void SetCancellationToken()
        {
            var token = Container.GetService<ICancellationTokenTransport>();
            token.Assign(CancellationToken);
        }

        public virtual async Task InitializeAsync()
        {
            var token = Container!.GetService<ICancellationTokenTransport>();
            if (token == null)
            {
                SetCancellationToken();
            }

            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            await DeleteTestStripeCustomers();
            await AccountsContext.DisposeAsync();
            await DashContext.DisposeAsync();
            await ConvoContext.DisposeAsync();
            WebHostFactory.Dispose();
        }
    }
}