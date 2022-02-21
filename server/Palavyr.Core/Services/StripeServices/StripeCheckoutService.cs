﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.Core.Services.StripeServices
{
    public class StripeCheckoutService
    {
        private readonly ILogger<StripeCheckoutService> logger;
        private readonly IStripeServiceLocatorProvider stripeServiceLocatorProvider;

        public StripeCheckoutService(ILogger<StripeCheckoutService> logger, IStripeServiceLocatorProvider stripeServiceLocatorProvider)
        {
            this.logger = logger;
            this.stripeServiceLocatorProvider = stripeServiceLocatorProvider;
        }

        public async Task<string> CreateCheckoutSessionId(string stripeCustomerId, string successUrl, string cancelUrl, string priceId)
        {
            var options = new SessionCreateOptions
            {
                // See https://stripe.com/docs/api/checkout/sessions/create
                // for additional parameters to pass.
                // {CHECKOUT_SESSION_ID} is a string literal; do not change it!
                // the actual Session ID is returned in the query parameter when your customer
                // is redirected to the success page.
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,

                // we use the customer ID here so they can provide any email they like.
                // Any time we contact the customer about payments, we should use the stripe customer email. This is very important.
                // Otherwise We will be creating a new customer ID with the same email over an over again. 
                Customer = stripeCustomerId,
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Mode = "subscription",
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = priceId,
                        Quantity = 1,
                    },
                },
            };
            Session session;
            try
            {
                session = await stripeServiceLocatorProvider.CheckoutSessionService.CreateAsync(options);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Payment Error: {ex.StripeError.Message}");
                throw new Exception(ex.StripeError.Message);
            }

            return session.Id;
        }
    }
}