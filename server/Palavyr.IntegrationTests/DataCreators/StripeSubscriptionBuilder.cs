#nullable enable
using System;
using System.Collections.Generic;
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Stripe;
using Test.Common.Random;

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

    public class StripeBillingSessionBuilder
    {
        private readonly BaseIntegrationFixture test;
        private string? custId;

        public StripeBillingSessionBuilder(BaseIntegrationFixture test)
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

    public class StripePriceBuilder
    {
        private readonly BaseIntegrationFixture test;
        private bool? inactive;
        private string? id;
        private string? prodId;
        private int? amount;

        private StagingProductRegistry productRegistry = new StagingProductRegistry();
        private PriceRecurring? priceRecurring;

        public StripePriceBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public StripePriceBuilder AsInActive()
        {
            this.inactive = false;
            return this;
        }

        public StripePriceBuilder WithId(string id)
        {
            this.id = id;
            return this;
        }

        public StripePriceBuilder WithPriceRecurring(PriceRecurring priceRecurring)
        {
            // priceDetails.Recurring.Interval;
            this.priceRecurring = priceRecurring;
            return this;
        }

        public StripePriceBuilder WithAmount(int amount)
        {
            this.amount = amount;
            return this;
        }

        public StripePriceBuilder WithFreeProductId()
        {
            this.prodId = productRegistry.GetProductIds().FreeProductId;
            return this;
        }

        public StripePriceBuilder WithProProductId()
        {
            this.prodId = productRegistry.GetProductIds().ProProductId;
            return this;
        }

        public Price Build()
        {

            var active = this.inactive ?? true;
            var id = this.id ?? A.RandomId();
            var prodId = this.prodId ?? productRegistry.GetProductIds().FreeProductId;
            var amount = this.amount ?? A.RandomInt(1, 10);
            var recurring = this.priceRecurring ?? test.CreateStripeRecurringBuilder().Build();
 
            var price = new Price
            {
                Active = active,
                Id = id,
                ProductId = prodId,
                UnitAmountDecimal = amount,
                Recurring = recurring
            };

            return price;
        }
    }


    public class StripeSubscriptionBuilder
    {
        private readonly BaseIntegrationFixture test;
        private Price? price;

        private StagingProductRegistry productRegistry = new StagingProductRegistry();
        private DateTime? periodEnd;
        private string customerId;
        private PriceRecurring? priceRecurring;

        public StripeSubscriptionBuilder(BaseIntegrationFixture test)
        {
            this.test = test;
        }

        public StripeSubscriptionBuilder WithPrice(Price price)
        {
            this.price = price;
            return this;
        }

        public StripeSubscriptionBuilder WithCustomerId(string customerId)
        {
            this.customerId = customerId;
            return this;
        }

        public StripeSubscriptionBuilder WithCurrentPeriodEnd(DateTime periodEnd)
        {
            this.periodEnd = periodEnd;
            return this;
        }

        public StripeSubscriptionBuilder WithPriceRecurring(PriceRecurring priceRecurring)
        {
            this.priceRecurring = priceRecurring;
            return this;
        }
        
        public Subscription Build()
        {
            var periodEnd = this.periodEnd ?? DateTime.Now;
            var custId = this.customerId ?? test.StripeCustomerId;
            var recurring = this.priceRecurring ?? test.CreateStripeRecurringBuilder().Build();

            var price = this.price ?? test.CreateStripePriceBuilder().Build();
              

            var items = new StripeList<SubscriptionItem>();
            items.Data = new List<SubscriptionItem>();
            items.Data.Add(
                new SubscriptionItem
                {
                    Id = "",

                    Created = DateTime.Now,
                    Plan = new Plan
                    {
                        Active = true,
                        Amount = 1,
                    },
                    Quantity = 1,
                    Price = price,
                    Subscription = "GreatSub",
                    
                });

            return new Subscription
            {
                Customer = new Customer
                {
                    Address = new Address
                    {
                        City = "",
                        Country = "",
                        Line1 = "",
                        PostalCode = ""
                    },
                    Balance = 0,
                    Created = DateTime.Now,
                    Currency = "$",
                    Email = "",
                    Delinquent = false,
                    Id = A.RandomId(),
                },
                Created = DateTime.Now,
                Id = A.RandomId(),
                CancelAtPeriodEnd = false,
                Items = items,
                CurrentPeriodEnd = periodEnd,
                CustomerId = custId,
                
            };
        }
    }
}