using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataCreators;
using Shouldly;
using Test.Common.Random;
using Xunit;
using Xunit.Abstractions;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;

namespace Palavyr.IntegrationTests.Tests.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessCheckoutSessionCompletedHandlerFixtureBase : StripeServiceFixtureBase
    {
        private IStripeSubscriptionService service = null!;
        private ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger = null!;
        private StagingProductRegistry registry = null!;

        [Fact]
        public async Task TheProcessCheckoutSessionCompletedUpdatesTheAccount()
        {
            var subscriptionId = A.RandomId();
            var session = this.CreateStripeCheckoutSessionBuilder()
                .WithSubscriptionId(subscriptionId)
                .WithCustomerId(StripeCustomerId)
                .Build();

            var checkoutEvent = new CheckoutSessionCompletedEvent(session);
            var handler = new ProcessStripeCheckoutSessionCompletedHandler(AccountsContext, registry, logger, service);

            await handler.Handle(checkoutEvent, CancellationToken);

            var account = await AccountRepository.GetAccount();

            account.PlanType.ShouldBe(Account.PlanTypeEnum.Free);
            account.HasUpgraded.ShouldBeTrue();
            account.CurrentPeriodEnd.ShouldBeEquivalentTo(CreatedAt.AddMonths(1));
        }

        public override Task InitializeAsync()
        {
            logger = Container.GetService<ILogger<ProcessStripeCheckoutSessionCompletedHandler>>();
            service = Container.GetService<IStripeSubscriptionService>();
            registry = new StagingProductRegistry();
            return base.InitializeAsync();
        }

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

            return base.CustomizeContainer(builder);
        }

        public ProcessCheckoutSessionCompletedHandlerFixtureBase(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}