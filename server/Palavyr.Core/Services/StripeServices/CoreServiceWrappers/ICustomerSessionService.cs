using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Palavyr.Core.Services.StripeServices.CoreServiceWrappers
{
    public interface ICustomerSessionService
    {
        Task<Customer> UpdateAsync(string customerId, CustomerUpdateOptions options, CancellationToken cancellationToken = default);
        Task DeleteAsync(string stripeCustomerId, CustomerDeleteOptions options,  CancellationToken cancellationToken = default);
        Task<StripeList<Customer>> ListAsync(CustomerListOptions customerListOptions, CancellationToken cancellationToken = default);
        Task<Customer> GetAsync(string customerId, CustomerGetOptions customerGetOptions, CancellationToken cancellationToken = default);
        Task<Customer> CreateAsync(CustomerCreateOptions createOptions, CancellationToken cancellationToken = default);
    }
}