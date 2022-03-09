using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Data;
using Palavyr.Core.Data.Setup.SeedData;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Services.AuthenticationServices;
using Palavyr.Core.Services.StripeServices;

namespace Palavyr.API.Controllers.Accounts.Develop
{
    public class DefaultDataController : PalavyrBaseController
    {
        private ILogger<DefaultDataController> logger;
        private AccountsContext accountsContext;
        private ConvoContext convoContext;
        private DashContext dashContext;
        private StripeCustomerService stripeCustomerService;
        private readonly IDetermineCurrentEnvironment determineCurrentEnvironment;

        public DefaultDataController(
            ILogger<DefaultDataController> logger,
            AccountsContext accountsContext,
            ConvoContext convoContext,
            DashContext dashContext,
            StripeCustomerService stripeCustomerService,
            IDetermineCurrentEnvironment determineCurrentEnvironment
        )
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
            this.convoContext = convoContext;
            this.dashContext = dashContext;
            this.stripeCustomerService = stripeCustomerService;
            this.determineCurrentEnvironment = determineCurrentEnvironment;
        }

        [AllowAnonymous]
        [HttpPut("setup/{devKey}")]
        public async Task RefreshData(string devKey, CancellationToken cancellationToken)
        {
            if (determineCurrentEnvironment.IsProduction())
            {
                throw new DomainException("Refreshing data is not allowed in Production");
            }

            var customerIds = await accountsContext.Accounts.Select(x => x.StripeCustomerId).ToListAsync(cancellationToken);
            await stripeCustomerService.DeleteStripeTestCustomers(customerIds);

            if (devKey != "secretTobyface")
            {
                logger.LogDebug("This is an attempt to Refresh database data.");
                return;
            }

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
                await PopulateDBs(devData, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"Error loading dev data: {ex.Message}");
            }

            try
            {
                logger.LogDebug("-----Attempting to populate with demo data...");
                await PopulateDBs(demoData, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogDebug($"----Error populating Demo Data: {ex.Message}");
            }
        }

        private async Task PopulateDBs(DevDataHolder dh, CancellationToken cancellationToken)
        {
            var freePlanType = new LytePlanTypeMeta();

            var devAccount = Account.CreateAccount(
                dh.UserName, dh.Email, dh.HashedPassword, dh.AccountId,
                dh.ApiKey, dh.CompanyName, dh.PhoneNumber, dh.Active, dh.Locale, dh.AccountType);
            var subscription = Subscription.CreateNew(dh.AccountId, dh.ApiKey, freePlanType.GetDefaultNumAreas());
            var data = new DevSeedData(dh.AccountId, dh.Email, devAccount.IntroductionId);

            var customer = await stripeCustomerService.CreateNewStripeCustomer(dh.Email, cancellationToken);

            devAccount.StripeCustomerId = customer.Id;

            await accountsContext.Subscriptions.AddAsync(subscription);
            await accountsContext.Accounts.AddAsync(devAccount);
            await accountsContext.SaveChangesAsync();

            await dashContext.Areas.AddRangeAsync(data.Areas);
            await dashContext.WidgetPreferences.AddAsync(data.WidgetPreference);
            await dashContext.SelectOneFlats.AddRangeAsync(data.DefaultDynamicTables);
            await dashContext.DynamicTableMetas.AddRangeAsync(data.DefaultDynamicTableMetas);
            await dashContext.SaveChangesAsync();

            await convoContext.ConversationRecords.AddRangeAsync(data.CompleteConversations);
            await convoContext.SaveChangesAsync();
        }

        private async Task DeleteAllData()
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
            dashContext.SelectOneFlats.RemoveRange(dashContext.SelectOneFlats);
            dashContext.DynamicTableMetas.RemoveRange(dashContext.DynamicTableMetas);
            await dashContext.SaveChangesAsync();

            convoContext.ConversationHistories.RemoveRange(convoContext.ConversationHistories);
            convoContext.ConversationRecords.RemoveRange(convoContext.ConversationRecords);
            await convoContext.SaveChangesAsync();
        }
    }
}