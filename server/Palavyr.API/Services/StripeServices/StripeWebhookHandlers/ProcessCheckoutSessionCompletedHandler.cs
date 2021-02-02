using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Accounts;
using Session = Stripe.Checkout.Session;

namespace Palavyr.API.Services.StripeServices.StripeWebhookHandlers
{
    public interface IProcessStripeCheckoutSessionCompletedHandler
    {
        Task ProcessCheckoutSessionCompleted(Session session);
    }

    public class ProcessStripeCheckoutSessionCompletedHandler : IProcessStripeCheckoutSessionCompletedHandler
    {
        private AccountsContext accountsContext;
        private ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger;
        private IStripeSubscriptionService stripeSubscriptionService;
        private IStripeProductService stripeProductService;

        public ProcessStripeCheckoutSessionCompletedHandler(
            AccountsContext accountsContext,
            ILogger<ProcessStripeCheckoutSessionCompletedHandler> logger,
            IStripeSubscriptionService stripeSubscriptionService,
            IStripeProductService stripeProductService
        )
        {
            this.accountsContext = accountsContext;
            this.logger = logger;
            this.stripeSubscriptionService = stripeSubscriptionService;
            this.stripeProductService = stripeProductService;
        }

        /// <summary>
        /// Payment is successful and the subscription is created.
        /// You should provision the subscription.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ProcessCheckoutSessionCompleted(Session session)
        {
            var account = await accountsContext
                .Accounts
                .SingleOrDefaultAsync(row => row.StripeCustomerId == session.CustomerId);
            if (account == null)
            {
                throw new Exception("ERROR TODO: EMAIL paul.e.gradie@gmail.com to manually set status");
            }

            var subscription = await stripeSubscriptionService.GetSubscription(session);
            var priceDetails = stripeSubscriptionService.GetPriceDetails(subscription);
            var productId = stripeSubscriptionService.GetProductId(priceDetails);
            var paymentInterval = stripeSubscriptionService.GetPaymentInterval(priceDetails);

            var product = await stripeProductService.GetProduct(productId);
            var planType = stripeProductService.GetPlanType(product);

            var planEnum = GetPlanEnum(planType);
            var paymentIntervalEnum = GetPaymentIntervalEnum(paymentInterval);

            var newPeriodEnd = GetNewPeriodEnd(paymentIntervalEnum);

            account.PlanType = planEnum;
            account.HasUpgraded = true;
            account.PaymentInterval = paymentIntervalEnum;
            account.CurrentPeriodEnd = newPeriodEnd;
            await accountsContext.SaveChangesAsync();
        }

        private DateTime GetNewPeriodEnd(UserAccount.PaymentIntervalEnum paymentIntervalEnum)
        {
            var currentDate = DateTime.Today;
            switch (paymentIntervalEnum)
            {
                case (UserAccount.PaymentIntervalEnum.Month):
                    return currentDate.AddMonths(1);
                case (UserAccount.PaymentIntervalEnum.Year):
                    return currentDate.AddYears(1);
                default:
                    throw new Exception("Payment interval could not be determined");
            }
        }

        private UserAccount.PaymentIntervalEnum GetPaymentIntervalEnum(string paymentInterval)
        {
            UserAccount.PaymentIntervalEnum paymentIntervalEnum;
            switch (paymentInterval)
            {
                case (UserAccount.PaymentIntervals.Month):
                    return UserAccount.PaymentIntervalEnum.Month;
                case (UserAccount.PaymentIntervals.Year):
                    return UserAccount.PaymentIntervalEnum.Year;
                default:
                    throw new Exception("Payment interval could not be determined");
            }
        }

        private UserAccount.PlanTypeEnum GetPlanEnum(string planType)
        {
            switch (planType)
            {
                case (UserAccount.PlanTypes.Premium):
                    return UserAccount.PlanTypeEnum.Premium;
                case (UserAccount.PlanTypes.Pro):
                    return UserAccount.PlanTypeEnum.Pro;
                default:
                    return UserAccount.PlanTypeEnum.Free;
            }
        }
    }
}