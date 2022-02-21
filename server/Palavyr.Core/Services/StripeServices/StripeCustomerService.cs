using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.AccountServices;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public class StripeCustomerService
    {
        private readonly ILogger<StripeCustomerService> logger;
        private readonly IDetermineCurrentEnvironment determineCurrentEnvironment;

        private readonly IPalavyrAccessChecker accessChecker;
        private readonly IStripeServiceLocatorProvider stripeServiceLocatorProvider;

        private bool IsTest => StripeConfiguration.ApiKey.ToLowerInvariant().Contains("test");


        public StripeCustomerService(
            ILogger<StripeCustomerService> logger,
            IDetermineCurrentEnvironment determineCurrentEnvironment,
            IPalavyrAccessChecker accessChecker,
            IStripeServiceLocatorProvider stripeServiceLocatorProvider
        )
        {
            this.logger = logger;
            this.determineCurrentEnvironment = determineCurrentEnvironment;
            this.accessChecker = accessChecker;
            this.stripeServiceLocatorProvider = stripeServiceLocatorProvider;
        }

        public async Task<Customer> UpdateStripeCustomerEmail(string emailAddress, string customerId)
        {
            var options = new CustomerUpdateOptions
            {
                Email = emailAddress
            };
            var customer = await stripeServiceLocatorProvider.CustomerService.UpdateAsync(customerId, options);
            return customer;
        }

        public async Task DeleteSingleLiveStripeCustomer(string stripeCustomerId)
        {
            if (IsTest)
            {
                try
                {
                    await stripeServiceLocatorProvider.CustomerService.DeleteAsync(stripeCustomerId);
                }
                catch
                {
                    logger.LogError("This customer ID doesn't exist anymore. It was probably deleted already manually.");
                }
            }
            else
            {
                try
                {
                    await stripeServiceLocatorProvider.CustomerService.DeleteAsync(stripeCustomerId);
                }
                catch (StripeException stripeException)
                {
                    throw new DomainException($"The stripe customer ID was not found in Stripe: {stripeCustomerId}. {stripeException.Message}");
                }
            }
        }

        public async Task DeleteStripeTestCustomerByEmailAddress(string emailAddress)
        {
            var customers = (await stripeServiceLocatorProvider.CustomerService.ListAsync(new CustomerListOptions() { Email = emailAddress })).ToList();
            if (customers.Count != 1)
            {
                throw new StripeException("Multiple customers were found during testing with the same email address. Please generate random unique test emails in your tests");
            }

            var customer = customers.Single();
            await stripeServiceLocatorProvider.CustomerService.DeleteAsync(customer.Id);
        }

        public async Task DeleteStripeTestCustomers(List<string> customerIds)
        {
            if (!IsTest)
            {
                throw new Exception("You may only delete test customers!");
            }

            if (determineCurrentEnvironment.IsProduction())
            {
                throw new DomainException("DeleteStripeTestCustomers not allowed to be called in production");
            }

            foreach (var customerId in customerIds)
            {
                await DeleteSingleLiveStripeCustomer(customerId);
            }
        }

        public async Task DeleteSingleStripeTestCustomer(string customerId)
        {
            var customer = await stripeServiceLocatorProvider.CustomerService.GetAsync(customerId, new CustomerGetOptions());
            if (accessChecker.IsATestStripeEmail(customer.Email))
            {
                await stripeServiceLocatorProvider.CustomerService.DeleteAsync(customer.Id);
            }
        }

        public async Task<Customer> CreateNewStripeCustomer(string emailAddress, CancellationToken cancellationToken)
        {
            var existing = await GetCustomerByEmailAddress(emailAddress, cancellationToken);
            if (existing.Count() != 0)
            {
                throw new DomainException("Attempting to create a Stripe Customer using an email address that already exists in the Stripe Customer list.");
            }

            var createOptions = new CustomerCreateOptions
            {
                Description = $"Customer automatically added for {(IsTest ? "testing" : "production")}.",
                Email = emailAddress
            };
            var customer = await stripeServiceLocatorProvider.CustomerService.CreateAsync(createOptions);
            return customer;
        }

        public async Task<Customer> GetCustomerByCustomerId(string customerId, CancellationToken cancellationToken)
        {
            var customer = await stripeServiceLocatorProvider.CustomerService.GetAsync(customerId, cancellationToken: cancellationToken);
            return customer;
        }

        public async Task<StripeList<Customer>> GetCustomerByEmailAddress(string emailAddress, CancellationToken cancellationToken)
        {
            var customer = await stripeServiceLocatorProvider.CustomerService.ListAsync(new CustomerListOptions() { Email = emailAddress });
            if (customer.Count() > 1)
            {
                throw new DomainException("Multiple stripe customers with the same email address found. This is not currently allowed.");
            }

            return customer;
        }

        public async Task<List<Customer>> ListCustomers(CancellationToken cancellationToken)
        {
            return (await stripeServiceLocatorProvider.CustomerService.ListAsync(cancellationToken: cancellationToken)).ToList();
        }
    }
}