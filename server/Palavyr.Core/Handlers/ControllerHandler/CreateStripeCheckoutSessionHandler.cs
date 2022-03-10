using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;
using Palavyr.Core.Repositories.StoreExtensionMethods;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class CreateStripeCheckoutSessionHandler : IRequestHandler<CreateStripeCheckoutSessionRequest, CreateStripeCheckoutSessionResponse>
    {
        private readonly ILogger<CreateStripeCheckoutSessionHandler> logger;
        private readonly IConfigurationEntityStore<Account> accountStore;

        private readonly IStripeCheckoutServiceSession stripeCheckoutServiceSession;

        public CreateStripeCheckoutSessionHandler(
            ILogger<CreateStripeCheckoutSessionHandler> logger,
            IConfigurationEntityStore<Account> accountStore,
            IStripeCheckoutServiceSession stripeCheckoutServiceSession
        )
        {
            this.logger = logger;
            this.accountStore = accountStore;
            this.stripeCheckoutServiceSession = stripeCheckoutServiceSession;
        }

        public async Task<CreateStripeCheckoutSessionResponse> Handle(CreateStripeCheckoutSessionRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();
            if (account.StripeCustomerId == null)
            {
                throw new DomainException("Account and Stripe customer Id must be set");
            }

            var sessionToken = await stripeCheckoutServiceSession.CreateCheckoutSessionId(account.StripeCustomerId, request.SuccessUrl, request.CancelUrl, request.PriceId);
            // await removeStaleSessions.CleanSessionDb();
            // var session = Session.CreateNew(sessionToken, accountStore.AccountId, account.ApiKey);
            // await sessionStore.Create(session);

            return new CreateStripeCheckoutSessionResponse(sessionToken);
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