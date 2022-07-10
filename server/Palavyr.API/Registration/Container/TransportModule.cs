using Autofac;
using Palavyr.Core.Sessions;

namespace Palavyr.API.Registration.Container
{
    public class TransportModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Experimental
            ///////!!! SPECIAL DANGER ZONE !!!//////////
            builder.RegisterType<AccountIdTransport>().As<IAccountIdTransport>().InstancePerLifetimeScope(); // DONT CHANGE THE LIFETIME SCOPE OF THIS TYPE
            builder.RegisterType<CancellationTokenTransport>().As<ICancellationTokenTransport>().InstancePerLifetimeScope(); // DONT CHANGE THE LIFETIME SCOPE OF THIS TYPE
            ///////////// ///////////// ////////// ////////// ////////// /////////// ///////////// ///////////// ////////// ////////// ////////// /////////// 

            base.Load(builder);
        }
    }
}