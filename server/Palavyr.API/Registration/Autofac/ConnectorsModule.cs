using Autofac;
using DashboardServer.Data.Abstractions;


namespace Palavyr.API.Registration.Autofac
{
    public class ConnectorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountsConnector>().As<IAccountsConnector>().InstancePerLifetimeScope();
            builder.RegisterType<DashConnector>().As<IDashConnector>().InstancePerLifetimeScope();
        }
    }
}