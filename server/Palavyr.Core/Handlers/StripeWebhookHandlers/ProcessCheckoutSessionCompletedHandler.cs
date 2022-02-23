using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Services.StripeServices;
using Palavyr.Core.Services.StripeServices.Products;
using Stripe.Checkout;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public class ProcessStripeCheckoutSessionCompletedHandler : INotificationHandler<CheckoutSessionCompletedEvent>
    {
        private AccountsContext accountsContext;
        private readonly IProductRegistry productRegistry;
        private readonly ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger;
        private IStripeSubscriptionService stripeSubscriptionService;

        public ProcessStripeCheckoutSessionCompletedHandler(
            AccountsContext accountsContext,
            IProductRegistry productRegistry,
            ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger,
            IStripeSubscriptionService stripeSubscriptionService
        )
        {
            this.accountsContext = accountsContext;
            this.productRegistry = productRegistry;
            this.logger = logger;
            this.stripeSubscriptionService = stripeSubscriptionService;
        }

        /// <summary>
        /// Payment is successful and the subscription is created.
        /// You should provision the subscription.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Handle(CheckoutSessionCompletedEvent notification, CancellationToken cancellationToken)
        {
            var session = notification.session;
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == session.CustomerId);
            if (account == null)
            {
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }

            var subscription = await stripeSubscriptionService.GetSubscription(session);

            // var planTypeEnum = await stripeSubscriptionService.GetPlanTypeEnum(subscription);
            // var bufferedPeriodEnd = await stripeSubscriptionService.GetBufferedEndTime(subscription);

            // var subscription = await stripeSubscriptionService.GetSubscription(session);
            var priceDetails = stripeSubscriptionService.GetPriceDetails(subscription);
            var productId = stripeSubscriptionService.GetProductId(priceDetails);
            var planTypeEnum = productRegistry.GetPlanTypeEnum(productId);

            var paymentInterval = stripeSubscriptionService.GetPaymentInterval(priceDetails);
            var paymentIntervalEnum = paymentInterval.GetPaymentIntervalEnum();
            var bufferedPeriodEnd = paymentIntervalEnum.AddEndTimeBuffer(subscription.CurrentPeriodEnd);

            account.PlanType = planTypeEnum;
            account.HasUpgraded = true;
            account.CurrentPeriodEnd = bufferedPeriodEnd;
            await accountsContext.SaveChangesAsync();
        }
    }

    public class CheckoutSessionCompletedEvent : INotification
    {
        public readonly Session session;

        public CheckoutSessionCompletedEvent(Session session)
        {
            this.session = session;
        }
    }
}