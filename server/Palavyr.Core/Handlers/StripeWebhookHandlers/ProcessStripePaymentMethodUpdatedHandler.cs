using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Stripe;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripePaymentMethodUpdatedHandler : INotificationHandler<PaymentMethodUpdatedEvent>
    {
        private readonly AccountsContext accountsContext;
        private readonly ISesEmail emailClient;

        public ProcessStripePaymentMethodUpdatedHandler(
            AccountsContext accountsContext,
            ISesEmail emailClient
        )
        {
            this.accountsContext = accountsContext;
            this.emailClient = emailClient;
        }

        public async Task Handle(PaymentMethodUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var paymentMethodUpdate = notification.paymentMethod;
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == paymentMethodUpdate.CustomerId);

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