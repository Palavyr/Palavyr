using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.API;
using Palavyr.Client;
using Palavyr.Core;
using Palavyr.Core.Configuration;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Test.Common;
using Test.Common.Random;
using Test.Common.TestFileAssetServices;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture
{
    public abstract class IntegrationTestBase<DbType> : IClassFixture<ServerFactory>, IAsyncLifetime where DbType : DbTypes
    {
        public string SessionId;
        public string AccountId;
        public string ApiKey;
        public readonly string StripeCustomerId = Guid.NewGuid().ToString();
        public readonly string EmailAddress = A.RandomTestEmail();
        public readonly string Password = "Password01!";
        public static readonly string ConfirmationToken = "RogerWilco";
        public readonly Lazy<AutofacServiceProvider> ServiceProvider;
        protected internal virtual bool SaveStoreActionsImmediately => true;

        protected IntegrationTestBase(ITestOutputHelper testOutputHelper, ServerFactory factory)
        {

            TestOutputHelper = testOutputHelper;
            WebHostFactory = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureAppConfiguration(
                                (context, configBuilder) =>
                                {
                                    // configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration(Assembly.GetExecutingAssembly()));
                                })
                            .ConfigureTestContainer<ContainerBuilder>(b =>
                            {
                                CustomizeContainer(b);
                            });
                        if (typeof(DbType).Name == nameof(DbTypes.Real))
                        {
                            builder.ConfigureAndCreateRealTestDatabase();
                        }
                        else if (typeof(DbType).Name == nameof(DbTypes.InMemory))
                        {
                            var dbRoot = new InMemoryDatabaseRoot();
                            builder.ConfigureInMemoryDatabase(dbRoot);
                        }
                    });

            ServiceProvider = new Lazy<AutofacServiceProvider>(
                () => { return (AutofacServiceProvider)WebHostFactory.Services; });
        }

        public ITestOutputHelper TestOutputHelper { get; set; }
        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;

        public PalavyrClient Client => new PalavyrClient(WebHostFactory.ConfigureInMemoryClient(SessionId));
        public PalavyrClient ApikeyClient => new PalavyrClient(WebHostFactory.ConfigureInMemoryApiKeyClient(ApiKey));
        public Func<string, PalavyrClient> ConfigurableClient => sessionId => new PalavyrClient(WebHostFactory.ConfigureInMemoryClient(sessionId));
        public Func<string, PalavyrClient> ConfigurableApiKeyClient => apikey => new PalavyrClient(WebHostFactory.ConfigureInMemoryApiKeyClient(apikey));

        public CancellationToken CancellationToken => new CancellationTokenSource(Timeout).Token;
        public TimeSpan Timeout => TimeSpan.FromMinutes(3);
        public ILogger? Logger => WebHostFactory.Services.GetService<ILogger>();


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
        public ConfigContainerServer Config => ConfigurationGetter.GetConfiguration();


        public virtual ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            var config = ConfigurationGetter.GetConfiguration();
            builder.RegisterInstance(config).AsSelf();

            builder.RegisterType<IntegrationTestFileSaver>().As<IntegrationTestFileSaver>();
            builder.RegisterType<IntegrationTestFileDelete>().As<IFileAssetDeleter>();

            builder.RegisterType<CreateS3TempFile>().As<ICreateS3TempFile>();
            UseFakeStripeCustomerService(builder);

            if (SaveStoreActionsImmediately)
            {
                builder.RegisterGenericDecorator(typeof(IntegrationTestEntityStoreEagerSavingDecorator<>), typeof(IEntityStore<>));
            }

            // builder.RegisterGenericDecorator(typeof(IntegrationTestMediatorNotificationHandlerDecorator<>), typeof(INotificationHandler<>));
            // builder.RegisterGenericDecorator(typeof(IntegrationTestMediatorRequestHandlerDecorator<,>), typeof(IRequestHandler<,>));

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
            var tempClient = ConfigurableClient(SessionId);
            await tempClient.Delete<DeleteAccountRequest>(CancellationToken);

            var provider = ResolveType<IUnitOfWorkContextProvider>();
            await provider.DisposeContexts();

            WebHostFactory.Dispose();
        }
    }
}