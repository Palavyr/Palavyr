using Autofac;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using IntegrationTests.Mocks;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Xunit.Abstractions;

namespace IntegrationTests.AppFactory.IntegrationTestFixtures
{
    public abstract class IntegrationTest<DbType> : IntegrationTestBase<DbType> where DbType : DbTypes
    {
        protected IntegrationTest(ITestOutputHelper testOutputHelper, ServerFactory factory) : base(testOutputHelper, factory)
        {
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            builder.RegisterType<MockEmailVerificationService>().As<IEmailVerificationService>();
            builder.RegisterType<MockStripeWebhookAuthService>().As<IStripeWebhookAuthService>();
            builder.RegisterType<MockStripeSubscriptionSetter>().As<IStripeSubscriptionSetter>();
            builder.RegisterType<MockSeSEmail>().As<ISesEmail>();

            return base.CustomizeContainer(builder);
        }   
    }
}