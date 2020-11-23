using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Accounts;
using Stripe;
using Subscription = Stripe.Subscription;

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
        private readonly string planType = "Plantype";

        public StripeEventWebhookService(
            ILogger<StripeEventWebhookService> logger,
            AccountsContext accountsContext
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.stripeClient = new StripeClient(StripeConfiguration.ApiKey);
        }

        public async Task ProcessStripeEvent(Event stripeEvent)
        {
            switch (stripeEvent.Type)
            {
                case Events.CheckoutSessionCompleted: //"checkout.session.completed":
                    await ProcessCheckoutSessionCompleted(stripeEvent);
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

        private async Task ProcessCheckoutSessionCompleted(Event stripeEvent)
        {
            // Payment is successful and the subscription is created.
            // You should provision the subscription.

            // get account via customer ID
            var session = (Stripe.Checkout.Session) stripeEvent.Data.Object;
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == session.CustomerId);
            if (account == null)
            {
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }

            // Get subscription details
            var subscriptionService = new SubscriptionService(stripeClient);
            Subscription subscription;
            try
            {
                subscription = await subscriptionService.GetAsync(session.SubscriptionId);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"EXCEPTION: {ex.Message}");
                throw new Exception();
            }

            var priceDetails = subscription.Items.Data[0].Price;
            var productId = priceDetails.ProductId;

            var produceService = new ProductService(stripeClient);
            Product product;
            try
            {
                product = await produceService.GetAsync(productId);
            }
            catch (StripeException ex)
            {
                logger.LogDebug($"Could not find product: {ex.Message}");
                throw new Exception("Exception on finding price details");
            }

            var planFound = product
                .Metadata
                .TryGetValue(this.planType, out var planType);
            if (!planFound)
            {
                throw new Exception("Plan Type not found in the subscription data!");
            }

            var paymentInterval = priceDetails.Recurring.Interval;

            UserAccount.PlanTypeEnum planEnum;
            switch (planType)
            {
                case (UserAccount.PlanTypes.Premium):
                    planEnum = UserAccount.PlanTypeEnum.Premium;
                    break;
                case (UserAccount.PlanTypes.Pro):
                    planEnum = UserAccount.PlanTypeEnum.Pro;
                    break;
                default:
                    planEnum = UserAccount.PlanTypeEnum.Free;
                    break;
            }

            UserAccount.PaymentIntervalEnum paymentIntervalEnum;
            switch (paymentInterval)
            {
                case (UserAccount.PaymentIntervals.Month):
                    paymentIntervalEnum = UserAccount.PaymentIntervalEnum.Month;
                    break;
                case (UserAccount.PaymentIntervals.Year):
                    paymentIntervalEnum = UserAccount.PaymentIntervalEnum.Year;
                    break;
                default:
                    throw new Exception("Payment interval could not be determined");
            }

            account.PlanType = planEnum;
            account.HasUpgraded = true;
            account.PaymentInterval = paymentIntervalEnum;
            await accountsContext.SaveChangesAsync();
        }
    }
}