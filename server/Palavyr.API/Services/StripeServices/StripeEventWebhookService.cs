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
        private readonly ProcessStripePaymentMethodUpdatedHandler processStripePaymentMethodUpdatedHandler;
        private readonly ProcessStripePlanUpdatedHandler processStripePlanUpdatedHandler;
        private readonly ProcessStripeInvoiceCreatedHandler processStripeInvoiceCreatedHandler;
        private readonly ProcessStripePriceUpdatedHandler processStripePriceUpdatedHandler;

        public StripeEventWebhookService(
            ILogger<StripeEventWebhookService> logger,
            ProcessStripeSubscriptionUpdatedHandler processStripeSubscriptionUpdatedHandler,
            ProcessStripeCheckoutSessionCompletedHandler processCheckoutSessionCompletedHandler,
            ProcessStripeInvoicePaidHandler processStripeInvoicePaidHandler,
            ProcessStripeInvoicePaymentFailedHandler processStripeInvoicePaymentFailedHandler,
            ProcessStripeSubscriptionDeletedHandler processStripeSubscriptionDeletedHandler,
            ProcessStripeSubscriptionCreatedHandler processStripeSubscriptionCreatedHandler,
            ProcessStripePaymentMethodUpdatedHandler processStripePaymentMethodUpdatedHandler,
            ProcessStripePlanUpdatedHandler processStripePlanUpdatedHandler,
            ProcessStripeInvoiceCreatedHandler processStripeInvoiceCreatedHandler,
            ProcessStripePriceUpdatedHandler processStripePriceUpdatedHandler
        )
        {
            this.logger = logger;
            this.processStripeSubscriptionUpdatedHandler = processStripeSubscriptionUpdatedHandler;
            this.processCheckoutSessionCompletedHandler = processCheckoutSessionCompletedHandler;
            this.processStripeInvoicePaidHandler = processStripeInvoicePaidHandler;
            this.processStripeInvoicePaymentFailedHandler = processStripeInvoicePaymentFailedHandler;
            this.processStripeSubscriptionDeletedHandler = processStripeSubscriptionDeletedHandler;
            this.processStripeSubscriptionCreatedHandler = processStripeSubscriptionCreatedHandler;
            this.processStripePaymentMethodUpdatedHandler = processStripePaymentMethodUpdatedHandler;
            this.processStripePlanUpdatedHandler = processStripePlanUpdatedHandler;
            this.processStripeInvoiceCreatedHandler = processStripeInvoiceCreatedHandler;
            this.processStripePriceUpdatedHandler = processStripePriceUpdatedHandler;
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

                case Events.CheckoutSessionCompleted:
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
                    var methodUpdated = (PaymentMethod) stripeEvent.Data.Object;
                    await processStripePaymentMethodUpdatedHandler.ProcessPaymentMethodUpdate(methodUpdated);
                    break;

                case Events.PlanUpdated:
                    var plan = (Plan) stripeEvent.Data.Object;
                    processStripePlanUpdatedHandler.ProcessStripePlanUpdate(plan);
                    // Modify the plan type used in the DB (plan type information is available is UserAccount
                    break;

                case Events.ChargeRefunded:
                    break;

                case Events.CustomerCreated: // Do we need this here? I don't think so. WE create the customer/ customer ID on registration.
                    break;

                case Events.InvoiceCreated:
                    // Send an email to the customer with information of their recent invoice
                    var invoiceCreated = (Invoice) stripeEvent.Data.Object;
                    await processStripeInvoiceCreatedHandler.ProcessInvoiceCreation(invoiceCreated);
                    break;

                case Events.PlanDeleted:
                    var deletePlan = (Plan) stripeEvent.Data.Object;
                    // NOT SURE IF THIS IS NEEDED.
                    // TODO: await processPlanUpdateHandler.DeletePlan(plan); // revert to Free tier at end of subscription.
                    // occurs when a customer downgrades to Free again and cancels their plan
                    break;

                case Events.PriceUpdated:
                    var priceUpdate = (Price) stripeEvent.Data.Object;
                    await processStripePriceUpdatedHandler.ProcessPriceUpdated(priceUpdate);
                    // use this to update customers that the price for their plan has been updated (hopefully to a lower price? But if we must... to a higher)
                    break;

                default:
                    logger.LogDebug($"Event Type not recognized: {stripeEvent.Type}");
                    break;
                
            }

            var stripeEventId = stripeEvent.Id;

        }
    }
}