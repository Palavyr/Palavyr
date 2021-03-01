using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.Common.UIDUtils;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class DbSetupAndTeardown
    {
        public static void SeedTestAccount(this AccountsContext accountContext)
        {
            var account = Account.CreateAccount(
                IntegrationConstants.EmailAddress,
                IntegrationConstants.Password,
                IntegrationConstants.AccountId,
                IntegrationConstants.ApiKey,
                AccountType.Default
            );

            accountContext.Accounts.Add(account);
            accountContext.SeedSession(IntegrationConstants.AccountId, IntegrationConstants.ApiKey);
            accountContext.SaveChanges();
        }

        public static void SeedSession(this AccountsContext accountsContext, string accountId, string apiKey)
        {
            var session = Session.CreateNew(IntegrationConstants.SessionId, accountId, apiKey);
            accountsContext.Sessions.Add(session);
        }

        public static void DisposeDbsByReset(this WebApplicationFactory<Startup> factory)
        {
            var services = factory.Services;
            var accountContext = services.GetRequiredService<AccountsContext>();
            var dashContext = services.GetRequiredService<DashContext>();
            var convoContext = services.GetRequiredService<ConvoContext>();
            ResetDbs(accountContext, dashContext, convoContext);
        }

        public static void DisposeByDelete(this WebApplicationFactory<Startup> factory)
        {
            var services = factory.Services;
            var accountContext = services.GetRequiredService<AccountsContext>();
            var dashContext = services.GetRequiredService<DashContext>();
            var convoContext = services.GetRequiredService<ConvoContext>();
            DeleteDbs(accountContext, dashContext, convoContext);
        }

        public static void ResetDbs(AccountsContext accountContext, DashContext dashContext, ConvoContext convoContext)
        {
            accountContext.ResetAccountDb();
            accountContext.SaveChanges();

            dashContext.ResetDashDb();
            dashContext.SaveChanges();

            convoContext.ResetConvoContext();
            convoContext.SaveChanges();
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