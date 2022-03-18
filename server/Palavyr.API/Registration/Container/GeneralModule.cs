using System;
using Autofac;
using MediatR;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Common.FileSystemTools;
using Palavyr.Core.Common.UniqueIdentifiers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Mappers;
using Palavyr.Core.Models;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Conversation;
using Palavyr.Core.Models.Nodes;
using Palavyr.Core.Models.Resources.Responses;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;
using Palavyr.Core.Services.AttachmentServices;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.CloudKeyResolvers;
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
using Palavyr.Core.Services.FileAssetServices.FileAssetLinkers;
using Palavyr.Core.Services.Localization;
using Palavyr.Core.Services.LogoServices;
using Palavyr.Core.Services.PdfService;
using Palavyr.Core.Services.PdfService.PdfSections.Util;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.TemporaryPaths;
using Palavyr.Core.Services.Units;
using Palavyr.Core.Sessions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.Delete;
using Module = Autofac.Module;

namespace Palavyr.API.Registration.Container
{
    public class GeneralModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Experimental
            ///////!!! SPECIAL DANGER ZONE !!!//////////
            builder.RegisterType<AccountIdTransport>().As<IAccountIdTransport>().InstancePerLifetimeScope(); // DONT CHANGE THE LIFETIME SCOPE OF THIS TYPE
            builder.RegisterType<CancellationTokenTransport>().As<ICancellationTokenTransport>().InstancePerLifetimeScope(); // DONT CHANGE THE LIFETIME SCOPE OF THIS TYPE
            ///////////// ///////////// ////////// ////////// ////////// /////////// ///////////// ///////////// ////////// ////////// ////////// /////////// 

            builder.RegisterType<DetermineCurrentOperatingSystem>().As<IDetermineCurrentOperatingSystem>();
            builder.RegisterType<Units>().AsSelf().SingleInstance();

            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IMapToNew<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IMapToPreExisting<,>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder
                .RegisterGeneric(typeof(EntityStore<>))
                .As(typeof(IEntityStore<>))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            
            
            builder.RegisterGeneric(typeof(PricingStrategyEntityStore<>)).As(typeof(IPricingStrategyEntityStore<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DynamicTableCommandExecutor<>)).As(typeof(IDynamicTableCommandExecutor<>)).InstancePerLifetimeScope();

