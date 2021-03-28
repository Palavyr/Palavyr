using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Palavyr.Services.Repositories;
using Palavyr.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    public class CreateCheckoutSessionRequest
    {
        [JsonProperty("priceId")] public string PriceId { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
    }

    public class CreateCheckoutSessionResponse
    {
        public string SessionId { get; set; }
    }

    public class ErrorResponse
    {
        public ErrorMessage ErrorMessage { get; set; }
    }

    public class ErrorMessage
    {
        public string Message { get; set; }
    }


    public class CreateCheckoutSessionController : PalavyrBaseController
    {
        private ILogger<CreateCheckoutSessionController> logger;
        private readonly IAccountRepository accountRepository;
        private readonly StripeCheckoutService stripeCheckoutService;

        public CreateCheckoutSessionController(
            ILogger<CreateCheckoutSessionController> logger,
            IAccountRepository accountRepository,
            StripeCheckoutService stripeCheckoutService
        )
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
            this.stripeCheckoutService = stripeCheckoutService;
        }

        [HttpPost("checkout/create-checkout-session")]
        public async Task<string> CreateSession(
            [FromHeader] string accountId,
            [FromBody] CreateCheckoutSessionRequest request
        )
        {
            var account = await accountRepository.GetAccount(accountId);
            if (account.StripeCustomerId == null)
            {
                throw new Exception("Account and Stripe customer Id must be set");
            }

            var sessionId = await stripeCheckoutService.CreateCheckoutSessionId(account.StripeCustomerId, request.SuccessUrl, request.CancelUrl, request.PriceId);
            await accountRepository.CreateAndAddNewSession(sessionId, accountId, account.ApiKey);
            await accountRepository.CommitChangesAsync();
            return sessionId;
        }
    }
}