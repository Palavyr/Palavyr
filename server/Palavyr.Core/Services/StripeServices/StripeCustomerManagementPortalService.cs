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

        public StripeCustomerManagementPortalService(ILogger<StripeCustomerService> logger)
        {
            this.logger = logger;
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
            var service = new SessionService();
            var session = await service.CreateAsync(options);
            
            logger.LogDebug($"Session Url: {session.Url}");
            return session.Url;
        }
    }
}