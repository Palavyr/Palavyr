using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Handlers.StripeWebhookHandlers
{
    public interface IStripeWebhookAccountGetter
    {
        Task<Account> GetAccount(string stripeCustomerId);
    }
}