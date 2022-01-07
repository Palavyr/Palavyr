using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Data;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class DbConfigurationExtensions
    {
        public static void AddRealDatabaseContexts(this IServiceCollection services)
        {
            services.AddDbContext<AccountsContext>(opt => { opt.UseNpgsql(IntegrationConstants.AccountDbConnString); });
            services.AddDbContext<DashContext>(opt => { opt.UseNpgsql(IntegrationConstants.DashDbConnString); });
            services.AddDbContext<ConvoContext>(opt => { opt.UseNpgsql(IntegrationConstants.ConvoDbConnString); });
        }

        public static void CreateDatabases(this IServiceCollection services)
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

            DbSetupAndTeardown.ResetDbs(accountContext, dashContext, convoContext);

            accountContext.SaveChanges();
            dashContext.SaveChanges();
            convoContext.SaveChanges();
        }

        public static void ConfigureInMemoryDatabases(this IServiceCollection services, InMemoryDatabaseRoot dbRoot)
        {
            var accountDbName = "TestAccountDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var dashDbName = "TestDashDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            var convoDbName = "TestConvoDbInMemory-" + StaticGuidUtils.CreateShortenedGuid(5);
            
            services.AddDbContext<AccountsContext>(opt => { opt.UseInMemoryDatabase(accountDbName, dbRoot); });
            services.AddDbContext<DashContext>(opt => { opt.UseInMemoryDatabase(dashDbName, dbRoot); });
            services.AddDbContext<ConvoContext>(opt => { opt.UseInMemoryDatabase(convoDbName, dbRoot); });
        }

        public static void ClearDescriptors(this IServiceCollection services)
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
    }
}