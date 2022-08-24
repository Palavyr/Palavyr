using System.Threading.Tasks;
using Autofac;
using MediatR;
using NSubstitute;
using Palavyr.Component.ComponentTestBase;
using Palavyr.Core.Data;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Shouldly;
using Test.Common.Builders.Accounts;
using Test.Common.Builders.StripeBuilders;
using Test.Common.Random;
using Xunit;
using Account = Palavyr.Core.Data.Entities.Account;

namespace Palavyr.Component.Tests.StripeWebhookHandlers
{
    public class ProcessCheckoutSessionCompletedHandlerTestBaseBase : ComponentTest
    {
        public const string RoundingString = "M/d/yyyy hh:00:00";

        // [Fact]
        public async Task TheProcessCheckoutSessionCompletedUpdatesTheAccount()
        {
            var subscriptionId = A.RandomId();
            var customerId = A.RandomId();
            await new AccountObjectBuilder()
                .WithAccountId(AccountId)
                .WithStripeCustomerId(customerId)
                .BuildAndMakeRaw(ResolveType<AppDataContexts>(), CancellationToken);

            var session = new StripeCheckoutSessionBuilder(this)
                .WithSubscriptionId(subscriptionId)
                .AsProSubscription(customerId)
                .WithCustomerId(customerId)
                .Build();
            var handler = ResolveType<INotificationHandler<CheckoutSessionCompletedNotification>>();

            await handler.Handle(new CheckoutSessionCompletedNotification(session), CancellationToken);


            var accountStore = ResolveStore<Account>();
            var account = await accountStore.Get(accountStore.AccountId, s => s.AccountId);

            account.PlanType.ShouldBe(Account.PlanTypeEnum.Pro);
            account.HasUpgraded.ShouldBeTrue();
            account.CurrentPeriodEnd.ToString(RoundingString)
                .ShouldBeEquivalentTo(session.Subscription.CurrentPeriodEnd.AddYears(100).ToString(RoundingString));
        }

        public override void OverrideCustomization(ContainerBuilder builder)
        {
            var priceRecurring = new StripePriceRecurringBuilder().WithMonthInterval().Build();
            var price = new StripePriceBuilder()
                .WithProProductId()
                .WithAmount(0)
                .WithPriceRecurring(priceRecurring)
                .Build();

            var subscription = new StripeSubscriptionBuilder(this)
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

            base.OverrideCustomization(builder);
        }

        public ProcessCheckoutSessionCompletedHandlerTestBaseBase(ComponentClassFixture fixture) : base(fixture)
        {
        }
    }
}