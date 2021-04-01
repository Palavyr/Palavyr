using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    [AllowAnonymous]
    public class GetStripeProductPricesController : PalavyrBaseController
    {
        private ILogger<GetStripeProductPricesController> logger;

        public GetStripeProductPricesController(
            ILogger<GetStripeProductPricesController> logger
        )
        {
            this.logger = logger;
        }

        [HttpGet("products/prices/get-prices/{productId}")]
        public async Task<List<Price>> Get(string productId)
        {
            logger.LogDebug("Getting product price by id");
            var options = new PriceListOptions
            {
                Product = productId
            };
            var service = new PriceService();
            var prices = await service.ListAsync(options);
            var usablePrices = prices.Where(row => row.Active).ToList();
            return usablePrices;
        }
    }
}