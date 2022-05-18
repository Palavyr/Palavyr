using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Mappers;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.StripeServices.Products;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetProductIdsHandler : IRequestHandler<GetProductIdsRequest, GetProductIdsResponse>
    {
        private readonly IProductRegistry productRegistry;
        private readonly IMapToNew<ProductIds, ProductIdResource> mapper;

        public GetProductIdsHandler(IProductRegistry productRegistry, IMapToNew<ProductIds, ProductIdResource> mapper)
        {
            this.productRegistry = productRegistry;
            this.mapper = mapper;
        }

        public async Task<GetProductIdsResponse> Handle(GetProductIdsRequest request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var productsIds = productRegistry.GetProductIds();
            var resource = await mapper.Map(productsIds);
            return new GetProductIdsResponse(resource);
        }
    }

    public class GetProductIdsResponse
    {
        public GetProductIdsResponse(ProductIdResource response) => Response = response;
        public ProductIdResource Response { get; set; }
    }

    public class GetProductIdsRequest : IRequest<GetProductIdsResponse>
    {
    }
}