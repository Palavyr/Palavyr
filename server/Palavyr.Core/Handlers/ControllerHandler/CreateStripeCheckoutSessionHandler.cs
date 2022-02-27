using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateStripeCheckoutSessionHandler : IRequestHandler<CreateStripeCheckoutSessionRequest, CreateStripeCheckoutSessionResponse>
    {
        private readonly ILogger<CreateStripeCheckoutSessionHandler> logger;
        private readonly IAccountRepository accountRepository;

        private readonly IStripeCheckoutServiceSession stripeCheckoutServiceSession;

        public CreateStripeCheckoutSessionHandler(
            ILogger<CreateStripeCheckoutSessionHandler> logger,
            IAccountRepository accountRepository,
            IStripeCheckoutServiceSession stripeCheckoutServiceSession
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
            this.stripeCheckoutServiceSession = stripeCheckoutServiceSession;
        }

        public async Task<CreateStripeCheckoutSessionResponse> Handle(CreateStripeCheckoutSessionRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();
            if (account.StripeCustomerId == null)
            {
                throw new DomainException("Account and Stripe customer Id must be set");
            }

            var sessionId = await stripeCheckoutServiceSession.CreateCheckoutSessionId(account.StripeCustomerId, request.SuccessUrl, request.CancelUrl, request.PriceId);
            await accountRepository.CreateAndAddNewSession(sessionId, account.ApiKey);
            await accountRepository.CommitChangesAsync();
            return new CreateStripeCheckoutSessionResponse(sessionId);
        }
    }

    public class CreateStripeCheckoutSessionResponse
    {
        public CreateStripeCheckoutSessionResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class CreateStripeCheckoutSessionRequest : IRequest<CreateStripeCheckoutSessionResponse>
    {
        [JsonProperty("priceId")]
        public string PriceId { get; set; }

        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}

