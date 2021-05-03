using Autofac;
using Palavyr.API.Controllers.Response.Tables.Dynamic;
using Palavyr.API.Controllers.Testing;
using Palavyr.API.Controllers.WidgetLive;
using Palavyr.BackupAndRestore;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.ConversationServices;
using Palavyr.Core.Services.Deletion;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Services.DynamicTableService.Compilers;
using Palavyr.Core.Services.DynamicTableService.Thresholds;
using Palavyr.Core.Services.EmailService.EmailResponse;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.LogoServices;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;

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
            builder.RegisterType<CategoryNestedThresholdCompiler>().AsSelf();

            builder.RegisterType<PreviewResponseGenerator>().As<IPreviewResponseGenerator>();
            builder.RegisterType<PdfResponseGenerator>().As<IPdfResponseGenerator>();
            builder.RegisterType<StaticTableCompiler>().As<IStaticTableCompiler>();
            builder.RegisterType<DynamicTableCompilerRetriever>().AsSelf();

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
            builder.RegisterType<S3Deleter>().As<IS3Deleter>();

            builder.RegisterType<PostgresBackup>().As<IPostgresBackup>();
            builder.RegisterType<UpdateDatabaseLatest>().As<IUpdateDatabaseLatest>();

            builder.RegisterType<NodeGetter>().AsSelf();
            builder.RegisterType<ConversationOptionSplitter>().As<IConversationOptionSplitter>().SingleInstance();
            builder.RegisterType<WidgetStatusUtils>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<MissingNodeCalculator>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RequiredNodeCalculator>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TreeRootFinder>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TreeWalker>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<NodeCounter>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GuidFinder>().AsSelf().SingleInstance();
            builder.RegisterType<ThresholdEvaluator>().As<IThresholdEvaluator>();
            builder.RegisterType<NodeOrderChecker>().AsSelf();
            builder.RegisterType<DynamicResponseComponentExtractor>().AsSelf();
            builder.RegisterType<LinkCreator>().As<ILinkCreator>();
            builder.RegisterType<AttachmentSaver>().As<IAttachmentSaver>();
            builder.RegisterType<S3KeyResolver>().As<IS3KeyResolver>();
            builder.RegisterType<TempPathCreator>().As<ITempPathCreator>();
            builder.RegisterType<AttachmentRetriever>().As<IAttachmentRetriever>();
            builder.RegisterType<AttachmentDeleter>().As<IAttachmentDeleter>();
            builder.RegisterType<LocalFileDeleter>().As<ILocalFileDeleter>();
            builder.RegisterType<LogoSaver>().As<ILogoSaver>();
            builder.RegisterType<LogoDeleter>().As<ILogoDeleter>();
            builder.RegisterType<LogoRetriever>().As<ILogoRetriever>();
            builder.RegisterType<CriticalResponses>().As<ICriticalResponses>();
            builder.RegisterType<CompileSenderDetails>().As<ICompileSenderDetails>();
            builder.RegisterType<ResponseEmailSender>().As<IResponseEmailSender>();
            builder.RegisterType<S3Retriever>().As<IS3Retriever>();
            builder.RegisterType<AreaDeleter>().As<IAreaDeleter>();
        }
    }
}