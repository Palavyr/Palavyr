using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Stripe;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public class StripePriceRecurringBuilder
    {
        private readonly IntegrationTest test;
        private string? interval;

        public StripePriceRecurringBuilder(IntegrationTest test)
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