using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Data;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class DbConfigurationExtensions
    {
        public static void ConfigureRealPostgresTestDatabases(this IServiceCollection services)
        {
            services.AddDbContext<AccountsContext>(opt => { opt.UseNpgsql(IntegrationConstants.AccountDbConnString); });
            services.AddDbContext<DashContext>(opt => { opt.UseNpgsql(IntegrationConstants.DashDbConnString); });
            services.AddDbContext<ConvoContext>(opt => { opt.UseNpgsql(IntegrationConstants.ConvoDbConnString); });
        }

        public static void ConfigureInMemoryDatabases(this IServiceCollection services, InMemoryDatabaseRoot dbRoot)
        {
            services.AddDbContext<AccountsContext>(opt => { opt.UseInMemoryDatabase("TestAccountDbInMemory", dbRoot); });
            services.AddDbContext<DashContext>(opt => { opt.UseInMemoryDatabase("TestDashDbInMemory", dbRoot); });
            services.AddDbContext<ConvoContext>(opt => { opt.UseInMemoryDatabase("TestConvoDbInMemory", dbRoot); });
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