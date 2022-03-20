#nullable enable
using System;
using System.Collections.Generic;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;
using Stripe;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public class StripeSubscriptionBuilder
    {
        private readonly BaseIntegrationFixture test;
        private Price? price;

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