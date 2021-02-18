using Autofac;
using Hangfire;
using Palavyr.API.Registration.Application;

namespace Palavyr.API.Registration.Autofac
{
    public class HangfireModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<RecurringJobManager>().As<IRecurringJobManager>().SingleInstance();
            builder.RegisterType<HangFireJobs>().AsSelf().SingleInstance();
        }
    }
}