using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetStripeCheckoutSessionHandler : IRequestHandler<GetStripeCheckoutSessionRequest, GetStripeCheckoutSessionResponse>
    {
        private readonly IStripeClient stripeClient;

        public GetStripeCheckoutSessionHandler(IStripeClient stripeClient)
        {
            this.stripeClient = stripeClient;
        }

        public async Task<GetStripeCheckoutSessionResponse> Handle(GetStripeCheckoutSessionRequest request, CancellationToken cancellationToken)
        {
            var service = new SessionService(this.stripeClient);
            var session = await service.GetAsync(request.SessionId);
            return new GetStripeCheckoutSessionResponse(session);
        }
    }

    public class GetStripeCheckoutSessionResponse
    {
        public GetStripeCheckoutSessionResponse(Session response) => Response = response;
        public Session Response { get; set; }
    }

    public class GetStripeCheckoutSessionRequest : IRequest<GetStripeCheckoutSessionResponse>
    {
        public string SessionId { get; set; }
    }
}