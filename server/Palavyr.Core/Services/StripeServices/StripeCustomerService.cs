using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Services.AccountServices;
using Palavyr.Core.Services.StripeServices.CoreServiceWrappers;
using Stripe;

namespace Palavyr.Core.Services.StripeServices
{
    public interface IStripeCustomerService
    {
        Task<Customer> UpdateStripeCustomerEmail(string emailAddress, string customerId);
        Task DeleteSingleLiveStripeCustomer(string stripeCustomerId);
        Task DeleteStripeTestCustomerByEmailAddress(string emailAddress);
        Task DeleteStripeTestCustomers(List<string> customerIds);
        Task DeleteSingleStripeTestCustomer(string customerId);
        Task<Customer> CreateNewStripeCustomer(string emailAddress, CancellationToken cancellationToken);
        Task<Customer> GetCustomerByCustomerId(string customerId, CancellationToken cancellationToken);
        Task<StripeList<Customer>> GetCustomerByEmailAddress(string emailAddress, CancellationToken cancellationToken);
        Task<List<Customer>> ListCustomers(CancellationToken cancellationToken);
    }

    public class StripeCustomerService : IStripeCustomerService
    {
        private readonly ILogger<StripeCustomerService> logger;
        private readonly IDetermineCurrentEnvironment determineCurrentEnvironment;

        private readonly IPalavyrAccessChecker accessChecker;
        private readonly ICustomerSessionService customerSessionService;

        private bool IsTest => StripeConfiguration.ApiKey.ToLowerInvariant().Contains("test");


        public StripeCustomerService(
            ILogger<StripeCustomerService> logger,
            IDetermineCurrentEnvironment determineCurrentEnvironment,
            IPalavyrAccessChecker accessChecker,
            ICustomerSessionService customerSessionService
        )
        {
            this.logger = logger;
            this.determineCurrentEnvironment = determineCurrentEnvironment;
            this.accessChecker = accessChecker;
            this.customerSessionService = customerSessionService;
        }

        public async Task<Customer> UpdateStripeCustomerEmail(string emailAddress, string customerId)
        {
            var options = new CustomerUpdateOptions
            {
                Email = emailAddress
            };
            var customer = await customerSessionService.UpdateAsync(customerId, options);
            return customer;
        }

        public async Task DeleteSingleLiveStripeCustomer(string stripeCustomerId)
        {
            if (IsTest)
            {
                try
                {
                    var opts = new CustomerDeleteOptions() { };
                    await customerSessionService.DeleteAsync(stripeCustomerId, opts);
                }
                catch
                {
                    logger.LogError("This customer ID doesn't exist anymore. It was probably deleted already manually");
                }
            }
            else
            {
                try
                {
                    var opts = new CustomerDeleteOptions() { };
                    await customerSessionService.DeleteAsync(stripeCustomerId, opts);
                }
                catch (StripeException stripeException)
                {
                    throw new DomainException($"The stripe customer ID was not found in Stripe: {stripeCustomerId}. {stripeException.Message}");
                }
            }
        }

        public async Task DeleteStripeTestCustomerByEmailAddress(string emailAddress)
        {
            var customers = (await customerSessionService.ListAsync(new CustomerListOptions() { Email = emailAddress })).ToList();
            if (customers.Count != 1)
            {
                throw new StripeException("Multiple customers were found during testing with the same email address. Please generate random unique test emails in your tests");
            }

            var customer = customers.Single();
            var opts = new CustomerDeleteOptions() { };
            await customerSessionService.DeleteAsync(customer.Id, opts);
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
            var customer = await customerSessionService.GetAsync(customerId, new CustomerGetOptions());
            if (accessChecker.IsATestStripeEmail(customer.Email))
            {
                var opts = new CustomerDeleteOptions() { };
                await customerSessionService.DeleteAsync(customer.Id, opts);
            }
        }

        public async Task<Customer> CreateNewStripeCustomer(string emailAddress, CancellationToken cancellationToken)
        {
            var existing = await GetCustomerByEmailAddress(emailAddress, cancellationToken);
            if (existing.Any())
            {
                throw new DomainException("Attempting to create a Stripe Customer using an email address that already exists in Stripe.");
            }

            var createOptions = new CustomerCreateOptions
            {
                Description = $"Customer automatically added for {(IsTest ? determineCurrentEnvironment.IsStaging() ? "testing-staging" : "testing-dev" : "production")}.",
                Email = emailAddress
            };
            var customer = await customerSessionService.CreateAsync(createOptions, cancellationToken);
            return customer;
        }

        public async Task<Customer> GetCustomerByCustomerId(string customerId, CancellationToken cancellationToken)
        {
            var opts = new CustomerGetOptions() { };
            var customer = await customerSessionService.GetAsync(customerId, opts, cancellationToken: cancellationToken);
            return customer;
        }

        public async Task<StripeList<Customer>> GetCustomerByEmailAddress(string emailAddress, CancellationToken cancellationToken)
        {
            var customer = await customerSessionService.ListAsync(new CustomerListOptions() { Email = emailAddress }, cancellationToken: cancellationToken);
            if (customer.Count() > 1)
            {
                throw new DomainException("Multiple stripe customers with the same email address found. This is not currently allowed.");
            }

            return customer;
        }

        public async Task<List<Customer>> ListCustomers(CancellationToken cancellationToken)
        {
            var opts = new CustomerListOptions() { };
            return (await customerSessionService.ListAsync(opts, cancellationToken: cancellationToken)).ToList();
        }
    }
}