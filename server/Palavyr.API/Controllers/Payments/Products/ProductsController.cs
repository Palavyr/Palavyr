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
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private static ILogger<ProductsController> _logger;
        private readonly IStripeClient _client = new StripeClient();
        private readonly ProductService _productService;
        public ProductsController(ILogger<ProductsController> logger)
        {
            
            StripeConfiguration.ApiKey = "sk_test_51HOtDQAnPqY603aZg1LhzHge6qQ7AEYcGPQhhCqMc5gXwfyr6XTEJJvJisBtzhFChIeOnytjCkhHK2ZmEgIuWyup00loOlq4W1";
            _logger = logger;
            _productService = new ProductService();
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {

            var options = new ProductListOptions { };
            var products = await _productService.ListAsync(options);
            return Ok(products);
        }

        [HttpGet("get-product/{productId}")]
        public async Task<IActionResult> GetProduct(string productId)
        {
            var options = new ProductListOptions();
            var product = await _productService.GetAsync(productId);
            return Ok(product);
        }
    }
}