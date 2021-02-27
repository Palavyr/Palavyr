using System.Linq;
using Palavyr.Common.UIDUtils;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class DbSetupAndTeardown
    {
        public static void SeedTestAccount(this AccountsContext accountContext)
        {
            var apiKey = IntegrationConstants.ApiKey;
            var accountId = GuidUtils.CreateNewId();
            var account = Account.CreateAccount(
                "test@gmail.com",
                "123456",
                accountId,
                apiKey,
                AccountType.Default
            );

            accountContext.Accounts.Add(account);
            accountContext.SeedSession(accountId, apiKey);
            accountContext.SaveChanges();
        }

        public static void SeedSession(this AccountsContext accountsContext, string accountId, string apiKey)
        {
            var session = Session.CreateNew(IntegrationConstants.SessionId, accountId, apiKey);
            accountsContext.Sessions.Add(session);
        }

        public static void ResetDbs(AccountsContext accountContext, DashContext dashContext, ConvoContext convoContext)
        {
            accountContext.ResetAccountDb();
            dashContext.ResetDashDb();
            convoContext.ResetConvoContext();
        }

        public static void ResetConvoContext(this ConvoContext convoContext)
        {
            var convos = convoContext.Conversations.ToArray();
            var completed = convoContext.CompletedConversations.ToArray();
            
            convoContext.Conversations.RemoveRange(convos);
            convoContext.CompletedConversations.RemoveRange(completed);
        }
        
        public static void ResetDashDb(this DashContext dashContext)
        {
            var areas = dashContext.Areas.ToArray();
            var convoNodes = dashContext.ConversationNodes.ToArray();
            var fees = dashContext.StaticFees.ToArray();
            var prefs = dashContext.WidgetPreferences.ToArray();
            var dynTables = dashContext.DynamicTableMetas.ToArray();
            var nameMaps = dashContext.FileNameMaps.ToArray();
            var perc = dashContext.PercentOfThresholds.ToArray();
            var select = dashContext.SelectOneFlats.ToArray();
            var staticMetas = dashContext.StaticTablesMetas.ToArray();
            var staticRows = dashContext.StaticTablesRows.ToArray();

            dashContext.Areas.RemoveRange(areas);
            dashContext.ConversationNodes.RemoveRange(convoNodes);
            dashContext.StaticFees.RemoveRange(fees);
            dashContext.WidgetPreferences.RemoveRange(prefs);
            dashContext.DynamicTableMetas.RemoveRange(dynTables);
            dashContext.FileNameMaps.RemoveRange(nameMaps);
            dashContext.PercentOfThresholds.RemoveRange(perc);
            dashContext.SelectOneFlats.RemoveRange(select);
            dashContext.StaticTablesMetas.RemoveRange(staticMetas);
            dashContext.StaticTablesRows.RemoveRange(staticRows);
        }

        public static void ResetAccountDb(this AccountsContext accountsContext)
        {
            var accounts = accountsContext.Accounts.ToArray();
            var backups = accountsContext.Backups.ToArray();
            var sessions = accountsContext.Sessions.ToArray();
            var subs = accountsContext.Subscriptions.ToArray();
            var emailVerifications = accountsContext.EmailVerifications.ToArray();
            var stripeWebhooks = accountsContext.StripeWebHookRecords.ToArray();

            accountsContext.Accounts.RemoveRange(accounts);
            accountsContext.Backups.RemoveRange(backups);
            accountsContext.Sessions.RemoveRange(sessions);
            accountsContext.Subscriptions.RemoveRange(subs);
            accountsContext.EmailVerifications.RemoveRange(emailVerifications);
            accountsContext.StripeWebHookRecords.RemoveRange(stripeWebhooks);
        }

        public static void DeleteDbs(AccountsContext accountContext, DashContext dashContext, ConvoContext convoContext)
        {
            accountContext.Database.EnsureDeleted();
            dashContext.Database.EnsureDeleted();
            convoContext.Database.EnsureDeleted();
        }
    }
}