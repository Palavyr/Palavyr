using System;
using System.Threading.Tasks;
using Autofac;
using NSubstitute;
using Palavyr.Core.Services.StripeServices;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Palavyr.IntegrationTests.DataCreators;
using Palavyr.IntegrationTests.DataCreators.StripeBuilders;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeHandlers
{
    public abstract class StripeServiceFixtureBase : InMemoryIntegrationFixture
    {
        protected StripeServiceFixtureBase(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }

        public DateTime CreatedAt = DateTime.Now;


        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            var priceRecurring = this.CreateStripeRecurringBuilder().WithMonthInterval().Build();
            var price = this.CreateStripePriceBuilder()
                .WithFreeProductId()
                .WithAmount(0)
                .WithPriceRecurring(priceRecurring)
                .Build();


            var subscription = this
                .CreateStripeSubscriptionBuilder()
                .WithPrice(price)
                .WithCurrentPeriodEnd(CreatedAt)
                .WithCustomerId(this.StripeCustomerId)
                .Build();


            builder.Register(
                ctx =>
                {
                    var sub = Substitute.For<IStripeSubscriptionRetriever>();
                    sub.GetSubscription(default).ReturnsForAnyArgs(subscription);
                    return sub;
                }).As<IStripeSubscriptionRetriever>();

            UseFakeStripeCustomerService(builder);

            return base.CustomizeContainer(builder);
        }


        public override Task InitializeAsync()
        {
            SetAccountId();
            SetCancellationToken();
            this.SetupFreeAccount();
            return base.InitializeAsync();
        }
    }
}