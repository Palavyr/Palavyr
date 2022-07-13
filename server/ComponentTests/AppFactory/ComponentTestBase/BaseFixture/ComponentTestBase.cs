using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Component.AppFactory.AutofacWebApplicationFactory;
using Component.AppFactory.ExtensionMethods;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.API;
using Palavyr.Client;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Contracts;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Test.Common;
using Test.Common.Random;
using Test.Common.TestFileAssetServices;
using Xunit;
using Xunit.Abstractions;

namespace Component.AppFactory.ComponentTestBase.BaseFixture
{
    public abstract class ComponentTestBase : IClassFixture<ServerFactory>, IAsyncLifetime
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

        protected ComponentTestBase(ITestOutputHelper testOutputHelper, ServerFactory factory)
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
                                    configBuilder.AddConfiguration(TestConfiguration.GetTestConfiguration(Assembly.GetExecutingAssembly()));
                                })
                            .ConfigureTestContainer<ContainerBuilder>(b => CustomizeContainer(b))
                            .ConfigureTestServices(
                                services =>
                                {
                                    ClearDescriptors(services);
                                    ConfigureInMemoryDatabases(services, new InMemoryDatabaseRoot());
                                    CreateDatabases(services);
                                });
                    });

            ServiceProvider = new Lazy<AutofacServiceProvider>(
                () => { return (AutofacServiceProvider)WebHostFactory.Services; });
        }

        private static void CreateDatabases(IServiceCollection services)
        {
            var sp = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var dashContext = scopedServices.GetService<DashContext>();
            var accountContext = scopedServices.GetService<AccountsContext>();
            var convoContext = scopedServices.GetService<ConvoContext>();

            accountContext.Database.EnsureDeleted();
            dashContext.Database.EnsureDeleted();
            convoContext.Database.EnsureDeleted();

            accountContext.Database.EnsureCreated();
            dashContext.Database.EnsureCreated();
            convoContext.Database.EnsureCreated();

            // var tempCancellationToken = new CancellationTokenTransport(new CancellationToken());
            // var contextProvider = new UnitOfWorkContextProvider(dashContext, accountContext, convoContext, tempCancellationToken);
            // ResetDbs(scopedServices, contextProvider, tempCancellationToken);

            // return Task.CompletedTask;
        }

        private static void ClearDescriptors(IServiceCollection services)
        {
            var accountsContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AccountsContext>));

            if (accountsContextDescriptor != null)
            {
                services.Remove(accountsContextDescriptor);
            }

            var dashContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DashContext>));
            if (dashContextDescriptor != null)
            {
                services.Remove(dashContextDescriptor);
            }

            var convoContextDescriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ConvoContext>));
            if (convoContextDescriptor != null)
            {
                services.Remove(convoContextDescriptor);
            }
        }
        
        private static void ConfigureInMemoryDatabases(IServiceCollection services, InMemoryDatabaseRoot dbRoot)
        {
            var accountDbName = "TestAccountDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var dashDbName = "TestDashDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var convoDbName = "TestConvoDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);

            // services.AddSingleton<DBTracker>();

            services.AddDbContext<AccountsContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(accountDbName, dbRoot);
                    SuppressWarnings(opt);
                });
            services.AddDbContext<DashContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(dashDbName, dbRoot);
                    SuppressWarnings(opt);
                });
            services.AddDbContext<ConvoContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(convoDbName, dbRoot);
                    SuppressWarnings(opt);
                });
        }

        private static void SuppressWarnings(DbContextOptionsBuilder builder)
        {
            builder.ConfigureWarnings(x => { x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning); });
            builder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }

        public ITestOutputHelper TestOutputHelper { get; set; }
        public WebApplicationFactory<Startup> WebHostFactory { get; set; } = null!;

        public PalavyrClient Client => new PalavyrClient(WebHostFactory.ConfigureInMemoryClient(SessionId));
        public PalavyrClient ApikeyClient => new PalavyrClient(WebHostFactory.ConfigureInMemoryApiKeyClient(ApiKey));

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
            // var tempClient = ConfigurableClient(SessionId);
            // await tempClient.Delete<DeleteAccountRequest>(CancellationToken);
            //
            // var provider = ResolveType<IUnitOfWorkContextProvider>();
            // await provider.DisposeContexts();
            //
            // WebHostFactory.Dispose();
        }
    }
}