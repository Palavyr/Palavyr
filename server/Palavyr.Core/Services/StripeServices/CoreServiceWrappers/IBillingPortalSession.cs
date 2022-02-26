using System.Threading.Tasks;

namespace Palavyr.Core.Services.StripeServices.CoreServiceWrappers
{
    public interface IBillingPortalSession
    {
        Task<Stripe.BillingPortal.Session> CreateAsync(Stripe.BillingPortal.SessionCreateOptions options);
    }
}