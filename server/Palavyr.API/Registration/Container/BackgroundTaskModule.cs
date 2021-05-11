using Autofac;
using Palavyr.Core.BackgroundJobs;

namespace Palavyr.API.Registration.Container
{
    public class BackgroundTaskModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RemoveOldS3Archives>().As<IRemoveOldS3Archives>();
            builder.RegisterType<RemoveStaleSessions>().As<IRemoveStaleSessions>();
        }
    }
}