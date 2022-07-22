using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Data.Entities.PricingStrategyTables;

namespace Palavyr.Core.Data
{
    public class AppDataContexts : DbContext
    {
        public AppDataContexts(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserSession> Sessions { get; set; }
        public DbSet<AccountEmailVerification> EmailVerifications { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<StripeWebhookReceivedRecord> StripeWebhookReceivedRecords { get; set; }
        public DbSet<Intent> Intents { get; set; }
        public DbSet<ConversationNode> ConversationNodes { get; set; }
        public DbSet<StaticFee> StaticFees { get; set; }
        public DbSet<StaticTablesMeta> StaticTablesMetas { get; set; }
        public DbSet<StaticTableRow> StaticTablesRows { get; set; }
        public DbSet<PricingStrategyTableMeta> PricingStrategyTableMetas { get; set; }

        public DbSet<FileAsset> FileAssets { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<AttachmentLinkRecord> AttachmentRecords { get; set; }
        public DbSet<WidgetPreference> WidgetPreferences { get; set; }

        public DbSet<CategorySelectTableRow> SimpleSelectTableRows { get; set; }
        public DbSet<PercentOfThresholdTableRow> PercentOfThresholds { get; set; }
        public DbSet<SimpleThresholdTableRow> SimpleThresholdTableRows { get; set; }
        public DbSet<SelectWithNestedSelectTableRow> TwoNestedSelectTableRows { get; set; }
        public DbSet<CategoryNestedThresholdTableRow> CategoryNestedThresholdTableRows { get; set; }

        public DbSet<ConversationHistoryRow> ConversationHistoryRows { get; set; }
        public DbSet<ConversationHistoryMeta> ConversationRecords { get; set; }
    }
}