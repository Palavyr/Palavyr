using Autofac;
using EmailService.Verification;

namespace Palavyr.BackupAndRestore.Modules
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestEmailVerification>().As<IRequestEmailVerification>();
            builder.RegisterType<EmailVerificationStatus>().AsSelf();
        }
    }
}