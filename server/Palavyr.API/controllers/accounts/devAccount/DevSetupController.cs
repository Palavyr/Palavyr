using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.Controllers;
using Palavyr.API.controllers.accounts.seedData;
using Palavyr.API.devControllers;
using Palavyr.Common.Constants;
using Server.Domain.Accounts;

namespace Palavyr.API.controllers.accounts.devAccount
{
    [Route("api/setup")]
    [ApiController]
    public class DefaultDataController : BaseController
    {
        public DefaultDataController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        [HttpPut]
        public void RefreshData()
        {
            var devData = new DevDataHolder(
                "qwerty",
                "dashboard",
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

            DeleteAllData();
            PopulateDBs(devData);
            PopulateDBs(demoData);
        }

        public void PopulateDBs(DevDataHolder dh)
        {
            var devAccount = UserAccount.CreateAccount(dh.UserName, dh.Email, dh.HashedPassword, dh.AccountId, dh.ApiKey, dh.CompanyName, dh.PhoneNumber, dh.Active, dh.Locale);
            var subscription = Subscription.CreateNew(dh.AccountId, dh.ApiKey, SubscriptionConstants.DefaultNumAreas);
            var data = new DevSeedData(dh.AccountId);
            var devSession = Session.CreateNew(dh.AccountId, dh.ApiKey);

            AccountContext.Subscriptions.Add(subscription);
            AccountContext.Accounts.Add(devAccount);
            AccountContext.Sessions.Add(devSession);
            AccountContext.SaveChanges();

            DashContext.Areas.AddRange(data.Areas);
            DashContext.Groups.AddRange(data.Groups);
            DashContext.WidgetPreferences.Add(data.WidgetPreference);
            DashContext.SelectOneFlats.AddRange(data.DefaultDynamicTables);
            DashContext.DynamicTableMetas.AddRange(data.DefaultDynamicTableMetas);
            DashContext.SaveChanges();
            
            ConvoContext.CompletedConversations.AddRange(data.CompleteConversations);
            ConvoContext.SaveChanges();
        }

        public void DeleteAllData()
        {
            AccountContext.Accounts.RemoveRange(AccountContext.Accounts);
            AccountContext.Sessions.RemoveRange(AccountContext.Sessions);
            AccountContext.Subscriptions.RemoveRange(AccountContext.Subscriptions);
            AccountContext.EmailVerifications.RemoveRange(AccountContext.EmailVerifications);
            AccountContext.SaveChanges();
            
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
            DashContext.SaveChanges();

            ConvoContext.Conversations.RemoveRange(ConvoContext.Conversations);
            ConvoContext.CompletedConversations.RemoveRange(ConvoContext.CompletedConversations);
            ConvoContext.SaveChanges();

        }
    }
}