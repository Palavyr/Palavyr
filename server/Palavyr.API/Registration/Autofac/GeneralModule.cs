using Autofac;
using EmailService.Verification;

namespace Palavyr.API.Registration.Autofac
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailVerificationStatus>().AsSelf().InstancePerLifetimeScope();
        }
    }
}