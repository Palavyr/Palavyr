using Autofac;
using MediatR;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.Mocks;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures
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