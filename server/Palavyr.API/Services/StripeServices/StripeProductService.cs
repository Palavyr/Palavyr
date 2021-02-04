using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.Services.StripeServices
{
    public class StripeProductService
    {
        private StripeClient stripeClient;
        private ILogger<StripeProductService> logger;
        private ProductService produceService;
        private readonly string planType = "Plantype";

        public StripeProductService(ILogger<StripeProductService> logger)
        {
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.produceService = new ProductService(stripeClient);
            this.logger = logger;
        }

        public async Task<Product> GetProduct(string productId)
        {
            Product product;
            try
            {
                product = await produceService.GetAsync(productId);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Could not find product: {ex.Message}");
                throw new Exception("Exception on finding price details");
            }

            return product;
        }

        public string GetPlanType(Product product)
        {
            var planFound = product.Metadata.TryGetValue(this.planType, out var planIdentifier);
            if (!planFound)
            {
                throw new Exception("Plan Type not found in the subscription data!");
            }
            return planIdentifier;
        }
    }
}