using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Palavyr.IntegrationTests.DataCreators.StripeBuilders;

namespace Palavyr.IntegrationTests.DataCreators
{
    public static partial class BuilderExtensionMethods
    {
        public static StripeSubscriptionBuilder CreateStripeSubscriptionBuilder(this BaseIntegrationFixture test)
        {
            return new StripeSubscriptionBuilder(test);
        }

        public static StripePriceBuilder CreateStripePriceBuilder(this BaseIntegrationFixture test)
        {
            return new StripePriceBuilder(test);
        }

        public static StripePriceRecurringBuilder CreateStripeRecurringBuilder(this BaseIntegrationFixture test)
        {
            return new StripePriceRecurringBuilder(test);
        }

        public static StripeBillingSessionBuilder CreateStripeBillingSessionBuilder(this BaseIntegrationFixture test)
        {
            return new StripeBillingSessionBuilder(test);
        }

        public static StripeCheckoutSessionBuilder CreateStripeCheckoutSessionBuilder(this BaseIntegrationFixture test)
        {
            return new StripeCheckoutSessionBuilder(test);
        }
    }
}