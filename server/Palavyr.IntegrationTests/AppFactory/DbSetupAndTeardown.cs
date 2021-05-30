using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Data;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures.BaseFixture;

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class DbSetupAndTeardown
    {


        public static void DisposeDbsByReset(this RealDatabaseIntegrationFixture fixture)
        {
            var services = fixture.Factory.Services;
            var accountContext = services.GetService<AccountsContext>();
            var dashContext = services.GetService<DashContext>();
            var convoContext = services.GetService<ConvoContext>();
            ResetDbs(accountContext, dashContext, convoContext);
        }
        
        public static void DisposeByDelete(this RealDatabaseIntegrationFixture fixture)
        {
            var services = fixture.Factory.Services;
            var accountContext = services.GetService<AccountsContext>();
            var dashContext = services.GetService<DashContext>();
            var convoContext = services.GetService<ConvoContext>();
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
            var staticMetas = dashContext.StaticTablesMetas.ToArray();
            var staticRows = dashContext.StaticTablesRows.ToArray();
            var perc = dashContext.PercentOfThresholds.ToArray();
            var select = dashContext.SelectOneFlats.ToArray();
            var basic = dashContext.BasicThresholds.ToArray();
            var categoryNested = dashContext.CategoryNestedThresholds.ToArray();
            var twoNested = dashContext.TwoNestedCategories.ToArray();

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
            dashContext.BasicThresholds.RemoveRange(basic);
            dashContext.CategoryNestedThresholds.RemoveRange(categoryNested);
            dashContext.TwoNestedCategories.RemoveRange(twoNested);
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