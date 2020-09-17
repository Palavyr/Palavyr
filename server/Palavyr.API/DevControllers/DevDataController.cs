using Microsoft.AspNetCore.Mvc;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Palavyr.Common.Constants;
using Server.Domain.AccountDB;

namespace Palavyr.API.Controllers
{
    [Route("api/setup")]
    [ApiController]
    public class DefaultDataController : BaseController
    {
        public DefaultDataController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env) { }

        public void CreateFakeDemoData()
        {
            // demo account
            var rawPassword = "abc123";
            var hashedPassword = PasswordHashing.CreateHashedPassword(rawPassword);
            var accountId = "zsd2342";
            var apiKey = "abc456";
            
            var devAccount = UserAccount.CreateAccount("Cool User", "demo@gmail.com", hashedPassword, accountId, apiKey, "Demo Dev Company", "+61-01-2345-6789", true, "en-AU");
            var subscription = Subscription.CreateNew(accountId, apiKey, SubscriptionConstants.DefaultNumAreas);
            var data = new DevData("DemoData");

            AccountContext.Accounts.Add(devAccount);
            AccountContext.Subscriptions.Add(subscription);
            AccountContext.SaveChanges();

            // dev session
            var devSession = Session.CreateNew(accountId, "abc123");
            AccountContext.Sessions.Add(devSession);
            AccountContext.SaveChanges();


            DashContext.Areas.AddRange(data.Areas);
            DashContext.Groups.AddRange(data.Groups);
            DashContext.WidgetPreferences.Add(data.WidgetPreference);
            DashContext.SelectOneFlats.AddRange(data.SelectOneFlatsDefaultData);
            DashContext.DynamicTableMetas.AddRange(data.DynamicTableMetas);
            DashContext.SaveChanges();
            
            ConvoContext.CompletedConversations.AddRange(data.CompleteConversations);
            ConvoContext.SaveChanges();
        }

        public void CreateFakeData()
        {
            // dev account
            var rawPassword = "qwerty";
            var hashedPassword = PasswordHashing.CreateHashedPassword(rawPassword);
            
            var devAccount = UserAccount.CreateAccount("Admin", "paul.e.gradie@gmail.com", hashedPassword, "dashboardDev", "abc123", "Pauls Dev Company", "+61-04-4970-2364", true, "en-AU");
            AccountContext.Accounts.Add(devAccount);
            AccountContext.SaveChanges();
            
            // dev session
            var devSession = Session.CreateNew("dashboardDev", "abc123");
            AccountContext.Sessions.Add(devSession);
            AccountContext.SaveChanges();
            
            
            var data = new DevData();
            DashContext.Areas.AddRange(data.Areas);
            DashContext.Groups.AddRange(data.Groups);
            DashContext.WidgetPreferences.Add(data.WidgetPreference);
            DashContext.SelectOneFlats.AddRange(data.SelectOneFlatsDefaultData);
            DashContext.DynamicTableMetas.AddRange(data.DynamicTableMetas);
            DashContext.SaveChanges();
            
            ConvoContext.CompletedConversations.AddRange(data.CompleteConversations);
            ConvoContext.SaveChanges();
        }

        public void DeleteAllData()
        {
            AccountContext.Accounts.RemoveRange(AccountContext.Accounts);
            AccountContext.Sessions.RemoveRange(AccountContext.Sessions);
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

        [HttpPut]
        public void RefreshData()
        {
            DeleteAllData();
            CreateFakeData();
            CreateFakeDemoData();
        }
    }
}