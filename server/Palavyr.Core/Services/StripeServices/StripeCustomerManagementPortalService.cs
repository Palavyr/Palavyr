using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stripe.BillingPortal;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeCustomerManagementPortalService
    {
        Task<string> FormCustomerSubscriptionManagementPortalUrl(string customerId, string returnUrl);
    }

    public class StripeCustomerManagementPortalService : IStripeCustomerManagementPortalService
    {
        private readonly ILogger<StripeCustomerService> logger;
        private readonly IStripeServiceLocatorProvider stripeServiceLocatorProvider;

        public StripeCustomerManagementPortalService(ILogger<StripeCustomerService> logger, IStripeServiceLocatorProvider stripeServiceLocatorProvider)
        {
            this.logger = logger;
            this.stripeServiceLocatorProvider = stripeServiceLocatorProvider;
        }

        public async Task<string> FormCustomerSubscriptionManagementPortalUrl(string customerId, string returnUrl)
        {
            logger.LogDebug("Creating management session");
            logger.LogDebug($"CustomerId: {customerId}");
            logger.LogDebug($"Return Url: {returnUrl}");
            var options = new SessionCreateOptions()
            {
                Customer = customerId,
                ReturnUrl = returnUrl,
            };
            var session = await stripeServiceLocatorProvider.BillingSessionService.CreateAsync(options);

            logger.LogDebug($"Session Url: {session.Url}");
            return session.Url;
        }
    }
}