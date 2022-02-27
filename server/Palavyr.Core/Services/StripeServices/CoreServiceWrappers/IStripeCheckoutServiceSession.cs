using System.Threading.Tasks;

namespace Palavyr.Core.Services.StripeServices.CoreServiceWrappers
{
    public interface IStripeCheckoutServiceSession
    {
        Task<Stripe.Checkout.Session> CreateAsync(Stripe.Checkout.SessionCreateOptions options);
        Task<string> CreateCheckoutSessionId(string stripeCustomerId, string successUrl, string cancelUrl, string priceId);
    }
}