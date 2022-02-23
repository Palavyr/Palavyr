using System.Threading.Tasks;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public class BillingPortalSession : IBillingPortalSession
    {
        private readonly Stripe.BillingPortal.SessionService billingSessionService;

        public BillingPortalSession(IStripeClient client)
        {
            billingSessionService = new Stripe.BillingPortal.SessionService(client);
        }

        public async Task<Stripe.BillingPortal.Session> CreateAsync(Stripe.BillingPortal.SessionCreateOptions options)
        {
            return await billingSessionService.CreateAsync(options);
        }
    }
}