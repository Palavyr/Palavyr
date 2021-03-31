using Autofac;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.Delete;

namespace Palavyr.API.Registration.Container
{
    public class ConnectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ConfigurationRepository>().As<IConfigurationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ConvoHistoryRepository>().As<IConvoHistoryRepository>().InstancePerLifetimeScope();

            builder.RegisterType<ConvoDeleter>().As<IConvoDeleter>().InstancePerLifetimeScope();
            builder.RegisterType<DashDeleter>().As<IDashDeleter>().InstancePerLifetimeScope();
            builder.RegisterType<AccountDeleter>().As<IAccountDeleter>().InstancePerLifetimeScope();
        }
    }
}