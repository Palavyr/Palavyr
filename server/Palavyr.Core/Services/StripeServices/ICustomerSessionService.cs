using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public interface ICustomerSessionService
    {
        Task<Customer> UpdateAsync(string customerId, CustomerUpdateOptions options, CancellationToken cancellationToken = default);
        Task DeleteAsync(string stripeCustomerId, CustomerDeleteOptions options,  CancellationToken cancellationToken = default);
        Task<StripeList<Customer>> ListAsync(CustomerListOptions customerListOptions, CancellationToken cancellationToken = default);
        Task<Customer> GetAsync(string customerId, CustomerGetOptions customerGetOptions, CancellationToken cancellationToken = default);
        Task<Customer> CreateAsync(CustomerCreateOptions createOptions, CancellationToken cancellationToken = default);
    }

    public class CustomerSessionService : ICustomerSessionService
    {
        private readonly CustomerService customerService;

        public CustomerSessionService(IStripeClient stripeClient)
        {
            customerService = new CustomerService(stripeClient);
        }

        public async Task<Customer> UpdateAsync(string customerId, CustomerUpdateOptions options, CancellationToken cancellationToken = default)
        {
            return await customerService.UpdateAsync(customerId, options);
        }

        public async Task DeleteAsync(string stripeCustomerId, CustomerDeleteOptions options, CancellationToken cancellationToken = default)
        {
            await customerService.DeleteAsync(stripeCustomerId, options, null, cancellationToken);
        }

        public async Task<StripeList<Customer>> ListAsync(CustomerListOptions customerListOptions, CancellationToken cancellationToken = default)
        {
            return await customerService.ListAsync(customerListOptions, cancellationToken: cancellationToken);
        }

        public async Task<Customer> GetAsync(string customerId, CustomerGetOptions customerGetOptions, CancellationToken cancellationToken = default)
        {
            return await customerService.GetAsync(customerId, customerGetOptions, null, cancellationToken);
        }

        public async Task<Customer> CreateAsync(CustomerCreateOptions createOptions, CancellationToken cancellationToken = default)
        {
            return await customerService.CreateAsync(createOptions, null, cancellationToken);
        }
    }
}