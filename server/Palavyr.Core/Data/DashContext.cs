using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Palavyr.Core.Models.Configuration.Schemas;
using Palavyr.Core.Models.Configuration.Schemas.DynamicTables;

namespace Palavyr.Core.Data
{
    public class DashContext : DbContext, IDataContext
    {
        public DashContext(DbContextOptions<DashContext> options) : base(options)
        {
        }

        private IDbContextTransaction transaction;
        

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

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            transaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public void BeginTransaction()
        {
            transaction = Database.BeginTransaction();
        }

        public async Task FinalizeAsync(CancellationToken cancellationToken)
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }
}