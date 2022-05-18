using System.Threading.Tasks;
using Autofac;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.DataCreators.StripeBuilders;
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
        private TestProductRegistry registry = null!;

        [Fact]
        public async Task TheProcessCheckoutSessionCompletedUpdatesTheAccount()
        {
            var subscriptionId = A.RandomId();
            var session = this.CreateStripeCheckoutSessionBuilder()
                .WithSubscriptionId(subscriptionId)
                .Build();

            var handler = ResolveType<INotificationHandler<CheckoutSessionCompletedNotification>>();

            await handler.Handle(new CheckoutSessionCompletedNotification(session), CancellationToken);
            
            var accountStore = ResolveStore<Account>();
            var account = await accountStore.Get(accountStore.AccountId, s => s.AccountId);
            
            account.PlanType.ShouldBe(Account.PlanTypeEnum.Pro);
            account.HasUpgraded.ShouldBeTrue();
            account.CurrentPeriodEnd.ShouldBeEquivalentTo(CreatedAt.AddMonths(1));
        }

        public override Task InitializeAsync()
        {
            logger = ResolveType<ILogger<ProcessStripeCheckoutSessionCompletedHandler>>();
            service = ResolveType<IStripeSubscriptionService>();
            registry = new TestProductRegistry();
            return base.InitializeAsync();
        }

        public override ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            var priceRecurring = this.CreateStripeRecurringBuilder().WithMonthInterval().Build();
            var price = this.CreateStripePriceBuilder()
                .WithProProductId()
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
                    sub.GetSubscription(default!).ReturnsForAnyArgs(subscription);
                    return sub;
                }).As<IStripeSubscriptionRetriever>();

            return base.CustomizeContainer(builder);
        }

        public ProcessCheckoutSessionCompletedHandlerFixtureBase(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory) : base(testOutputHelper, factory)
        {
        }
    }
}