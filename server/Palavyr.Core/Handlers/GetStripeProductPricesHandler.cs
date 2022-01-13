using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Stripe;

namespace Palavyr.Core.Handlers
{
    public class GetStripeProductPricesHandler : IRequestHandler<GetStripeProductPricesRequest, GetStripeProductPricesResponse>
    {
        private readonly ILogger<GetStripeProductPricesHandler> logger;

        public GetStripeProductPricesHandler(ILogger<GetStripeProductPricesHandler> logger)
        {
            this.logger = logger;
        }

        public async Task<GetStripeProductPricesResponse> Handle(GetStripeProductPricesRequest request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Getting product price by id");
            var options = new PriceListOptions
            {
                Product = request.ProductId
            };
            var service = new PriceService();
            var prices = await service.ListAsync(options);
            var usablePrices = prices.Where(row => row.Active).ToList();
            return new GetStripeProductPricesResponse(usablePrices);
        }
    }

    public class GetStripeProductPricesRequest : IRequest<GetStripeProductPricesResponse>
    {
        [Required]
        public string ProductId { get; set; }
    }

    public class GetStripeProductPricesResponse
    {
        public GetStripeProductPricesResponse(List<Price> response) => Response = response;
        public List<Price> Response { get; set; }
    }
}