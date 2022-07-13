using Stripe;

namespace Test.Common.Builders.StripeBuilders
{
    public class StripePriceRecurringBuilder
    {
        private string? interval;

        public StripePriceRecurringBuilder()
        {
            
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