// using Autofac;
// using Component.AppFactory.AutofacWebApplicationFactory;
// using Component.AppFactory.IntegrationTestFixtures.BaseFixture;
// using Component.Mocks;
// using Palavyr.Core.Services.AccountServices;
// using Palavyr.Core.Services.EmailService.ResponseEmailTools;
// using Palavyr.Core.Services.StripeServices;
// using Xunit.Abstractions;
//
// namespace Component.AppFactory.IntegrationTestFixtures
// {
//     public abstract class ComponentTest : ComponentTestBase
//     {
//         protected ComponentTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
//         {
//         }
//
//         public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
//         {
//             builder.RegisterType<MockEmailVerificationService>().As<IEmailVerificationService>();
//             builder.RegisterType<MockStripeWebhookAuthService>().As<IStripeWebhookAuthService>();
//             builder.RegisterType<MockStripeSubscriptionSetter>().As<IStripeSubscriptionSetter>();
//             builder.RegisterType<MockSeSEmail>().As<ISesEmail>();
//
//             return base.CustomizeContainer(builder);
//         }
//     }
// }