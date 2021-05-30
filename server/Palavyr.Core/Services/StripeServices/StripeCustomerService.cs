using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Palavyr.Core.Exceptions;
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

        public async Task<Customer> CreateNewStripeCustomer(string emailAddress, CancellationToken cancellationToken)
        {
            var existing = await GetCustomerByEmailAddress(emailAddress, cancellationToken);
            if (existing.Count() != 0)
            {
                throw new DomainException("Attempting to create a Stripe Customer using an email address that already exists in the Stripe Customer List");
            }

            var createOptions = new CustomerCreateOptions
            {
                Description = $"Customer automatically added for {(IsTest ? "testing" : "production")}.",
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

        public async Task<StripeList<Customer>> GetCustomerByEmailAddress(string emailAddress, CancellationToken cancellationToken)
        {
            var customer = await customerService.ListAsync(new CustomerListOptions() {Email = emailAddress});
            if (customer.Count() > 1)
            {
                throw new DomainException("Multiple stripe customers with the same email address found. This is not currently allowed.");
            }

            return customer;
        }

        public async Task<List<Customer>> ListCustomers(CancellationToken cancellationToken)
        {
            return (await customerService.ListAsync(cancellationToken: cancellationToken)).ToList();
        }
    }
}