            builder.RegisterType<MissingNodeCalculator>().As<IMissingNodeCalculator>();
            builder.RegisterType<RequiredNodeCalculator>().As<IRequiredNodeCalculator>();
            builder.RegisterType<TreeRootFinder>().As<ITreeRootFinder>();
            builder.RegisterType<TreeWalker>().As<ITreeWalker>();
            builder.RegisterType<NodeCounter>().As<INodeCounter>();
            builder.RegisterType<AccountRegistrationMaker>().As<IAccountRegistrationMaker>();
            builder.RegisterType<AccountSetupService>().As<IAccountSetupService>();
            builder.RegisterType<AreaDeleter>().As<IAreaDeleter>();
            builder.RegisterType<AttachmentAssetSaver>().As<IAttachmentAssetSaver>();
            builder.RegisterType<AttachmentDeleter>().As<IAttachmentDeleter>();
            builder.RegisterType<AttachmentLinker>().As<IFileAssetLinker<AttachmentLinker>>();
            builder.RegisterType<AttachmentRetriever>().As<IAttachmentRetriever>();
            builder.RegisterType<AuthService>().As<IAuthService>();
            builder.RegisterType<AwsCloudFileSaver>().As<ICloudFileSaver>();
            builder.RegisterType<AwsS3LinkCreator>().As<ILinkCreator>();
            builder.RegisterType<S3PreSignedUrlCreator>().As<IS3PreSignedUrlCreator>();
            builder.RegisterType<BasicThresholdCompiler>().As<IBasicThresholdCompiler>();
            builder.RegisterType<BillingPortalSession>().As<IBillingPortalSession>();
            builder.RegisterType<BusinessRules>().As<IBusinessRules>();
            builder.RegisterType<CategoryNestedThresholdCompiler>().As<ICategoryNestedThresholdCompiler>();
            builder.RegisterType<CloudDeleter>().As<ICloudDeleter>();
            builder.RegisterType<CloudFileDownloader>().As<ICloudFileDownloader>();
            builder.RegisterType<CloudCompatibleKeyResolver>().As<ICloudCompatibleKeyResolver>();
            builder.RegisterType<CompileSenderDetails>().As<ICompileSenderDetails>();
            builder.RegisterType<CompletedConversationModifier>().As<ICompletedConversationModifier>();
            builder.RegisterType<ConversationNodeUpdater>().As<IConversationNodeUpdater>();
            builder.RegisterType<ConversationOptionSplitter>().As<IConversationOptionSplitter>();
            builder.RegisterType<ConversationRecordRetriever>().As<IConversationRecordRetriever>();
            builder.RegisterType<CriticalResponses>().As<ICriticalResponses>();
            builder.RegisterType<CurrentLocaleAndLocaleMapRetriever>().As<ICurrentLocaleAndLocaleMapRetriever>();
            builder.RegisterType<CustomerSessionService>().As<ICustomerSessionService>();
            builder.RegisterType<DangerousAccountDeleter>().As<IDangerousAccountDeleter>();
            builder.RegisterType<DetermineCurrentEnvironment>().As<IDetermineCurrentEnvironment>();
            builder.RegisterType<DynamicResponseComponentExtractor>().As<IDynamicResponseComponentExtractor>();
            builder.RegisterType<DynamicTableCompilerOrchestrator>().As<IDynamicTableCompilerOrchestrator>();
            builder.RegisterType<DynamicTableCompilerRetriever>().As<IDynamicTableCompilerRetriever>();
            builder.RegisterType<EmailVerificationService>().As<IEmailVerificationService>();
            builder.RegisterType<EmailVerificationStatus>().As<IEmailVerificationStatus>();
            builder.RegisterType<EndingSequenceAttacher>().As<IEndingSequenceAttacher>();
            builder.RegisterType<EndingSequenceNodes>().As<IEndingSequenceNodes>();
            builder.RegisterType<EnquiryDeleter>().As<IEnquiryDeleter>();
            builder.RegisterType<EnquiryInsightComputer>().As<IEnquiryInsightComputer>();
            builder.RegisterType<FileAssetDeleter>().As<IFileAssetDeleter>();
            builder.RegisterType<CompilePdfServerRequest>().As<ICompilePdfServerRequest>();

