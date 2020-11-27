using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.API.Controllers.Accounts.Setup.SeedData;
using Palavyr.API.Services.StripeServices;
using Palavyr.Common.Constants;
using Server.Domain.Accounts;
using AccountType = Palavyr.FileSystem.UniqueIdentifiers.AccountType;
using Subscription = Server.Domain.Accounts.Subscription;

namespace Palavyr.API.Controllers.Accounts.Develop
{
    [Route("api")]
    [ApiController]
    public class DefaultDataController : ControllerBase
    {
        private ILogger<DefaultDataController> logger;
        private AccountsContext accountsContext;
        private ConvoContext convoContext;
        private DashContext dashContext;
        private IStripeCustomerService stripeCustomerService;

        public DefaultDataController(
            ILogger<DefaultDataController> logger,
            AccountsContext accountsContext,
            ConvoContext convoContext,
            DashContext dashContext,
            IStripeCustomerService stripeCustomerService
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.convoContext = convoContext;
            this.dashContext = dashContext;
            this.stripeCustomerService = stripeCustomerService;
        }

        [AllowAnonymous]
        [HttpPut("setup/{devKey}")]
        public async Task RefreshData(string devKey)
        {
            await stripeCustomerService.DeleteStripeTestCustomers();

            if (devKey != "secretTobyface")
                logger.LogDebug("This is an attempt to Refresh database data.");
            var devData = new DevDataHolder(
                "qwerty",
                "devdashboard",
                "abc123",
                "paul.e.gradie@gmail.com",
                "Admin",
                "Pauls Dev Company",
                "+61-04-4970-2364",
                true,
                "en-AU",
                AccountType.Google
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
                "en-AU",
                AccountType.Default
            );

            try
            {
                logger.LogDebug("Trying to Deleting All Data currently in the database!");
                Console.WriteLine("Trying to Deleting All Data currently in the database!");

                await DeleteAllData();
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Error deleting all data... {ex.Message}");
                Console.WriteLine($"Error deleting all data... {ex.Message}");
            }

            try
            {
                logger.LogDebug("-----Attempting to populate with dev data...");
                await PopulateDBs(devData);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Error loading dev data: {ex.Message}");
            }

            try
            {
                logger.LogDebug("-----Attempting to populate with demo data...");
                await PopulateDBs(demoData);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"----Error populating Demo Data: {ex.Message}");
            }
        }

        public async Task PopulateDBs(DevDataHolder dh)
        {
            var devAccount = UserAccount.CreateAccount(dh.UserName, dh.Email, dh.HashedPassword, dh.AccountId,
                dh.ApiKey, dh.CompanyName, dh.PhoneNumber, dh.Active, dh.Locale, dh.AccountType);
            var subscription = Subscription.CreateNew(dh.AccountId, dh.ApiKey, SubscriptionConstants.DefaultNumAreas);
            var data = new DevSeedData(dh.AccountId, dh.Email);    

            var customer = await stripeCustomerService.CreateNewStripeCustomer(dh.Email);

            devAccount.StripeCustomerId = customer.Id;

            await accountsContext.Subscriptions.AddAsync(subscription);
            await accountsContext.Accounts.AddAsync(devAccount);
            await accountsContext.SaveChangesAsync();

            await dashContext.Areas.AddRangeAsync(data.Areas);
            await dashContext.Groups.AddRangeAsync(data.Groups);
            await dashContext.WidgetPreferences.AddAsync(data.WidgetPreference);
            await dashContext.SelectOneFlats.AddRangeAsync(data.DefaultDynamicTables);
            await dashContext.DynamicTableMetas.AddRangeAsync(data.DefaultDynamicTableMetas);
            await dashContext.SaveChangesAsync();

            await convoContext.CompletedConversations.AddRangeAsync(data.CompleteConversations);
            await convoContext.SaveChangesAsync();
        }

        public async Task DeleteAllData()
        {
            accountsContext.Accounts.RemoveRange(accountsContext.Accounts);
            accountsContext.Sessions.RemoveRange(accountsContext.Sessions);
            accountsContext.Subscriptions.RemoveRange(accountsContext.Subscriptions);
            accountsContext.EmailVerifications.RemoveRange(accountsContext.EmailVerifications);
            await accountsContext.SaveChangesAsync();

            dashContext.Areas.RemoveRange(dashContext.Areas);
            dashContext.WidgetPreferences.RemoveRange(dashContext.WidgetPreferences);
            dashContext.ConversationNodes.RemoveRange(dashContext.ConversationNodes);
            dashContext.StaticTablesMetas.RemoveRange(dashContext.StaticTablesMetas);
            dashContext.StaticTablesRows.RemoveRange(dashContext.StaticTablesRows);
            dashContext.StaticFees.RemoveRange(dashContext.StaticFees);
            dashContext.FileNameMaps.RemoveRange(dashContext.FileNameMaps);
            dashContext.Groups.RemoveRange(dashContext.Groups);
            dashContext.SelectOneFlats.RemoveRange(dashContext.SelectOneFlats);
            dashContext.DynamicTableMetas.RemoveRange(dashContext.DynamicTableMetas);
            await dashContext.SaveChangesAsync();

            convoContext.Conversations.RemoveRange(convoContext.Conversations);
            convoContext.CompletedConversations.RemoveRange(convoContext.CompletedConversations);
            await convoContext.SaveChangesAsync();
        }
    }
}