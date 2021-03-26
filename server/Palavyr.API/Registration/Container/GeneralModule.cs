using Autofac;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.API.Controllers.Testing;
using Palavyr.Domain.Conversation;
using Palavyr.Services.AccountServices;
using Palavyr.Services.ConversationServices;
using Palavyr.Services.EmailService.Verification;
using Palavyr.Services.Repositories;

namespace Palavyr.API.Registration.Container
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EmailVerificationStatus>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<LocaleDefinition>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TestDataProvider>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<OrphanRemover>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CompletedConversationRetriever>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CompletedConversationModifier>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(GenericDynamicTableRepository<>)).As(typeof(IGenericDynamicTableRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DynamicTableCommandHandler<>)).As(typeof(IDynamicTableCommandHandler<>)).InstancePerLifetimeScope();
        }
    }
}