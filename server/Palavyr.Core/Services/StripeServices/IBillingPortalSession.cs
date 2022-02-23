using System.Threading.Tasks;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IBillingPortalSession
    {
        Task<Stripe.BillingPortal.Session> CreateAsync(Stripe.BillingPortal.SessionCreateOptions options);
    }
}