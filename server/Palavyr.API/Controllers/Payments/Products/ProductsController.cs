// using System.Linq;
// using System.Threading.Tasks;
// using DashboardServer.Data;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Stripe;
//
// namespace Palavyr.API.Controllers.Payments.Products
// {
//     
//     
//     
//     
//     [AllowAnonymous]
//     [Route("api/")]
//     [ApiController]
//     public class ProductsController : ControllerBase
//     {
//         private static ILogger<ProductsController> logger;
//         private readonly IStripeClient _client = new StripeClient();
//         private readonly ProductService _productService;
//         public ProductsController(ILogger<ProductsController> logger)
//         {
//             this.logger = logger;
//             _productService = new ProductService();
//         }
//         
//         // [HttpGet("products/get-products")]
//         // public async Task<IActionResult> GetProducts()
//         // {
//         //
//         //     var options = new ProductListOptions { };
//         //     var products = await _productService.ListAsync(options);
//         //     return Ok(products);
//         // }
//
//         // [HttpGet("products/get-product/{productId}")]
//         // public async Task<IActionResult> GetProduct(string productId)
//         // {
//         //     var options = new ProductListOptions();
//         //     var product = await _productService.GetAsync(productId);
//         //     return Ok(product);
//         // }
//     }
// }