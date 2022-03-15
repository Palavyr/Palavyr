using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.Delete;
using Test.Common;
using Test.Common.TestFileAssetServices;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
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
            // services.AddSingleton<DBTracker>();
            services.AddDbContext<AccountsContext>(
                opt =>
                {
                    opt.UseNpgsql(IntegrationConstants.AccountDbConnString);
                    opt.SuppressWarnings();
                });
            services.AddDbContext<DashContext>(
                opt =>
                {
                    opt.UseNpgsql(IntegrationConstants.DashDbConnString);
                    opt.SuppressWarnings();
                });
            services.AddDbContext<ConvoContext>(
                opt =>
                {
                    opt.UseNpgsql(IntegrationConstants.ConvoDbConnString);
                    opt.SuppressWarnings();
                });
        }

        private static Task CreateDatabases(this IServiceCollection services)
        {
            var sp = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var dashContext = scopedServices.GetService<DashContext>();
            var accountContext = scopedServices.GetService<AccountsContext>();
            var convoContext = scopedServices.GetService<ConvoContext>();
            // var tracker = scopedServices.GetService<DBTracker>();

            // if (!tracker.HasBeenReCreated)
            // {
            //     accountContext.Database.EnsureDeleted();
            //     dashContext.Database.EnsureDeleted();
            //     convoContext.Database.EnsureDeleted();
            //     tracker.HasBeenReCreated = true;
            // }

            accountContext.Database.EnsureCreated();
            dashContext.Database.EnsureCreated();
            convoContext.Database.EnsureCreated();

            var tempCancellationToken = new CancellationTokenTransport(new CancellationToken());
            var contextProvider = new UnitOfWorkContextProvider(dashContext, accountContext, convoContext, tempCancellationToken);
            ResetDbs(scopedServices, contextProvider, tempCancellationToken);

            // contextProvider.DangerousCommitAllContexts();

            return Task.CompletedTask;
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
                    opt.SuppressWarnings();
                });
            services.AddDbContext<DashContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(dashDbName, dbRoot);
                    opt.SuppressWarnings();
                });
            services.AddDbContext<ConvoContext>(
                opt =>
                {
                    opt.UseInMemoryDatabase(convoDbName, dbRoot);
                    opt.SuppressWarnings();
                });
        }

        private static void SuppressWarnings(this DbContextOptionsBuilder builder)
        {
            builder.ConfigureWarnings(x => { x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning); });
        }

        private static void ClearDescriptors(IServiceCollection services)
        {
            var accountsContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AccountsContext>));
            if (accountsContextDescriptor != null)
            {
                services.Remove(accountsContextDescriptor);
            }

            var dashContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DashContext>));
            if (dashContextDescriptor != null)
            {
                services.Remove(dashContextDescriptor);
            }

            var convoContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ConvoContext>));
            if (convoContextDescriptor != null)
            {
                services.Remove(convoContextDescriptor);
            }
        }

        public static void ResetDbs(IServiceProvider scopedServices, IUnitOfWorkContextProvider contextProvider, ICancellationTokenTransport cancellationTokenTransport)
        {
            var context = scopedServices.GetService<AccountsContext>();
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
                    contextProvider.ConfigurationContext(),
                    contextProvider.ConvoContext(),
                    contextProvider.AccountsContext(),
                    accountTransport,
                    cancellationTokenTransport);
                accountDeleter.DeleteAllThings();
            }
        }
    }

    // public sealed class DBTracker
    // {
    //     public bool HasBeenReCreated { get; set; }
    // }
}