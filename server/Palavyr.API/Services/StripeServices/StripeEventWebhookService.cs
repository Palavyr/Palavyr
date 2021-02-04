using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.API.Services.StripeServices.StripeWebhookHandlers;
using Stripe;

namespace Palavyr.API.Services.StripeServices
{

    public class StripeEventWebhookService
    {
        private ILogger<StripeEventWebhookService> logger;
        private readonly ProcessStripeSubscriptionUpdatedHandler processStripeSubscriptionUpdatedHandler;
        private IProcessStripeCheckoutSessionCompletedHandler processCheckoutSessionCompletedHandler;
        private readonly ProcessStripeInvoicePaidHandler processStripeInvoicePaidHandler;
        private readonly ProcessStripeInvoicePaymentFailedHandler processStripeInvoicePaymentFailedHandler;
        private readonly ProcessStripeSubscriptionDeletedHandler processStripeSubscriptionDeletedHandler;
        private readonly ProcessStripeSubscriptionCreatedHandler processStripeSubscriptionCreatedHandler;

        public StripeEventWebhookService(
            ILogger<StripeEventWebhookService> logger,
            ProcessStripeSubscriptionUpdatedHandler processStripeSubscriptionUpdatedHandler,
            ProcessStripeCheckoutSessionCompletedHandler processCheckoutSessionCompletedHandler,
            ProcessStripeInvoicePaidHandler processStripeInvoicePaidHandler,
            ProcessStripeInvoicePaymentFailedHandler processStripeInvoicePaymentFailedHandler,
            ProcessStripeSubscriptionDeletedHandler processStripeSubscriptionDeletedHandler,
            ProcessStripeSubscriptionCreatedHandler processStripeSubscriptionCreatedHandler
        )
        {
            this.logger = logger;
            this.processStripeSubscriptionUpdatedHandler = processStripeSubscriptionUpdatedHandler;
            this.processCheckoutSessionCompletedHandler = processCheckoutSessionCompletedHandler;
            this.processStripeInvoicePaidHandler = processStripeInvoicePaidHandler;
            this.processStripeInvoicePaymentFailedHandler = processStripeInvoicePaymentFailedHandler;
            this.processStripeSubscriptionDeletedHandler = processStripeSubscriptionDeletedHandler;
            this.processStripeSubscriptionCreatedHandler = processStripeSubscriptionCreatedHandler;
        }

        public async Task ProcessStripeEvent(Event stripeEvent)
        {
            switch (stripeEvent.Type)
            {
                case Events.CustomerSubscriptionUpdated:
                    var subscriptionUpdated = (Subscription) stripeEvent.Data.Object;
                    await processStripeSubscriptionUpdatedHandler.ProcessSubscriptionUpdated(subscriptionUpdated);
                    break;
                
                case Events.CustomerSubscriptionCreated:
                    var subscriptionCreated = (Subscription) stripeEvent.Data.Object;
                    await processStripeSubscriptionCreatedHandler.ProcessSubscriptionCreated(subscriptionCreated);
                    break; 
                
                case Events.CustomerSubscriptionDeleted:
                    var subscriptionDeleted = (Subscription) stripeEvent.Data.Object;
                    await processStripeSubscriptionDeletedHandler.ProcessSubscriptionDeleted(subscriptionDeleted);
                    break;

                case Events.CheckoutSessionCompleted: //"checkout.session.completed":
                    var session = (Stripe.Checkout.Session) stripeEvent.Data.Object;
                    await processCheckoutSessionCompletedHandler.ProcessCheckoutSessionCompleted(session);
                    break;

                case Events.InvoicePaid:
                    var invoicePaid = (Invoice) stripeEvent.Data.Object;
                    await processStripeInvoicePaidHandler.ProcessInvoicePaid(invoicePaid);

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

                    var failedInvoice = (Invoice) stripeEvent.Data.Object;
                    await processStripeInvoicePaymentFailedHandler.ProcessInvoicePaymentFailed(failedInvoice);
                    break;

                case Events.PaymentMethodUpdated:
                    // Simply send an email confirming they've updated their payment method.
                    break;

                case Events.PlanUpdated:
                    var plan = (Plan) stripeEvent.Data.Object;
                    // TODO: await processPlanUpdateHandler.ProcessPlanUpdate(plan);
                    // Modify the plan type used in the DB (plan type information is available is UserAccount
                    break;

                case Events.ChargeRefunded:
                    break;

                case Events.CustomerCreated: // Do we need this here? I don't think so. WE create the customer/ customer ID on registration.
                    break;

                case Events.InvoiceCreated:
                    var invoice = (Invoice) stripeEvent.Data.Object;
                    // Simply send an email to notify customer there is a new invoice for subscription.
                    // Can notify the customer that they have a new invoice.
                    break;

                case Events.PlanDeleted:
                    var deletePlan = (Plan) stripeEvent.Data.Object;
                    // TODO: await processPlanUpdateHandler.DeletePlan(plan); // revert to Free tier at end of subscription.
                    // occurs when a customer downgrades to Free again and cancels their plan
                    break;

                case Events.PriceUpdated:
                    // use this to update customers that the price for their plan has been updated (hopefully to a lower price? But if we must... to a higher)
                    // TODO: Send a simple email to communicate that I've changed the price of a current subscription plan. 
                    break;

                default:
                    logger.LogDebug($"Event Type not recognized: {stripeEvent.Type}");
                    break;
            }
        }
    }
}