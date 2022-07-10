using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Palavyr.Core.Stores;
using Session = Stripe.Checkout.Session;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    // public class ProcessStripeCheckoutSessionCompletedHandler : INotificationHandler<CheckoutSessionCompletedNotification>
    // {
    //     private readonly IEntityStore<Account> accountStore;
    //     private readonly ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger;
    //     private IStripeSubscriptionService stripeSubscriptionService;
    //
    //     public ProcessStripeCheckoutSessionCompletedHandler(
    //         IEntityStore<Account> accountStore, 
    //         ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger,
    //         IStripeSubscriptionService stripeSubscriptionService
    //     )
    //     {
    //         this.accountStore = accountStore;
    //         this.logger = logger;
    //         this.stripeSubscriptionService = stripeSubscriptionService;
    //     }
    //
    //     /// <summary>
    //     /// Payment is successful and the subscription is created.
    //     /// You should provision the subscription.
    //     /// </summary>
    //     /// <param name="session"></param>
    //     /// <returns></returns>
    //     /// <exception cref="Exception"></exception>
    //     public async Task Handle(CheckoutSessionCompletedNotification notification, CancellationToken cancellationToken)
    //     {
    //         var session = notification.session;
    //         var account = await accountStore.GetOrNull(session.CustomerId, s => s.StripeCustomerId);
    //         if (account == null)
    //         {
    //             throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
    //         }
    //
    //         var subscription = await stripeSubscriptionService.GetSubscription(session);
    //
    //         var planTypeEnum = await stripeSubscriptionService.GetPlanTypeEnum(subscription);
    //         var bufferedPeriodEnd = await stripeSubscriptionService.GetBufferedEndTime(subscription);
    //         
    //         account.PlanType = planTypeEnum;
    //         account.HasUpgraded = true;
    //         account.CurrentPeriodEnd = bufferedPeriodEnd;
    //     }
    // }

    public class CheckoutSessionCompletedNotification : INotification
    {
        public readonly Session session;

        public CheckoutSessionCompletedNotification(Session session)
        {
            this.session = session;
        }
    }
}