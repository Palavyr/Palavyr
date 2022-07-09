using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.API;
using Palavyr.Client;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.Delete;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Test.Common;
using Test.Common.TestFileAssetServices;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public abstract class BaseIntegrationFixture : IClassFixture<ServerFactory>, IAsyncLifetime
    {
        public string SessionId;
        public string AccountId;
        public string ApiKey;
        public readonly string StripeCustomerId = Guid.NewGuid().ToString();
        public readonly string EmailAddress = $"test-{Guid.NewGuid()}@gmail.com";
        public readonly string Password = "Password01!";
        public readonly Lazy<AutofacServiceProvider> ServiceProvider;
        protected internal virtual bool SaveStoreActionsImmediately => true;
        public readonly ServerFactory Factory;

        protected BaseIntegrationFixture(ITestOutputHelper testOutputHelper, ServerFactory factory)
        {
            TestOutputHelper = testOutputHelper;
            Factory = factory;

            ServiceProvider = new Lazy<AutofacServiceProvider>(
                () => { return (AutofacServiceProvider)WebHostFactory.Services; });
        }

        public ITestOutputHelper TestOutputHelper { get; set; }
        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;

        public PalavyrClient Client => new PalavyrClient(WebHostFactory.ConfigureInMemoryClient(SessionId));

        public Lazy<PalavyrClient> LazyClient =>
            new Lazy<PalavyrClient>(
                () => { return new PalavyrClient(WebHostFactory.ConfigureInMemoryClient(SessionId)); });

        public PalavyrClient ApikeyClient => LazyApikeyClient.Value;

        public Lazy<PalavyrClient> LazyApikeyClient =>
            new Lazy<PalavyrClient>(
                () => { return new PalavyrClient(WebHostFactory.ConfigureInMemoryApiKeyClient(ApiKey)); });

        public Func<string, PalavyrClient> ConfigurableClient => sessionId => LazyConfigurableClient(sessionId).Value;

        public Func<string, Lazy<PalavyrClient>> LazyConfigurableClient =>
            sessionId => new Lazy<PalavyrClient>(
                () => { return new PalavyrClient(WebHostFactory.ConfigureInMemoryClient(sessionId)); });

        public CancellationToken CancellationToken => new CancellationTokenSource(Timeout).Token;
        public TimeSpan Timeout => TimeSpan.FromMinutes(3);
        public ILogger Logger => WebHostFactory.Services.GetService<ILogger>();


        public IEntityStore<TEntity> ResolveStore<TEntity>() where TEntity : class, IEntity
        {
            var store = WebHostFactory.Services.GetService<IEntityStore<TEntity>>();
            return store;
        }

        public TType ResolveType<TType>()
        {
            return WebHostFactory.Services.GetService<TType>();
        }

        public AutofacServiceProvider Container => ServiceProvider.Value;
        public IConfiguration Configuration => TestConfiguration.GetTestConfiguration(Assembly.GetExecutingAssembly());


        public virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<IntegrationTestFileSaver>().As<IntegrationTestFileSaver>();
            builder.RegisterType<IntegrationTestFileDelete>().As<IFileAssetDeleter>();

            builder.RegisterType<CreateS3TempFile>().As<ICreateS3TempFile>();
            UseFakeStripeCustomerService(builder);

            if (SaveStoreActionsImmediately)
            {
                builder.RegisterGenericDecorator(typeof(IntegrationTestEntityStoreEagerSavingDecorator<>), typeof(IEntityStore<>));
            }

            builder.RegisterGenericDecorator(typeof(IntegrationTestMediatorNotificationHandlerDecorator<>), typeof(INotificationHandler<>));
            builder.RegisterGenericDecorator(typeof(IntegrationTestMediatorRequestHandlerDecorator<,>), typeof(IRequestHandler<,>));

            return builder;
        }

        public void UseFakeStripeCustomerService(ContainerBuilder builder)
        {
            builder.Register(
                ctx =>
                {
                    var sub = Substitute.For<IStripeCustomerService>();
                    sub.DeleteStripeTestCustomers(default!);
                    return sub;
                });
        }


        private protected virtual async Task DeleteTestStripeCustomers()
        {
            await Task.CompletedTask;
            // var customerService = ResolveType<IStripeCustomerService>();
            // await customerService.DeleteSingleStripeTestCustomer(StripeCustomerId);
        }

        public void SetAccountIdTransport()
        {
            var transport = ResolveType<IAccountIdTransport>();
            if (!transport.IsSet())
            {
                transport.Assign(AccountId);
            }
        }

        public void SetCancellationToken()
        {
            var token = Container.GetService<ICancellationTokenTransport>();
            if (!token.IsSet())
            {
                token.Assign(CancellationToken);
            }
        }

        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
        {
            await DeleteTestStripeCustomers();
            SetCancellationToken();

            var tempClient = ConfigurableClient(SessionId);
            await tempClient.Delete<DeleteAccountRequest>(CancellationToken);

            var sessionStore = ResolveStore<Session>();
            await sessionStore.Delete(SessionId, s => s.SessionId);

            var provider = ResolveType<IUnitOfWorkContextProvider>();
            await provider.DangerousCommitAllContexts();
            await provider.DisposeContexts();

            WebHostFactory.Dispose();
        }
    }
}