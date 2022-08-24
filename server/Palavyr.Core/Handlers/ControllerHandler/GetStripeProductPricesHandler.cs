using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Mappers;
using Stripe;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStripeProductPricesHandler : IRequestHandler<GetStripeProductPricesRequest, GetStripeProductPricesResponse>
    {
        private readonly IMapToNew<Price, PriceResource> mapper;
        private readonly ILogger<GetStripeProductPricesHandler> logger;

        public GetStripeProductPricesHandler(
            IMapToNew<Price, PriceResource> mapper,
            ILogger<GetStripeProductPricesHandler> logger)
        {
            this.mapper = mapper;
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
            //TODO: Map usablePrices to a prices resource dammit

            var resources = await mapper.MapMany(usablePrices, cancellationToken);

            return new GetStripeProductPricesResponse(resources.ToList());
        }
    }

    public class PriceResource : Price
    {
    }

    public class PriceResourceMapper : IMapToNew<Price, PriceResource>
    {
        public async Task<PriceResource> Map(Price from, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            var cloned = from.DeepClone<Price, PriceResource>();
            return cloned;
        }
    }


    public class GetStripeProductPricesRequest : IRequest<GetStripeProductPricesResponse>
    {
        [Required]
        public string ProductId { get; set; }
    }

    public class GetStripeProductPricesResponse
    {
        public GetStripeProductPricesResponse(List<PriceResource> response) => Response = response;
        public List<PriceResource> Response { get; set; }
    }

    public static class Extensions
    {
        public static Tr DeepClone<T, Tr>(this T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (Tr)formatter.Deserialize(stream);
            }
        }
    }
}