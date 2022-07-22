using System;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;
using Account = Palavyr.Core.Data.Entities.Account;

namespace Palavyr.Core.Services.StripeServices.CoreServiceWrappers
{
    public interface IStripeSubscriptionService
    {
        Task<DateTime> GetBufferedEndTime(Subscription subscription);
        Task<Account.PlanTypeEnum> GetPlanTypeEnum(Subscription subscription);


        // Task<Subscription> GetSubscription(Session session);
        Price GetPriceDetails(Subscription subscription);
        string GetProductId(Price priceDetails);
        string GetPaymentInterval(Price priceDetails);
        Task<Subscription> GetSubscription(Session session);
    }
}