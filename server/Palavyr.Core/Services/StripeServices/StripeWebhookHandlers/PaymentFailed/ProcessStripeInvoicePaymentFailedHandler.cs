using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.EmailService;
using Palavyr.Core.Services.EmailService.ResponseEmailTools;
using Palavyr.Core.Stores;
using Stripe;
using Account = Palavyr.Core.Models.Accounts.Schemas.Account;


namespace Palavyr.Core.Services.StripeServices.StripeWebhookHandlers.PaymentFailed
{
    public interface IProcessStripeInvoicePaymentFailedHandler
    {
        Task Handle(Invoice invoice);
    }

    public class ProcessStripeInvoicePaymentFailedHandler : INotificationHandler<InvoicePaymentFailedEvent>
    {
        private readonly ILogger<ProcessStripeInvoicePaymentFailedHandler> logger;
        private readonly IEntityStore<Account> accountStore;
        private readonly ISesEmail emailClient;

        public ProcessStripeInvoicePaymentFailedHandler(
            ILogger<ProcessStripeInvoicePaymentFailedHandler> logger,
            IEntityStore<Account> accountStore,
            ISesEmail emailClient
        )
        {
            this.logger = logger;
            this.accountStore = accountStore;
            this.emailClient = emailClient;
        }

        public async Task Handle(InvoicePaymentFailedEvent notification, CancellationToken cancellationToken)
        {
            var invoice = notification.invoice;
            var account = await accountStore.Get(invoice.CustomerId, s => s.StripeCustomerId);

            // if we don't get payment, we don't update the currentPeriodEnd. We check this at the beginning of each login, so
            // if we don't update, then time moves forward, and eventually the login will set IsActive to false. If isActive is false,
            // then we freeze their account because they owe us money. From there, they can pay their bill, and then cancel if they prefer
            // to not use the service.
            var endDate = account.CurrentPeriodEnd;


            var htmlBody = EmailPaymentFailed.GetPaymentFailedEmailHtml(endDate);
            var textBody = EmailPaymentFailed.GetPaymentFailedEmailText(endDate);
            await emailClient.SendEmail(
                EmailConstants.PalavyrMainEmailAddress,
                account.EmailAddress,
                EmailConstants.PalavyrPaymentFailedSubject,
                htmlBody,
                textBody);
            // }

            // var emailAddress = account.EmailAddress;
            // var amountDue = invoice.AmountDue;
            // var amountPaid = invoice.AmountPaid;
            // var chargeId = invoice.ChargeId;
            // var hasPaid = invoice.Paid; // should be false
            // var periodStart = invoice.PeriodStart; // DateTime - when the period began
            // var periodEnd = invoice.PeriodEnd; // DateTime - when the period ends

            // use this information to report to the customer using the Email Service (and a new email template for this problem) that
            // their latest invoice payment failed for some reason. Also tell them that they can resolve this by visiting their billing tab on the dashboard.
            // You can also tell them that this for given billing period, and that their subscription will be automatically cancelled a few days after the periodEnd (give
            // them a week or something?)

            // TODO Email the user and inform them that the payment failed. There will be a 2 day grace period before we cancel.
            // The cancel date will be the date provided by the stripe invoice paid webhook + 2 days worth (TODO: Add 2 days in the
            // invoice paid controller.
        }
    }

    public class InvoicePaymentFailedEvent : INotification
    {
        public readonly Invoice invoice;

        public InvoicePaymentFailedEvent(Invoice invoice)
        {
            this.invoice = invoice;
        }
    }
}