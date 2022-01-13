using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Stripe;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    [AllowAnonymous]
    public class GetStripeProductPricesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "products/prices/get-prices/{productId}";

        public GetStripeProductPricesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<List<Price>> Get([FromHeader] GetStripeProductPricesRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}