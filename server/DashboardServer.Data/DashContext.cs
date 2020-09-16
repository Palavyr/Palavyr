 using Microsoft.EntityFrameworkCore;
using Server.Domain;
 using Server.Domain.App.schema;
 using Server.Domain.DynamicTables;

 namespace DashboardServer.Data
{
    public class DashContext : DbContext
    {
        public DashContext(DbContextOptions<DashContext> options) : base(options)
        {
        }
        public DbSet<Area> Areas { get; set; }
        public DbSet<ConversationNode> ConversationNodes { get; set; }
        public DbSet<FileNameMap> FileNameMaps { get; set; }
        public DbSet<StaticFee> StaticFees { get; set; }
        public DbSet<GroupMap> Groups { get; set; }
        public DbSet<StaticTablesMeta> StaticTablesMetas { get; set; }
        public DbSet<StaticTableRow> StaticTablesRows { get; set; }
        public DbSet<DynamicTableMeta> DynamicTableMetas { get; set; }
        
        public DbSet<WidgetPreference> WidgetPreferences { get; set; }
        public DbSet<SelectOneFlat> SelectOneFlats { get; set; }

    }
}