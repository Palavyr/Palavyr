using System.Threading.Tasks;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.CoreServiceWrappers
{
    public interface IStripeProductService
    {
        Task<Product> GetProduct(string productId);
    }
}