
using Palavyr.Core.Services.StripeServices.Products;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Stripe;
using Test.Common.Random;

namespace Palavyr.IntegrationTests.DataCreators.StripeBuilders
{
    public class StripePriceBuilder
    {
        private readonly IntegrationTest test;
        private bool? inactive;
        private string? id;
        private string? prodId;
        private int? amount;

        private TestProductRegistry productRegistry = new TestProductRegistry();
        private PriceRecurring? priceRecurring;

        public StripePriceBuilder(IntegrationTest test)
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

        public StripePriceBuilder WithPriceRecurring(PriceRecurring recurring)
        {
            this.priceRecurring = recurring;
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
}