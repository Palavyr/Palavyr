using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using DashboardServer.Data;
using EmailService.verification;
using Google.Apis.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.receiverTypes;
using Palavyr.API.response;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.API.Controllers.Payments
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

    [Route("api/checkout")]
    [ApiController]
    public class CreateCheckoutSessionController : BaseController
    {
        private static ILogger<CreateCheckoutSessionController> _logger;
        private readonly IStripeClient _stripeClient = new StripeClient(StripeConfiguration.ApiKey);

        public CreateCheckoutSessionController(
            ILogger<CreateCheckoutSessionController> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env
        ) : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateSession(
            [FromHeader] string accountId,
            [FromBody] CreateCheckoutSessionRequest request)
        {
            var account = AccountContext.Accounts.SingleOrDefault(row => row.AccountId == accountId);
            if (account == null) throw new Exception("Account Not FOUND!");

            var successUrl =
                request.SuccessUrl; // "http://localhost:8080/dashboard/subscribe/payment/success?session_id={CHECKOUT_SESSION_ID}";
            var cancelUrl = request.CancelUrl; // "http://localhost:8080/dashboard/subscribe/payment/canceled";

            var options = new SessionCreateOptions
            {
                // See https://stripe.com/docs/api/checkout/sessions/create
                // for additional parameters to pass.
                // {CHECKOUT_SESSION_ID} is a string literal; do not change it!
                // the actual Session ID is returned in the query parameter when your customer
                // is redirected to the success page.
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                
                // we use the custoemr ID here so they can provide any eail they like.
                // Any time we contact the customer about payments, we should use the stripe customer email. This is very important.
                // Otherwise We will be creating a new customer ID with the same email over an over again. 
                Customer = account.StripeCustomerId, 
                
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = request.PriceId,
                        Quantity = 1,
                    },
                },
            };

            var service = new SessionService(_stripeClient);
            Session session;
            try
            {
                session = await service.CreateAsync(options);
            }
            catch (StripeException ex)
            {
                _logger.LogDebug($"Payment Error: {ex.StripeError.Message}");
                var response = new ErrorResponse();
                response.ErrorMessage = new ErrorMessage
                {
                    Message = ex.StripeError.Message
                };
                return BadRequest(response);
            }

            // Create a new session using the session.Id and save it to the session db with the account Details.
            // when the transaction is successfull, then use the returned ID to 

            var newSession = Server.Domain.Accounts.Session.CreateNew(session.Id, accountId, account.ApiKey);
            await AccountContext.Sessions.AddAsync(newSession);
            await AccountContext.SaveChangesAsync();

            return Ok(session.Id);
        }
    }
}