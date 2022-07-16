using Stripe;
using Account = Palavyr.Core.Data.Entities.Account;

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
            this.interval = Account.PaymentIntervals.Month;
            return this;
        }

        public StripePriceRecurringBuilder WithYearInterval()
        {
            this.interval = Account.PaymentIntervals.Month;
            return this;
        }

        public PriceRecurring Build()
        {
            var interval = this.interval ?? Account.PaymentIntervals.Month;
            return new PriceRecurring
            {
                Interval = interval
            };
        }
    }
}