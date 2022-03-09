using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;

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

            ResetDbs(accountContext, dashContext, convoContext);

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

        public static void ResetDbs(AccountsContext accountContext, DashContext dashContext, ConvoContext convoContext)
        {
            accountContext.ResetAccountDb();
            accountContext.SaveChanges();

            dashContext.ResetDashDb();
            dashContext.SaveChanges();

            convoContext.ResetConvoContext();
            convoContext.SaveChanges();
        }

        public static void ResetConvoContext(this ConvoContext convoContext)
        {
            var convos = convoContext.ConversationHistories.ToArray();
            var completed = convoContext.ConversationRecords.ToArray();

            convoContext.ConversationHistories.RemoveRange(convos);
            convoContext.ConversationRecords.RemoveRange(completed);
        }

        public static void ResetDashDb(this DashContext dashContext)
        {
            var areas = dashContext.Areas.ToArray();
            var convoNodes = dashContext.ConversationNodes.ToArray();
            var fees = dashContext.StaticFees.ToArray();
            var prefs = dashContext.WidgetPreferences.ToArray();
            var dynTables = dashContext.DynamicTableMetas.ToArray();
            var staticMetas = dashContext.StaticTablesMetas.ToArray();
            var staticRows = dashContext.StaticTablesRows.ToArray();
            var perc = dashContext.PercentOfThresholds.ToArray();
            var select = dashContext.SelectOneFlats.ToArray();
            var basic = dashContext.BasicThresholds.ToArray();
            var categoryNested = dashContext.CategoryNestedThresholds.ToArray();
            var twoNested = dashContext.TwoNestedCategories.ToArray();

            dashContext.Areas.RemoveRange(areas);
            dashContext.ConversationNodes.RemoveRange(convoNodes);
            dashContext.StaticFees.RemoveRange(fees);
            dashContext.WidgetPreferences.RemoveRange(prefs);
            dashContext.DynamicTableMetas.RemoveRange(dynTables);
            dashContext.StaticTablesMetas.RemoveRange(staticMetas);
            dashContext.StaticTablesRows.RemoveRange(staticRows);

            dashContext.SelectOneFlats.RemoveRange(select);
            dashContext.PercentOfThresholds.RemoveRange(perc);
            dashContext.BasicThresholds.RemoveRange(basic);
            dashContext.CategoryNestedThresholds.RemoveRange(categoryNested);
            dashContext.TwoNestedCategories.RemoveRange(twoNested);
        }

        public static void ResetAccountDb(this AccountsContext accountsContext)
        {
            var accounts = accountsContext.Accounts.ToArray();
            var backups = accountsContext.Backups.ToArray();
            var sessions = accountsContext.Sessions.ToArray();
            var subs = accountsContext.Subscriptions.ToArray();
            var emailVerifications = accountsContext.EmailVerifications.ToArray();
            var stripeWebhooks = accountsContext.StripeWebHookRecords.ToArray();

            accountsContext.Accounts.RemoveRange(accounts);
            accountsContext.Backups.RemoveRange(backups);
            accountsContext.Sessions.RemoveRange(sessions);
            accountsContext.Subscriptions.RemoveRange(subs);
            accountsContext.EmailVerifications.RemoveRange(emailVerifications);
            accountsContext.StripeWebHookRecords.RemoveRange(stripeWebhooks);
        }
    }
}