            builder.RegisterType<FileAssetKeyResolver>().As<IFileAssetKeyResolver>();
            builder.RegisterType<NodeLinker>().As<IFileAssetLinker<NodeLinker>>();
            builder.RegisterType<LogoLinker>().As<IFileAssetLinker<LogoLinker>>();
            builder.RegisterType<FileAssetSaver>().As<IFileAssetSaver>();
            builder.RegisterType<GuidFinder>().As<IGuidFinder>();
            builder.RegisterType<GuidUtils>().As<IGuidUtils>();
            builder.RegisterType<HtmlToPdfClient>().As<IHtmlToPdfClient>();
            builder.RegisterType<JwtAuthenticationService>().As<IJwtAuthenticationService>();
            builder.RegisterType<LocaleDefinitions>().As<ILocaleDefinitions>();
            builder.RegisterType<LocalIo>().As<ILocalIo>();
            builder.RegisterType<LogoAssetSaver>().As<ILogoAssetSaver>();
            builder.RegisterType<LogoDeleter>().As<ILogoDeleter>();
            builder.RegisterType<LogoRetriever>().As<ILogoRetriever>();
            builder.RegisterType<NewAccountUtils>().As<INewAccountUtils>();
            builder.RegisterType<NodeBranchLengthCalculator>().As<INodeBranchLengthCalculator>();
            builder.RegisterType<NodeFileAssetSaver>().As<INodeFileAssetSaver>();
            builder.RegisterType<NodeGetter>().As<INodeGetter>();
            builder.RegisterType<NodeOrderChecker>().As<INodeOrderChecker>();
            builder.RegisterType<OrphanRemover>().As<IOrphanRemover>();
            builder.RegisterType<PalavyrAccessChecker>().As<IPalavyrAccessChecker>();
            builder.RegisterType<PdfResponseKeyResolver>().As<IPdfResponseKeyResolver>();
            builder.RegisterType<PdfServerClient>().As<IPdfServerClient>();
            builder.RegisterType<PercentOfThresholdCompiler>().As<IPercentOfThresholdCompiler>();
            builder.RegisterType<PlanTypeRetriever>().As<IPlanTypeRetriever>();
            builder.RegisterType<PreviewResponseGenerator>().As<IPreviewResponseGenerator>();
            builder.RegisterType<RemoveStaleSessions>().As<IRemoveStaleSessions>();
            builder.RegisterType<RequestEmailVerification>().As<IRequestEmailVerification>();
            builder.RegisterType<ResponseCustomizer>().As<IResponseCustomizer>();
            builder.RegisterType<ResponseEmailSender>().As<IResponseEmailSender>();
            builder.RegisterType<ResponseHtmlBuilder>().As<IResponseHtmlBuilder>();
            builder.RegisterType<ResponsePdfGenerator>().As<IResponsePdfGenerator>();
            builder.RegisterType<ResponsePdfPreviewKeyResolver>().As<IResponsePdfPreviewKeyResolver>();
            builder.RegisterType<ResponsePdfTableCompiler>().As<IResponsePdfTableCompiler>();
            builder.RegisterType<ResponseRetriever>().As<IResponseRetriever>();
            builder.RegisterType<Is3FileUploader>().As<IS3FileUploader>();
            builder.RegisterType<Is3FileDeleter>().As<IS3FileDeleter>();

            builder.RegisterType<S3Downloader>().As<IS3Downloader>();
            builder.RegisterType<SafeFileNameCreator>().As<ISafeFileNameCreator>();
            builder.RegisterType<SelectOneFlatCompiler>().As<ISelectOneFlatCompiler>();
            builder.RegisterType<SelectOneFlatNodeUpdater>().As<ISelectOneFlatNodeUpdater>();
            builder.RegisterType<SesEmail>().As<ISesEmail>();

            builder.RegisterType<StaticTableCompiler>().As<IStaticTableCompiler>();
            builder.RegisterType<TemporaryPath>().As<ITemporaryPath>();
            builder.RegisterType<ThresholdEvaluator>().As<IThresholdEvaluator>();
            builder.RegisterType<TwoNestedCategoryCompiler>().As<ITwoNestedCategoryCompiler>();
            builder.RegisterType<UnitRetriever>().As<IUnitRetriever>();
            builder.RegisterType<WidgetStatusChecker>().As<IWidgetStatusChecker>();
            builder.RegisterType<UnitOfWorkContextProvider>().As<IUnitOfWorkContextProvider>();
            
            builder.RegisterDecorator<FileAssetDeleterDeleteDatabaseRecordDecorator, IFileAssetDeleter>();
            builder.RegisterDecorator<FileAssetDeleterDereferenceConvoNodesDecorator, IFileAssetDeleter>();
            builder.RegisterDecorator<FileAssetSaverDatabaseDecorator, IFileAssetSaver>();
            builder.RegisterDecorator<HtmlToPdfClientFileAssetCreatingDecorator, IHtmlToPdfClient>();
            builder.RegisterDecorator<LogoAssetSaverDatabaseUpdaterDecorator, ILogoAssetSaver>();
            builder.RegisterDecorator<ResponseHtmlCustomizationDecorator, IResponseHtmlBuilder>();
            builder.RegisterDecorator<ResponsePdfGeneratorUpdateConversationRecordDecorator, IResponsePdfGenerator>();
            
        }
    }
}