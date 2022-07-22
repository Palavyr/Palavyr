using System.Threading.Tasks;
using Palavyr.Core.Data.Entities;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeWebhookAccountGetter
    {
        Task<Account> GetAccount(string stripeCustomerId);
    }
}