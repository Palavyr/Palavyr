using System.Linq;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;


namespace Palavyr.API.Controllers.Payments.Products
{

        [AllowAnonymous]
        [Route("api/products/prices")]
        [ApiController]
        public class PricesController : ControllerBase
        {
            private static ILogger<PricesController> _logger;
            private readonly IStripeClient _client = new StripeClient();
            private readonly ProductService _productService;

            public PricesController(ILogger<PricesController> logger)
            {
                StripeConfiguration.ApiKey = "sk_test_51HOtDQAnPqY603aZg1LhzHge6qQ7AEYcGPQhhCqMc5gXwfyr6XTEJJvJisBtzhFChIeOnytjCkhHK2ZmEgIuWyup00loOlq4W1";
                _logger = logger;
                _productService = new ProductService();
            }

            [HttpGet("get-prices/{productId}")]
            public async Task<IActionResult> GetPrices(string productId)
            {
                var options = new PriceListOptions
                {
                    Product = productId
                };
                var service = new PriceService();
                var prices = await service.ListAsync(options);
                return Ok(prices);
            }
        }
}