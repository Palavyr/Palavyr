using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.Core.Data
{
    public class DashContext : DbContext
    {
        public DashContext(DbContextOptions<DashContext> options) : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<ConversationNode> ConversationNodes { get; set; }
        public DbSet<StaticFee> StaticFees { get; set; }
        public DbSet<StaticTablesMeta> StaticTablesMetas { get; set; }
        public DbSet<StaticTableRow> StaticTablesRows { get; set; }
        public DbSet<DynamicTableMeta> DynamicTableMetas { get; set; }

        public DbSet<FileAsset> FileAssets { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<AttachmentLinkRecord> AttachmentRecords { get; set; }

        public DbSet<WidgetPreference> WidgetPreferences { get; set; }
        public DbSet<SelectOneFlat> SelectOneFlats { get; set; }
        public DbSet<PercentOfThreshold> PercentOfThresholds { get; set; }
        public DbSet<BasicThreshold> BasicThresholds { get; set; }
        public DbSet<TwoNestedCategory> TwoNestedCategories { get; set; }
        public DbSet<CategoryNestedThreshold> CategoryNestedThresholds { get; set; }
    }
}