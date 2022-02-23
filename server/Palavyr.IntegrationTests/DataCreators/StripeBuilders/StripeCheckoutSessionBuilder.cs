using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public class StripeCheckoutSessionBuilder
    {
        private readonly BaseIntegrationFixture test;
        private string subId;
        private string customerId;

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

        public Stripe.Checkout.Session Build()
        {
            var subscriptionId = this.subId ?? A.RandomId();
            var custId = this.customerId ?? test.StripeCustomerId;

            return new Stripe.Checkout.Session()
            {
                SubscriptionId = subscriptionId,
                CustomerId = custId
            };
        }
    }
}