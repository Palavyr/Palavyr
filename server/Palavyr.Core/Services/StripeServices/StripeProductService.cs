using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public class StripeProductService
    {
        private ILogger<StripeProductService> logger;
        private readonly IStripeServiceLocatorProvider stripeServiceLocatorProvider;


        public StripeProductService(ILogger<StripeProductService> logger, IStripeServiceLocatorProvider stripeServiceLocatorProvider)
        {
            this.logger = logger;
            this.stripeServiceLocatorProvider = stripeServiceLocatorProvider;
        }

        public async Task<Product> GetProduct(string productId)
        {
            Product stripeProduct;
            try
            {
                stripeProduct = await stripeServiceLocatorProvider.ProductService.GetAsync(productId);
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