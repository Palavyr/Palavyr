using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.API.Controllers.Payments.Products
{
    public class GetProductIdsController : PalavyrBaseController
    {
        private readonly IProductRegistry productRegistry;
        private ILogger<GetProductIdsController> logger;
        public GetProductIdsController(IProductRegistry productRegistry, ILogger<GetProductIdsController> logger)
        {
            this.productRegistry = productRegistry;
            this.logger = logger;
        }
        
        [HttpGet("products/all")]
        public ProductIds GetProducts()
        {
            var products = productRegistry.GetProductIds();
            return products;
        }
    }
}