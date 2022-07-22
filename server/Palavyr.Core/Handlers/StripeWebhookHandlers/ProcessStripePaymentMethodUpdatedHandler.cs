using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Services.StripeServices;
using Stripe;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripePaymentMethodUpdatedHandler : INotificationHandler<PaymentMethodUpdatedEvent>
    {
        private readonly ISesEmail emailClient;
        private readonly IStripeWebhookAccountGetter stripeWebhookAccountGetter;

        public ProcessStripePaymentMethodUpdatedHandler(
            ISesEmail emailClient,
            IStripeWebhookAccountGetter stripeWebhookAccountGetter
        )
        {
            this.emailClient = emailClient;
            this.stripeWebhookAccountGetter = stripeWebhookAccountGetter;
        }

        public async Task Handle(PaymentMethodUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var paymentMethodUpdate = notification.paymentMethod;
            var account = await stripeWebhookAccountGetter.GetAccount(paymentMethodUpdate.CustomerId);
            
            if (paymentMethodUpdate.Livemode)
            {
                if (account == null)
                {
                    throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status - ProcessStripePaymentMethodUpdate");
                }
            }

            var subject = "Your Palavyr Payment Method Was Updated On Stripe";
            var htmlBody = "<h4>You've successfully updated your Palavyr Payment Method</h4> ";
            var textBody = "You've successfully updated your Palavyr Payment Method.";
            await emailClient.SendEmail("palavyr@gmail.com", account.EmailAddress, subject, htmlBody, textBody);
        }
    }

    public class PaymentMethodUpdatedEvent : INotification
    {
        public readonly PaymentMethod paymentMethod;

        public PaymentMethodUpdatedEvent(PaymentMethod paymentMethod)
        {
            this.paymentMethod = paymentMethod;
        }
    }
}