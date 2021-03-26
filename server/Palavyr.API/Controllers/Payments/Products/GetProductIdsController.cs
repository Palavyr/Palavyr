using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Services.StripeServices;

namespace Palavyr.API.Controllers.Payments.Products
{
    public class GetProductIdsController : PalavyrBaseController
    {
        private ILogger<GetProductIdsController> logger;
        public GetProductIdsController(ILogger<GetProductIdsController> logger)
        {
            this.logger = logger;
        }
        
        [HttpGet("products/all")]
        public ProductIds GetProducts()
        {
            var products = ProductRegistry.GetProductIds();
            return products;
        }
    }
}