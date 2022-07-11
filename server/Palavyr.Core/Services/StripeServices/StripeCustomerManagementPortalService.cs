using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Stripe.BillingPortal;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public interface IStripeCustomerManagementPortalService
    {
        Task<string> FormCustomerSubscriptionManagementPortalUrl(string customerId, string returnUrl);
    }

    public class StripeCustomerManagementPortalService : IStripeCustomerManagementPortalService
    {
        private readonly ILogger<StripeCustomerService> logger;
        private readonly IBillingPortalSession billingPortalSession;

        public StripeCustomerManagementPortalService(ILogger<StripeCustomerService> logger, IBillingPortalSession billingPortalSession)
        {
            this.logger = logger;
            this.billingPortalSession = billingPortalSession;
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
            var session = await billingPortalSession.CreateAsync(options);

            logger.LogDebug($"Session Url: {session.Url}");
            return session.Url;
        }
    }
}