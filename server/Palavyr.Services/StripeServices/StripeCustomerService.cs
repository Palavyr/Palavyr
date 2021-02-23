using System;
using System.Threading.Tasks;
using Stripe;

namespace Palavyr.Services.StripeServices
{
    public class StripeCustomerService
    {
        private bool IsTest => StripeConfiguration.ApiKey.Contains("test");
        
        private CustomerService customerService;
        
        public StripeCustomerService()
        {
            var stripeClient = new StripeClient(StripeConfiguration.ApiKey);
            this.customerService = new CustomerService(stripeClient);
        }
        
        public async Task<Customer> UpdateStripeCustomerEmail(string emailAddress, string customerId)
        {
            var options = new CustomerUpdateOptions
            {
                Email = emailAddress
            };
            var customer = await customerService.UpdateAsync(customerId, options);
            return customer;
        }

        public async Task DeleteSingleLiveStripeCustomer(string stripeCustomerId)
        {
            await customerService.DeleteAsync(stripeCustomerId);
        }
        
        public async Task DeleteStripeTestCustomers()
        {
            if (!IsTest)
            {
                throw new Exception("You may only delete test customers!");
            }

            var options = new CustomerListOptions { };
            var customers = await customerService.ListAsync(options);
            foreach (var customer in customers)
            {
                await customerService.DeleteAsync(customer.Id);
            }
        }

        public async Task<Customer> CreateNewStripeCustomer(string emailAddress)
        {
            var description = IsTest ? "testing" : "production";
            var createOptions = new CustomerCreateOptions
            {
                Description = $"Customer automatically added for {description}.",
                Email = emailAddress
            };
            var customer = await customerService.CreateAsync(createOptions);
            return customer;
        }
    }
}