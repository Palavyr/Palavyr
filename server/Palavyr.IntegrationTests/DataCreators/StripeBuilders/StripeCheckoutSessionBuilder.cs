using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Stripe;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public class StripeCheckoutSessionBuilder
    {
        private readonly BaseIntegrationFixture test;
        private string subId;
        private string customerId;
        private Subscription sub;
        
        
        public StripeCheckoutSessionBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public StripeCheckoutSessionBuilder WithSubscriptionId(string id)
        {
            this.subId = id;
            return this;
        }

        public StripeCheckoutSessionBuilder WithCustomerId(string custId)
        {
            this.customerId = custId;
            return this;
        }

        public StripeCheckoutSessionBuilder AsFreeSubscription(string custId)
        {
            this.customerId = custId;
            this.sub = test.CreateStripeSubscriptionBuilder()
                .WithPrice(test.CreateStripePriceBuilder().WithFreeProductId().Build())
                .WithCustomerId(custId)
                .Build();
            return this;
        }

        public StripeCheckoutSessionBuilder AsProSubscription(string custId)
        {
            this.customerId = custId;
            this.sub = test.CreateStripeSubscriptionBuilder()
                .WithPrice(test.CreateStripePriceBuilder().WithProProductId().Build())
                .WithCustomerId(custId)
                .Build();
            return this;
        }
  

        public Stripe.Checkout.Session Build()
        {
            var subscriptionId = this.subId ?? A.RandomId();
            var custId = this.customerId ?? test.StripeCustomerId;
            var subs = this.sub ?? test.CreateStripeSubscriptionBuilder().Build();

            return new Stripe.Checkout.Session()
            {
                SubscriptionId = subscriptionId,
                CustomerId = custId,
                Subscription = subs
            };
        }
    }
}