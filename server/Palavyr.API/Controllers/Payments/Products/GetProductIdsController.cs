using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Payments.Products
{
    public class GetProductIdsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "products/all";

        public GetProductIdsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<ProductIdResource> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetProductIdsRequest(), cancellationToken);
            return response.Response;
        }
    }
}