using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers;
using Palavyr.API.controllers.accounts.seedData;
using Palavyr.API.devControllers;
using Palavyr.Common.Constants;
using Server.Domain.Accounts;
using Stripe;
using AccountType = Palavyr.Common.uniqueIdentifiers.AccountType;
using Subscription = Server.Domain.Accounts.Subscription;

namespace Palavyr.API.controllers.accounts.devAccount
{
    [Route("api/setup")]
    [ApiController]
    public class DefaultDataController : BaseController
    {
        private static ILogger<DefaultDataController> _logger;
        private readonly IStripeClient _stripeClient = new StripeClient(StripeConfiguration.ApiKey);

        public DefaultDataController(
            ILogger<DefaultDataController> logger,
            AccountsContext accountContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IWebHostEnvironment env)
            : base(accountContext, convoContext, dashContext, env)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPut("{devKey}")]
        public async Task RefreshData(string devKey)
        {
            var options = new CustomerListOptions {};
            var service = new CustomerService(_stripeClient);
            var customers = await service.ListAsync(options);
            foreach (var customer in customers)
            {
                await service.DeleteAsync(customer.Id);
            }

            if (devKey != "secretTobyface")
                _logger.LogDebug("This is an attempt to Refresh database data.");
            var devData = new DevDataHolder(
                "qwerty",
                "devdashboard",
                "abc123",
                "paul.e.gradie@gmail.com",
                "Admin",
                "Pauls Dev Company",
                "+61-04-4970-2364",
                true,
                "en-AU"
            );

            var demoData = new DevDataHolder(
                "abc123",
                "zsd2342",
                "abc456",
                "palavyrDemo@gmail.com",
                "Cool User",
                "Demo Dev Company",
                "+61-01-2345-6789",
                true,
                "en-AU"
            );

            try
            {
                _logger.LogDebug("Trying to Deleting All Data currently in the database!");
                Console.WriteLine("Trying to Deleting All Data currently in the database!");

                await DeleteAllData();
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error deleting all data... {ex.Message}");
                Console.WriteLine($"Error deleting all data... {ex.Message}");
            }

            try
            {
                _logger.LogDebug("-----Attempting to populate with dev data...");
                await PopulateDBs(devData);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Error loading dev data: {ex.Message}");
            }

            try
            {
                _logger.LogDebug("-----Attempting to populate with demo data...");
                await PopulateDBs(demoData);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"----Error populating Demo Data: {ex.Message}");
            }
        }

        public async Task PopulateDBs(DevDataHolder dh)
        {
            var devAccount = UserAccount.CreateAccount(dh.UserName, dh.Email, dh.HashedPassword, dh.AccountId,
                dh.ApiKey, dh.CompanyName, dh.PhoneNumber, dh.Active, dh.Locale, AccountType.Default);
            var subscription = Subscription.CreateNew(dh.AccountId, dh.ApiKey, SubscriptionConstants.DefaultNumAreas);
            var data = new DevSeedData(dh.AccountId, dh.Email);
            
            var createOptions = new CustomerCreateOptions
            {
                Description = "Customer automatically added for testing.",
                Email = dh.Email,
            };
            
            var service = new CustomerService();
            var customer = await service.CreateAsync(createOptions);
            devAccount.StripeCustomerId = customer.Id;
            
            AccountContext.Subscriptions.Add(subscription);
            AccountContext.Accounts.Add(devAccount);
            AccountContext.SaveChangesAsync();

            DashContext.Areas.AddRange(data.Areas);
            DashContext.Groups.AddRange(data.Groups);
            DashContext.WidgetPreferences.Add(data.WidgetPreference);
            DashContext.SelectOneFlats.AddRange(data.DefaultDynamicTables);
            DashContext.DynamicTableMetas.AddRange(data.DefaultDynamicTableMetas);
            DashContext.SaveChangesAsync();

            ConvoContext.CompletedConversations.AddRange(data.CompleteConversations);
            ConvoContext.SaveChangesAsync();
        }

        public async Task DeleteAllData()
        {
            AccountContext.Accounts.RemoveRange(AccountContext.Accounts);
            AccountContext.Sessions.RemoveRange(AccountContext.Sessions);
            AccountContext.Subscriptions.RemoveRange(AccountContext.Subscriptions);
            AccountContext.EmailVerifications.RemoveRange(AccountContext.EmailVerifications);
            AccountContext.SaveChangesAsync();

            DashContext.Areas.RemoveRange(DashContext.Areas);
            DashContext.WidgetPreferences.RemoveRange(DashContext.WidgetPreferences);
            DashContext.ConversationNodes.RemoveRange(DashContext.ConversationNodes);
            DashContext.StaticTablesMetas.RemoveRange(DashContext.StaticTablesMetas);
            DashContext.StaticTablesRows.RemoveRange(DashContext.StaticTablesRows);
            DashContext.StaticFees.RemoveRange(DashContext.StaticFees);
            DashContext.FileNameMaps.RemoveRange(DashContext.FileNameMaps);
            DashContext.Groups.RemoveRange(DashContext.Groups);
            DashContext.SelectOneFlats.RemoveRange(DashContext.SelectOneFlats);
            DashContext.DynamicTableMetas.RemoveRange(DashContext.DynamicTableMetas);
            DashContext.SaveChangesAsync();

            ConvoContext.Conversations.RemoveRange(ConvoContext.Conversations);
            ConvoContext.CompletedConversations.RemoveRange(ConvoContext.CompletedConversations);
            ConvoContext.SaveChangesAsync();
        }
    }
}