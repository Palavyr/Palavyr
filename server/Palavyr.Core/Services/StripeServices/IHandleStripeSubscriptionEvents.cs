using System.Threading.Tasks;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IHandleStripeSubscriptionEvents
    {
        Task Handle(Subscription  subscription);
    }
}