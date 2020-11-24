using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.StripeServices.StripeWebhookHandlers;
using Stripe;

namespace Palavyr.API.Services.StripeEventService
{
    public interface IStripeEventWebhookService
    {
        Task ProcessStripeEvent(Event stripeEvent);
    }

    public class StripeEventWebhookService : IStripeEventWebhookService
    {
        private AccountsContext accountsContext;
        private ILogger<StripeEventWebhookService> logger;
        private StripeClient stripeClient;
        private IProcessStripeCheckoutSessionCompletedHandler processCheckoutSessionCompletedHandler;

        public StripeEventWebhookService(
            ILogger<StripeEventWebhookService> logger,
            AccountsContext accountsContext,
            IProcessStripeCheckoutSessionCompletedHandler processCheckoutSessionCompletedHandler
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.processCheckoutSessionCompletedHandler = processCheckoutSessionCompletedHandler;
        }

        public async Task ProcessStripeEvent(Event stripeEvent)
        {
            var session = (Stripe.Checkout.Session) stripeEvent.Data.Object;

            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted: //"checkout.session.completed":
                    await processCheckoutSessionCompletedHandler.ProcessCheckoutSessionCompleted(session);
                    break;

                case Events.InvoicePaid: //"invoice.paid":
                    await ProcessInvoicePaid(stripeEvent);
                    // Continue to provision the subscription as payments continue to be made.
                    // Store the status in your database and check when a user accesses your service.
                    // This approach helps you avoid hitting rate limits.

                    // TODO: Update the subscription database perhaps to record the account payment to display later in the account settings.
                    // TODO: Send an email thanking the user a fulfilled invoice and thank them for their ongoing business.
                    
                    break;

                case Events.InvoicePaymentFailed: //"invoice.payment_failed":
                    // The payment failed or the customer does not have a valid payment method.
                    // The subscription becomes past_due. Notify your customer and send them to the
                    // customer portal to update their payment information.
                    // TODO: Request that the user update their payment information
                    // TODO: Let them know how long until service is shut off.
                    await InvoicePaymentFailed(stripeEvent);
                    break;

                default:
                    logger.LogDebug($"Event Type not recognized: {stripeEvent.Type}");
                    break;
            }
        }

        private async Task InvoicePaymentFailed(Event stripeEvent)
        {
            throw new NotImplementedException();
        }

        private async Task ProcessInvoicePaid(Event stripeEvent)
        {
            throw new NotImplementedException();
        }
    }
}