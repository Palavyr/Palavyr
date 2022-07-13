using Test.Common.Random;

namespace Test.Common.Builders.StripeBuilders
{
    public class StripeBillingSessionBuilder
    {
        private string? custId;

        public StripeBillingSessionBuilder()
        {
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