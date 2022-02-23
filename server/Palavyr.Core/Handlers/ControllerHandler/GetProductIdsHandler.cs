using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.StripeServices.Products;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetProductIdsHandler : IRequestHandler<GetProductIdsRequest, GetProductIdsResponse>
    {
        private readonly IProductRegistry productRegistry;

        public GetProductIdsHandler(IProductRegistry productRegistry)
        {
            this.productRegistry = productRegistry;
        }

        public async Task<GetProductIdsResponse> Handle(GetProductIdsRequest request, CancellationToken cancellationToken)
        {
            var productsIds = productRegistry.GetProductIds();
            return new GetProductIdsResponse(productsIds);
        }
    }

    public class GetProductIdsResponse
    {
        public GetProductIdsResponse(ProductIds response) => Response = response;
        public ProductIds Response { get; set; }
    }

    public class GetProductIdsRequest : IRequest<GetProductIdsResponse>
    {
    }
}