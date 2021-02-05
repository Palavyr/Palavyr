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
        private ProductService productService;

        public StripeProductService(ILogger<StripeProductService> logger)
        {
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.productService = new ProductService(stripeClient);
            this.logger = logger;
        }

        public async Task<Product> GetProduct(string productId)
        {
            Product stripeProduct;
            try
            {
                stripeProduct = await productService.GetAsync(productId);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Could not find product: {ex.Message}");
                throw new Exception("Exception on finding price details");
            }

            return stripeProduct;
        }

    }
}