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
        public static void ConfigureRealPostgresTestDatabases(this IServiceCollection services)
        {
            var accountId = GuidUtils.CreateShortenedGuid(5);
            var dashId = GuidUtils.CreateShortenedGuid(5);
            var convoId = GuidUtils.CreateShortenedGuid(5);
            services.AddDbContext<AccountsContext>(opt => { opt.UseNpgsql(IntegrationConstants.AccountDbConnString(accountId)); });
            services.AddDbContext<DashContext>(opt => { opt.UseNpgsql(IntegrationConstants.DashDbConnString(dashId)); });
            services.AddDbContext<ConvoContext>(opt => { opt.UseNpgsql(IntegrationConstants.ConvoDbConnString(convoId)); });
        }

        public static void ConfigureInMemoryDatabases(this IServiceCollection services, InMemoryDatabaseRoot dbRoot)
        {
            var accountDbName = "TestAccountDbInMemory-" + GuidUtils.CreateShortenedGuid(5);
            var dashDbName = "TestDashDbInMemory-" + GuidUtils.CreateShortenedGuid(5);
            var convoDbName = "TestConvoDbInMemory-" + GuidUtils.CreateShortenedGuid(5);
            
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