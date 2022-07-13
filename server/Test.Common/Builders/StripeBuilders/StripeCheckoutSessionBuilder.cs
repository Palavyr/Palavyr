using Stripe;
using Test.Common.Random;

namespace Test.Common.Builders.StripeBuilders
{
    public class StripeCheckoutSessionBuilder
    {
        private readonly IHaveStripeCustomerId test;
        private string subId;
        private string customerId;
        private Subscription sub;
        
        
        public StripeCheckoutSessionBuilder(IHaveStripeCustomerId test)
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
            this.sub = new StripeSubscriptionBuilder(test)
                .WithPrice(new StripePriceBuilder().WithFreeProductId().Build())
                .WithCustomerId(custId)
                .Build();
            return this;
        }

        public StripeCheckoutSessionBuilder AsProSubscription(string custId)
        {
            this.customerId = custId;
            this.sub = new StripeSubscriptionBuilder(test)
                .WithPrice(new StripePriceBuilder().WithProProductId().Build())
                .WithCustomerId(custId)
                .Build();
            return this;
        }
  

        public Stripe.Checkout.Session Build()
        {
            var subscriptionId = this.subId ?? A.RandomId();
            var custId = this.customerId ?? test.StripeCustomerId;
            var subs = this.sub ?? new StripeSubscriptionBuilder(test).Build();

            return new Stripe.Checkout.Session()
            {
                SubscriptionId = subscriptionId,
                CustomerId = custId,
                Subscription = subs,
                
            };
        }
    }
}