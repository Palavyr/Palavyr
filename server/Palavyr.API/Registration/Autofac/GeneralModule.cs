using Autofac;
using Palavyr.API.Services.AccountServices;
using Palavyr.Services.EmailService.Verification;

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