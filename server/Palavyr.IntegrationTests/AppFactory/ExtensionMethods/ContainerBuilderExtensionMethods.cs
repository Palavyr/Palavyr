using System.Threading;
using Autofac;
using Palavyr.Core.Sessions;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
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
                }).As<IHoldAnAccountId>().InstancePerLifetimeScope();
            return containerBuilder;
        }

        public static ContainerBuilder AddCancellationToken(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Register(
                c =>
                {
                    var ctx = new CancellationTokenSource();
                    var holder = new CancellationTokenTransport();
                    holder.Assign(ctx.Token);
                    return holder;
                }).As<ITransportACancellationToken>().InstancePerLifetimeScope();

            return containerBuilder;
        }
    }
}