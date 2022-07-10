using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public class StripeBillingSessionBuilder
    {
        private readonly IntegrationTest test;
        private string? custId;

        public StripeBillingSessionBuilder(IntegrationTest test)
        {
            this.test = test;
        }

        public StripeBillingSessionBuilder WithCustomerId(string id)
        {
            this.custId = id;
            return this;
        }

        public Stripe.BillingPortal.Session Build()
        {
            var customerId = this.custId ?? A.RandomId();
            return new Stripe.BillingPortal.Session
            {
                Customer = customerId,
            };
        }
    }
}