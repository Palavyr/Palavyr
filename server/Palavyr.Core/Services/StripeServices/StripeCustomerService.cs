using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public class StripeCustomerService
    {
        private bool IsTest => StripeConfiguration.ApiKey.ToLowerInvariant().Contains("test");

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

        public async Task DeleteStripeTestCustomerByEmailAddress(string emailAddress)
        {
            var customers = (await customerService.ListAsync(new CustomerListOptions() {Email = emailAddress})).ToList();
            if (customers.Count != 1)
            {
                throw new StripeException("Multiple customers were found during testing with the same email address. Please generate random unique test emails in your tests");
            }

            var customer = customers.Single();
            await customerService.DeleteAsync(customer.Id);
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
        
        public async Task<Customer> GetCustomerByCustomerId(string customerId, CancellationToken cancellationToken)
        {
            var customer = await customerService.GetAsync(customerId, cancellationToken: cancellationToken);
            return customer;
        }

        public async Task<List<Customer>> ListCustomers(CancellationToken cancellationToken)
        {
            return (await customerService.ListAsync(cancellationToken: cancellationToken)).ToList();
        }
    }
}