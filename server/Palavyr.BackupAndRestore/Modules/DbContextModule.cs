using Autofac;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Palavyr.BackupAndRestore.Modules
{
    public class DbContextModule : Module
    {
        private readonly IConfiguration configuration;
        private const string AccountDbStringKey = "AccountsContextPostgres";

        public DbContextModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.Register(
                context =>
                {
                    var options = new DbContextOptionsBuilder<AccountsContext>();
                    options.UseNpgsql(configuration.GetConnectionString(AccountDbStringKey));
                    return new AccountsContext(options.Options);
                })
                .AsSelf()
                .InstancePerLifetimeScope();
    
            
        }
    }
}