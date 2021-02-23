using Autofac;
using Palavyr.Services.DatabaseService;
using Palavyr.Services.DatabaseService.Delete;

namespace Palavyr.API.Registration.Container
{
    public class ConnectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountsConnector>().As<IAccountsConnector>().InstancePerLifetimeScope();
            builder.RegisterType<DashConnector>().As<IDashConnector>().InstancePerLifetimeScope();
            builder.RegisterType<ConvoConnector>().As<IConvoConnector>().InstancePerLifetimeScope();

            builder.RegisterType<ConvoDeleter>().As<IConvoDeleter>().InstancePerLifetimeScope();
            builder.RegisterType<DashDeleter>().As<IDashDeleter>().InstancePerLifetimeScope();
            builder.RegisterType<AccountDeleter>().As<IAccountDeleter>().InstancePerLifetimeScope();
        }
    }
}