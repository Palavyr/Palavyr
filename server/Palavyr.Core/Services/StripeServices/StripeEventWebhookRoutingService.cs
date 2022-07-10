using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Handlers.StripeWebhookHandlers;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoiceCreated;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.InvoicePaid;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.PaymentFailed;
using Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.SubscriptionCreated;
using Stripe;
using Stripe.Checkout;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeEventWebhookRoutingService
    {
        Task ProcessStripeEvent(Event stripeEvent, string signature, CancellationToken cancellationToken);
    }

    public class StripeEventWebhookRoutingService : IStripeEventWebhookRoutingService
    {
        private readonly IMediator mediator;

        private ILogger<IStripeEventWebhookRoutingService> logger;

        public StripeEventWebhookRoutingService(IMediator mediator, ILogger<IStripeEventWebhookRoutingService> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        public async Task ProcessStripeEvent(Event stripeEvent, string signature, CancellationToken cancellationToken)
        {
            // TODO: write a cleanup task to remove old stripe events (older than say a week?)
            var response = await mediator.Send(new NewStripeEventReceivedEvent(signature), cancellationToken);
            if (response == null || (response != null && response.ShouldCancelProcessing))
            {
                // we've been hit with a replay -- we should not process this event
                return;
            }

            switch (stripeEvent.Type)
            {
                case Events.CustomerSubscriptionUpdated:
                    await mediator.Publish(new SubscriptionUpdatedEvent(stripeEvent.To<Subscription>()), cancellationToken);
                    break;

                case Events.CustomerSubscriptionCreated:
                    await mediator.Publish(new SubscriptionCreatedEvent(stripeEvent.To<Subscription>()), cancellationToken);
                    break;

                case Events.CustomerSubscriptionDeleted:
                    await mediator.Publish(new SubscriptionDeletedEvent(stripeEvent.To<Subscription>()), cancellationToken);
                    break;

                case Events.CheckoutSessionCompleted:
                    await mediator.Publish(new CheckoutSessionCompletedNotification(stripeEvent.To<Session>()), cancellationToken);
                    break;

                case Events.InvoicePaid:
                    await mediator.Publish(new InvoicePaymentSuccessfulEvent(stripeEvent.To<Invoice>()), cancellationToken);

                    // Continue to provision the subscription as payments continue to be made.
                    // Store the status in your database and check when a user accesses your service.
                    // This approach helps you avoid hitting rate limits.

                    // TODO: Update the subscription database perhaps to record the account payment to display later in the account settings.
                    // TODO: Send an email thanking the user a fulfilled invoice and thank them for their ongoing business.
                    break;

                case Events.InvoicePaymentFailed: //"invoice.payment_failed":
                    await mediator.Publish(new InvoicePaymentFailedEvent(stripeEvent.To<Invoice>()), cancellationToken);

                    // The payment failed or the customer does not have a valid payment method.
                    // The subscription becomes past_due. Notify your customer and send them to the
                    // customer portal to update their payment information.
                    // TODO: Request that the user update their payment information
                    // TODO: Let them know how long until service is shut off.

                    break;

                case Events.PaymentMethodUpdated:
                    await mediator.Publish(new PaymentMethodUpdatedEvent(stripeEvent.To<PaymentMethod>()), cancellationToken);
                    break;

                case Events.PlanUpdated:
                    await mediator.Publish(new PlanUpdatedEvent(stripeEvent.To<Plan>()), cancellationToken);
                    // Modify the plan type used in the DB (plan type information is available is UserAccount
                    break;

                case Events.ChargeRefunded:
                    break;

                case Events.CustomerCreated: // Do we need this here? I don't think so. WE create the customer/ customer ID on registration.
                    // this will get triggered when the customer finished registration with their auth token.
                    // I could take this and check it against the records to confirm we've got the customer Id correct.
                    break;

                case Events.InvoiceCreated:
                    await mediator.Publish(new StripeInvoiceCreatedEvent(stripeEvent.To<Invoice>()), cancellationToken);
                    // Send an email to the customer with information of their recent invoice
                    break;

                case Events.PlanDeleted:
                    // await mediator.Publish(new PlanDeletedEvent(stripeEvent.To<Plan>()), cancellationToken);
                    // var deletePlan = (Plan)stripeEvent.Data.Object;
                    // NOT SURE IF THIS IS NEEDED.
                    // TODO: await processPlanUpdateHandler.DeletePlan(plan); // revert to Free tier at end of subscription.
                    // occurs when a customer downgrades to Free again and cancels their plan
                    break;

                case Events.PriceUpdated:
                    await mediator.Publish(new PriceUpdatedEvent(stripeEvent.To<Price>()), cancellationToken);
                    // use this to update customers that the price for their plan has been updated (hopefully to a lower price? But if we must... to a higher)
                    break;

                default:
                    logger.LogDebug($"Event Type not recognized: {stripeEvent.Type}");
                    break;
            }

            await mediator.Publish(new StripeEventProcessedSuccessfullyEvent(stripeEvent.Id, signature));
        }
    }

    public static class StriveEventExtensionMethods
    {
        public static TEvent To<TEvent>(this Event stripeEvent) where TEvent : class
        {
            var x = stripeEvent.Data.Object as TEvent;
            return x;
        }
    }
}