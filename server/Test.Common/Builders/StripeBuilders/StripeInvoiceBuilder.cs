using System;
using System.Threading.Tasks;
using Stripe;
using Test.Common.Random;

namespace Test.Common.Builders.StripeBuilders
{
    public interface IHaveStripeCustomerId
    {
        public string StripeCustomerId { get; set; }
    }
    
    public class StripeInvoiceBuilder
    {
        private readonly IHaveStripeCustomerId test;
        private string customerId;
        private Subscription? sub;
        private string? curr;
        private long? amountDue;
        private DateTime? dueDate;

        public StripeInvoiceBuilder(IHaveStripeCustomerId test)
        {
            this.test = test;
        }

        public StripeInvoiceBuilder WithCustomerId(string customerId)
        {
            this.customerId = customerId;
            return this;
        }

        public StripeInvoiceBuilder WithSubscription(Subscription subscription)
        {
            this.sub = subscription;
            return this;
        }

        public StripeInvoiceBuilder WithCurrency(string currency)
        {
            this.curr = currency;
            return this;
        }

        public StripeInvoiceBuilder WithDueDate(DateTime dueDate)
        {
            this.dueDate = dueDate;
            return this;
        }

        public StripeInvoiceBuilder WithAmountDue(long amountDue)
        {
            this.amountDue = amountDue;
            return this;
        }

        public Invoice Build()
        {
            var custId = this.customerId ?? test.StripeCustomerId;
            var subscription = this.sub ?? new StripeSubscriptionBuilder(test).Build();
            var currency = this.curr ?? "$";
            var amtDue = this.amountDue ?? A.RandomInt(1, 10);
            var dDate = this.dueDate ?? DateTime.Now.AddDays(3);

            return new Invoice
            {
                CustomerId = custId,
                Subscription = subscription,
                Currency = currency,
                AmountDue = amtDue,
                DueDate = dDate,
                
            };
        }
    }
}