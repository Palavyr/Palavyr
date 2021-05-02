using Autofac;
using Palavyr.BackupAndRestore;
using Palavyr.Core.BackgroundJobs;

namespace Palavyr.API.Registration.Container
{
    public class BackgroundTaskModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreatePalavyrSnapshot>().As<ICreatePalavyrSnapshot>();
            builder.RegisterType<RemoveOldS3Archives>().As<IRemoveOldS3Archives>();
            builder.RegisterType<RemoveStaleSessions>().As<IRemoveStaleSessions>();
        }
    }
}