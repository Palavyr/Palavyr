using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.Delete;
using Test.Common.TestFileAssetServices;

namespace Test.Common
{
    public static class DbConfigurationExtensions
    {
        public static IWebHostBuilder ConfigureInMemoryDatabase(this IWebHostBuilder builder, InMemoryDatabaseRoot dbRoot)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        ClearDescriptors(services);
                        ConfigureInMemoryDatabases(services, dbRoot);
                        CreateDatabases(services);
                    });
        }

        public static IWebHostBuilder ConfigureAndCreateRealTestDatabase(this IWebHostBuilder builder)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        ClearDescriptors(services);
                        AddRealDatabaseContexts(services);
                        CreateDatabases(services);
                    });
        }

        private static void AddRealDatabaseContexts(IServiceCollection services)
        {
            services.AddDbContext<AppDataContexts>(
                opt =>
                {
                    opt.UseNpgsql(ConnectionStringBuilder.BuildConnectionString());
                    opt.SuppressWarnings();
                });
        }

        private static Task CreateDatabases(this IServiceCollection services)
        {
            var sp = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var rawContext = scopedServices.GetService<AppDataContexts>();
            rawContext.Database.EnsureDeleted();
            rawContext.Database.EnsureCreated();

            // var tempCancellationToken = new CancellationTokenTransport(new CancellationToken());
            // var contextProvider = new UnitOfWorkContextProvider(dashContext, accountContext, convoContext, tempCancellationToken);
            // ResetDbs(scopedServices, contextProvider, tempCancellationToken);

            return Task.CompletedTask;
        }

        private static void ConfigureInMemoryDatabases(IServiceCollection services, InMemoryDatabaseRoot dbRoot)
        {
            var dbName = "TestAccountDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);

            services.AddDbContext<AppDataContexts>(
                opt =>
                {
                    opt.UseInMemoryDatabase(dbName, dbRoot);
                    opt.SuppressWarnings();
                });
        }

        private static void SuppressWarnings(this DbContextOptionsBuilder builder)
        {
            builder.ConfigureWarnings(x => { x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning); });
            builder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }

        private static void ClearDescriptors(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDataContexts>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
        }

        public static void ResetDbs(IServiceProvider scopedServices, IUnitOfWorkContextProvider contextProvider, ICancellationTokenTransport cancellationTokenTransport)
        {
            var context = scopedServices.GetService<AppDataContexts>();
            var accounts = context.Accounts.ToList();

            foreach (var account in accounts)
            {
                var accountTransport = new AccountIdTransport(account.AccountId);
                var assetStore = new EntityStore<FileAsset>(contextProvider, accountTransport, cancellationTokenTransport);
                var decoratedStore = new IntegrationTestEntityStoreEagerSavingDecorator<FileAsset>(assetStore, contextProvider);

                var deleter = new IntegrationTestFileDelete(
                    decoratedStore);

                var accountDeleter = new DangerousAccountDeleter(
                    scopedServices,
                    contextProvider,
                    deleter,
                    contextProvider.Data,
                    accountTransport,
                    cancellationTokenTransport);
                accountDeleter.DeleteAllThings().Wait();
            }
        }
    }
}