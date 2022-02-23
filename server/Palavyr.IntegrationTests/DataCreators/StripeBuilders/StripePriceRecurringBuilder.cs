using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Stripe;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public class StripePriceRecurringBuilder
    {
        private readonly BaseIntegrationFixture test;
        private string? interval;

        public StripePriceRecurringBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public StripePriceRecurringBuilder WithMonthInterval()
        {
            this.interval = Palavyr.Core.Models.Accounts.Schemas.Account.PaymentIntervals.Month;
            return this;
        }

        public StripePriceRecurringBuilder WithYearInterval()
        {
            this.interval = Palavyr.Core.Models.Accounts.Schemas.Account.PaymentIntervals.Month;
            return this;
        }

        public PriceRecurring Build()
        {
            var interval = this.interval ?? Palavyr.Core.Models.Accounts.Schemas.Account.PaymentIntervals.Month;
            return new PriceRecurring
            {
                Interval = interval
            };
        }
    }
}