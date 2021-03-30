using Autofac;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.API.Controllers.Testing;
using Palavyr.Domain.Conversation;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.AccountServices;
using Palavyr.Services.ConversationServices;
using Palavyr.Services.DynamicTableService;
using Palavyr.Services.DynamicTableService.Compilers;
using Palavyr.Services.EmailService.Verification;
using Palavyr.Services.PdfService;
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
            builder.RegisterType<HtmlToPdfClient>().As<IHtmlToPdfClient>().InstancePerLifetimeScope();
            builder.RegisterType<ResponseCustomizer>().As<IResponseCustomizer>();
            builder.RegisterType<ResponseHtmlBuilder>().As<IResponseHtmlBuilder>();
            builder.RegisterType<DynamicTableCompilerOrchestrator>().As<IDynamicTableCompilerOrchestrator>().InstancePerLifetimeScope();
            
            builder.RegisterType<SelectOneFlatCompiler>().AsSelf();
            builder.RegisterType<PercentOfThresholdCompiler>().AsSelf();
            builder.RegisterType<BasicThresholdCompiler>().AsSelf();
            builder.RegisterType<TwoNestedCategoryCompiler>().AsSelf();

            
            builder.RegisterType<PreviewResponseGenerator>().As<IPreviewResponseGenerator>();
            builder.RegisterType<PdfResponseGenerator>().As<IPdfResponseGenerator>();
            builder.RegisterType<StaticTableCompiler>().As<IStaticTableCompiler>();
            
            builder.RegisterGeneric(typeof(GenericDynamicTableRepository<>)).As(typeof(IGenericDynamicTableRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DynamicTableCommandHandler<>)).As(typeof(IDynamicTableCommandHandler<>)).InstancePerLifetimeScope();

        }
    }
}