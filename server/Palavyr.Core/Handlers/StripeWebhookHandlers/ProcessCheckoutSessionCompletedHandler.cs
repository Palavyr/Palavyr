using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.StripeServices;
using Session = Stripe.Checkout.Session;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeCheckoutSessionCompletedHandler : INotificationHandler<CheckoutSessionCompletedNotification>
    {
        private readonly IStripeSubscriptionSetter subscriptionSetter;

        public ProcessStripeCheckoutSessionCompletedHandler(IStripeSubscriptionSetter subscriptionSetter)
        {
            this.subscriptionSetter = subscriptionSetter;
        }

        /// <summary>
        /// Payment is successful and the subscription is created.
        /// You should provision the subscription.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Handle(CheckoutSessionCompletedNotification notification, CancellationToken cancellationToken)
        {
            await subscriptionSetter.SetSubscription(notification.session, cancellationToken);
        }
    }


    public class CheckoutSessionCompletedNotification : INotification
    {
        public readonly Session session;

        public CheckoutSessionCompletedNotification(Session session)
        {
            this.session = session;
        }
    }
}