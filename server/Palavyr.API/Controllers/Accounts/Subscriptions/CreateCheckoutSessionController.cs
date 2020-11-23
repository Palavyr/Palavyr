using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.API.Controllers.Payments
{
    public class CreateCheckoutSessionRequest
    {
        [JsonProperty("priceId")] 
        public string PriceId { get; set; }
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

    [Route("api")]
    [ApiController]
    public class CreateCheckoutSessionController : ControllerBase
    {
        private ILogger<CreateCheckoutSessionController> logger;
        private readonly IStripeClient stripeClient;
        private AccountsContext accountsContext;

        public CreateCheckoutSessionController(
            ILogger<CreateCheckoutSessionController> logger,
            AccountsContext accountsContext
        )
        {
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.accountsContext = accountsContext;
            this.logger = logger;
        }

        [HttpPost("checkout/create-checkout-session")]
        public async Task<IActionResult> CreateSession(
            [FromHeader] string accountId,
            [FromBody] CreateCheckoutSessionRequest request)
        {
            
            
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            logger.LogDebug($"Account: {account}");
            if (account == null || account.StripeCustomerId == null)
            {
                throw new Exception("Account and Stripe customer Id must be set");
            }
            
            
            
            var options = new SessionCreateOptions
            {
                // See https://stripe.com/docs/api/checkout/sessions/create
                // for additional parameters to pass.
                // {CHECKOUT_SESSION_ID} is a string literal; do not change it!
                // the actual Session ID is returned in the query parameter when your customer
                // is redirected to the success page.
                SuccessUrl = request.SuccessUrl,
                CancelUrl = request.CancelUrl,
                
                // we use the customer ID here so they can provide any eail they like.
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

            var sessionService = new SessionService(stripeClient);
            Session session;
            try
            {
                session = await sessionService.CreateAsync(options);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Payment Error: {ex.StripeError.Message}");
                var response = new ErrorResponse();
                response.ErrorMessage = new ErrorMessage
                {
                    Message = ex.StripeError.Message
                };
                return BadRequest(response);
            }

            // Create a new session using the session.Id and save it to the session db with the account Details.
            // when the transaction is successful, then use the returned ID to 
            var newSession = Server.Domain.Accounts.Session.CreateNew(session.Id, accountId, account.ApiKey);
            await accountsContext.Sessions.AddAsync(newSession);
            await accountsContext.SaveChangesAsync();

            return Ok(session.Id);
        }
    }
}