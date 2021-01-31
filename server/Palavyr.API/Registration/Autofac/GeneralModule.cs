using Autofac;
using EmailService.Verification;
using Palavyr.API.Services.AccountServices;

namespace Palavyr.API.Registration.Autofac
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailVerificationStatus>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<LocaleDefinition>().AsSelf().InstancePerLifetimeScope();
        }
    }
}