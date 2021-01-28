using Autofac;
using EmailService.Verification;
using Palavyr.BackupAndRestore.Jobs;

namespace Palavyr.BackupAndRestore.Modules
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestEmailVerification>().As<IRequestEmailVerification>();
            builder.RegisterType<EmailVerificationStatus>().AsSelf();
            builder.RegisterType<EmailStatusCheckJob>().AsSelf();
        }
    }
}