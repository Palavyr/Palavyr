using System.Threading.Tasks;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeWebhookAccountGetter
    {
        Task<Account> GetAccount(string stripeCustomerId);
    }
}