using System.Threading;
using Autofac;
using Palavyr.Core.Sessions;

namespace Component.AppFactory.ExtensionMethods
{
    public static class ContainerBuilderExtensionMethods
    {
        public static ContainerBuilder AddAccountIdAndCancellationToken(this ContainerBuilder containerBuilder, string accountId)
        {
            containerBuilder.AddAccountId(accountId).AddCancellationToken();
            return containerBuilder;
        }

        public static ContainerBuilder AddAccountId(this ContainerBuilder containerBuilder, string accountId)
        {
            containerBuilder.Register(
                c =>
                {
                    var holder = new AccountIdTransport();
                    holder.Assign(accountId);
                    return holder;
                }).As<IAccountIdTransport>().InstancePerLifetimeScope();
            return containerBuilder;
        }

        public static ContainerBuilder AddCancellationToken(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(
                c => { return new CancellationTokenTransport(new CancellationTokenSource().Token); }).As<ICancellationTokenTransport>().InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}