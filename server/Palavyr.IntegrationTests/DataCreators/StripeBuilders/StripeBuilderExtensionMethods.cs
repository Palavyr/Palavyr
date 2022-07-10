using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public static partial class StripeBuilderExtensionMethods
    {
        public static StripeSubscriptionBuilder CreateStripeSubscriptionBuilder(this IntegrationTest test)
        {
            return new StripeSubscriptionBuilder(test);
        }

        public static StripePriceBuilder CreateStripePriceBuilder(this IntegrationTest test)
        {
            return new StripePriceBuilder(test);
        }

        public static StripePriceRecurringBuilder CreateStripeRecurringBuilder(this IntegrationTest test)
        {
            return new StripePriceRecurringBuilder(test);
        }

        public static StripeBillingSessionBuilder CreateStripeBillingSessionBuilder(this IntegrationTest test)
        {
            return new StripeBillingSessionBuilder(test);
        }

        public static StripeCheckoutSessionBuilder CreateStripeCheckoutSessionBuilder(this IntegrationTest test)
        {
            return new StripeCheckoutSessionBuilder(test);
        }

        public static StripeInvoiceBuilder CreateStripeInvoiceBuilder(this IntegrationTest test)
        {
            return new StripeInvoiceBuilder(test);
        }
    }
}