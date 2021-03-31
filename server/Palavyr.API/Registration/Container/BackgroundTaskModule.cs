using Autofac;
using Palavyr.Background;

namespace Palavyr.API.Registration.Container
{
    public class BackgroundTaskModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreatePalavyrSnapshot>().As<ICreatePalavyrSnapshot>();
            builder.RegisterType<RemoveOldS3Archives>().As<IRemoveOldS3Archives>();
            builder.RegisterType<RemoveStaleSessions>().As<IRemoveStaleSessions>();
            builder.RegisterType<ValidateAttachments>().As<IValidateAttachments>();
        }
    }
}