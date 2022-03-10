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
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.Delete;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Sessions;

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

        private static void CreateDatabases(this IServiceCollection services)
        {
            var sp = ServiceCollectionContainerBuilderExtensions.BuildServiceProvider(services);
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var accountContext = scopedServices.GetRequiredService<AccountsContext>();
            var dashContext = scopedServices.GetRequiredService<DashContext>();
            var convoContext = scopedServices.GetRequiredService<ConvoContext>();

            accountContext.Database.EnsureCreated();
            dashContext.Database.EnsureCreated();
            convoContext.Database.EnsureCreated();

            ResetDbs(scopedServices, accountContext, dashContext, convoContext);

            accountContext.SaveChanges();
            dashContext.SaveChanges();
            convoContext.SaveChanges();
        }

        private static void ConfigureInMemoryDatabases(IServiceCollection services, InMemoryDatabaseRoot dbRoot)
        {
            var accountDbName = "TestAccountDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var dashDbName = "TestDashDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var convoDbName = "TestConvoDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);

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

        public static async Task ResetDbs(IServiceProvider scopedServices, AccountsContext accountContext, DashContext dashContext, ConvoContext convoContext)
        {
            var accounts = await accountContext.Accounts.Select(x => x.AccountId).ToArrayAsync();

            foreach (var account in accounts)
            {
                var accountStore = scopedServices.GetService<EntityStore<Account>>();
                var fileAssetDeleter = scopedServices.GetService<IFileAssetDeleter>();
                var accountTransport = new AccountIdTransport();
                accountTransport.Assign(account);
                var token = new CancellationTokenTransport();
                token.Assign(default);

                var accountDeleter = new DangerousAccountDeleter(accountStore, fileAssetDeleter, dashContext, convoContext, accountContext, accountTransport, token);
                await accountDeleter.DeleteAllThings();
            }

            accountContext.SaveChanges();
            dashContext.SaveChanges();
            convoContext.SaveChanges();
        }
    }
}