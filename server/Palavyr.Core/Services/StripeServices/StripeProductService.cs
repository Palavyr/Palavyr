using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeProductService
    {
        Task<Product> GetProduct(string productId);
    }

    public class StripeProductService : IStripeProductService
    {
        private ILogger<StripeProductService> logger;
        private readonly ProductService productService;


        public StripeProductService(ILogger<StripeProductService> logger, IStripeClient stripeClient)
        {
            this.logger = logger;
            productService = new  ProductService(stripeClient);
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