using Autofac;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.API.Controllers.Testing;
using Palavyr.BackupAndRestore;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.Domain.Conversation;
using Palavyr.Domain.Resources.Responses;
using Palavyr.Services.AccountServices;
using Palavyr.Services.AmazonServices.S3Service;
using Palavyr.Services.AuthenticationServices;
using Palavyr.Services.ConversationServices;
using Palavyr.Services.DynamicTableService;
using Palavyr.Services.DynamicTableService.Compilers;
using Palavyr.Services.EmailService.ResponseEmailTools;
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

            
            builder.RegisterType<JwtAuthenticationService>().As<IJwtAuthenticationService>();
            builder.RegisterType<AccountSetupService>().As<IAccountSetupService>();
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<EmailVerificationService>().As<IEmailVerificationService>();
            builder.RegisterType<SesEmail>().As<ISesEmail>();
            builder.RegisterType<RequestEmailVerification>().As<IRequestEmailVerification>();
            builder.RegisterType<PdfResponseGenerator>().As<IPdfResponseGenerator>();
            builder.RegisterType<S3Saver>().As<IS3Saver>();
            builder.RegisterType<PostgresBackup>().As<IPostgresBackup>();
            builder.RegisterType<UserDataBackup>().As<IUserDataBackup>();
            builder.RegisterType<UpdateDatabaseLatest>().As<IUpdateDatabaseLatest>();
        }
    }
}