using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Palavyr.Common.GlobalConstants;
using Palavyr.Data;


namespace Palavyr.BackupAndRestore.Modules
{
    public class DbContextModule : Module
    {
        private readonly IConfiguration configuration;

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
                        options.UseNpgsql(configuration.GetConnectionString(ConfigSections.AccountDbStringKey));
                        return new AccountsContext(options.Options);
                    })
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.Register(
                    context =>
                    {
                        var options = new DbContextOptionsBuilder<DashContext>();
                        options.UseNpgsql(configuration.GetConnectionString(ConfigSections.ConfigurationDbStringKey));
                        return new DashContext(options.Options);
                    })
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}