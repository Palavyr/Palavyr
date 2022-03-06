using System;
using Autofac;
using Palavyr.API.Controllers.Testing;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Common.FileSystemTools;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.ConversationServices;
using Palavyr.Core.Services.Deletion;
using Palavyr.Core.Services.DynamicTableService;
using Palavyr.Core.Services.DynamicTableService.Compilers;
using Palavyr.Core.Services.DynamicTableService.NodeUpdaters;
using Palavyr.Core.Services.DynamicTableService.Thresholds;
using Palavyr.Core.Services.EmailService.EmailResponse;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.EmailService.Verification;
using Palavyr.Core.Services.EnquiryServices;
using Palavyr.Core.Services.FileAssetServices;
using Palavyr.Core.Services.Localization;
using Palavyr.Core.Services.LogoServices;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Services.Units;
using Palavyr.Core.Sessions;
using Module = Autofac.Module;

namespace Palavyr.API.Registration.Container
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AccountRegistrationMaker>().As<IAccountRegistrationMaker>();
            builder.RegisterType<PalavyrAccessChecker>().As<IPalavyrAccessChecker>();
            builder.RegisterType<EmailVerificationStatus>().As<IEmailVerificationStatus>().InstancePerLifetimeScope();
            builder.RegisterType<LocaleDefinitions>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TestDataProvider>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<OrphanRemover>().As<IOrphanRemover>().InstancePerLifetimeScope();
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
            builder.RegisterType<ResponsePdfGenerator>().As<IResponsePdfGenerator>();
            builder.RegisterType<StaticTableCompiler>().As<IStaticTableCompiler>();
            builder.RegisterType<DynamicTableCompilerRetriever>().As<IDynamicTableCompilerRetriever>();

            builder.RegisterGeneric(typeof(GenericDynamicTableRepository<>)).As(typeof(IGenericDynamicTableRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DynamicTableCommandExecutor<>)).As(typeof(IDynamicTableCommandExecutor<>)).InstancePerLifetimeScope();

            builder.RegisterType<JwtAuthenticationService>().As<IJwtAuthenticationService>();
            builder.RegisterType<AccountSetupService>().As<IAccountSetupService>();
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<EmailVerificationService>().As<IEmailVerificationService>();
            builder.RegisterType<SesEmail>().As<ISesEmail>();
            builder.RegisterType<RequestEmailVerification>().As<IRequestEmailVerification>();
            builder.RegisterType<ResponsePdfGenerator>().As<IResponsePdfGenerator>();
            builder.RegisterType<Is3FileUploader>().As<IS3FileUploader>();
            builder.RegisterType<Is3FileDeleter>().As<IS3FileDeleter>();

            builder.RegisterType<ConversationOptionSplitter>().As<IConversationOptionSplitter>().SingleInstance();
            builder.RegisterType<WidgetStatusChecker>().As<IWidgetStatusChecker>();
            builder.RegisterType<MissingNodeCalculator>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<RequiredNodeCalculator>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TreeRootFinder>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<TreeWalker>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<NodeCounter>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GuidFinder>().AsSelf().SingleInstance();
            builder.RegisterType<ThresholdEvaluator>().As<IThresholdEvaluator>();
            builder.RegisterType<NodeOrderChecker>().As<INodeOrderChecker>();
            builder.RegisterType<DynamicResponseComponentExtractor>().As<IDynamicResponseComponentExtractor>();
            builder.RegisterType<AwsS3LinkCreator>().As<ILinkCreator>();
            builder.RegisterType<S3KeyResolver>().As<IS3KeyResolver>();
            builder.RegisterType<TemporaryPath>().As<ITemporaryPath>();
            builder.RegisterType<AttachmentRetriever>().As<IAttachmentRetriever>();
            builder.RegisterType<AttachmentDeleter>().As<IAttachmentDeleter>();
            builder.RegisterType<LogoAssetSaver>().As<ILogoAssetSaver>();
            builder.RegisterType<LogoDeleter>().As<ILogoDeleter>();
            builder.RegisterType<LogoRetriever>().As<ILogoRetriever>();
            builder.RegisterType<CriticalResponses>().As<ICriticalResponses>();
            builder.RegisterType<CompileSenderDetails>().As<ICompileSenderDetails>();
            builder.RegisterType<ResponseEmailSender>().As<IResponseEmailSender>();
            builder.RegisterType<Is3Downloader>().As<IS3Downloader>();
            builder.RegisterType<AreaDeleter>().As<IAreaDeleter>();
            builder.RegisterType<EnquiryDeleter>().As<IEnquiryDeleter>();
            builder.RegisterType<ConversationRecordRecordRetriever>().As<IConversationRecordRetriever>();
            builder.RegisterType<CompletedConversationModifier>().As<ICompletedConversationModifier>();
            builder.RegisterType<RemoveStaleSessions>().As<IRemoveStaleSessions>();
            builder.RegisterType<SafeFileNameCreator>().As<ISafeFileNameCreator>();
            builder.RegisterType<LocalIo>().As<ILocalIo>();
            builder.RegisterType<PdfServerClient>().As<IPdfServerClient>();
            builder.RegisterType<NodeFileAssetSaver>().As<INodeFileAssetSaver>();
            builder.RegisterType<FileAssetDeleter>().As<IFileAssetDeleter>();
            builder.RegisterType<NewAccountUtils>().As<INewAccountUtils>();
            builder.RegisterType<GuidUtils>().As<IGuidUtils>();
            builder.RegisterType<PlanTypeRetriever>().As<IPlanTypeRetriever>();
            builder.RegisterType<BusinessRules>().As<IBusinessRules>();
            builder.RegisterType<DetermineCurrentEnvironment>().As<IDetermineCurrentEnvironment>();
            builder.RegisterType<ConversationNodeUpdater>().As<IConversationNodeUpdater>();
            builder.RegisterType<SelectOneFlatNodeUpdater>().As<ISelectOneFlatNodeUpdater>();
            builder.RegisterType<NodeGetter>().As<INodeGetter>();
            builder.RegisterType<LocaleDefinitions>().As<ILocaleDefinitions>();
            builder.RegisterType<EnquiryInsightComputer>().As<IEnquiryInsightComputer>();
            builder.RegisterType<NodeBranchLengthCalculator>().As<INodeBranchLengthCalculator>();

            builder.RegisterType<DetermineCurrentOperatingSystem>().As<IDetermineCurrentOperatingSystem>();
            builder.RegisterType<ResponseRetriever>().As<IResponseRetriever>().InstancePerDependency();

            builder.RegisterType<CurrentLocaleAndLocalMapRetriever>().As<ICurrentLocaleAndLocalMapRetriever>().InstancePerLifetimeScope();

            builder.RegisterType<Units>().AsSelf().SingleInstance();
            builder.RegisterType<UnitRetriever>().As<IUnitRetriever>().InstancePerLifetimeScope();

            builder.RegisterType<EndingSequenceAttacher>().As<IEndingSequenceAttacher>().InstancePerLifetimeScope();
            builder.RegisterType<EndingSequenceNodes>().As<IEndingSequenceNodes>().InstancePerLifetimeScope();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IMapToNew<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IMapToPreExisting<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterType<AwsCloudFileSaver>().As<ICloudFileSaver>();
            builder.RegisterDecorator<CloudSaverWritesToDatabaseDecorator, ICloudFileSaver>();

            builder.RegisterType<AttachmentAssetSaver>().As<IAttachmentAssetSaver>();
            builder.RegisterDecorator<AttachmentSaverDecorator, IAttachmentAssetSaver>();


            // Experimental
            ///////!!! SPECIAL DANGER ZONE !!!//////////
            builder.RegisterType<AccountIdTransport>().As<IAccountIdTransport>().InstancePerLifetimeScope(); // DONT CHANGE THE LIFETIME SCOPE OF THIS TYPE
            builder.RegisterType<CancellationTokenTransport>().As<ICancellationTokenTransport>().InstancePerLifetimeScope(); // DONT CHANGE THE LIFETIME SCOPE OF THIS TYPE
            ///////////// ///////////// ////////// ////////// ////////// /////////// ///////////// ///////////// ////////// ////////// ////////// /////////// 
        }
    }
}