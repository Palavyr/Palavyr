using Autofac;
using Palavyr.BackupAndRestore.Jobs;
using Palavyr.Services.EmailService.Verification;

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