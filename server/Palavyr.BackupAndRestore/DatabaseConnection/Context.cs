using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;

namespace Palavyr.BackupAndRestore.DatabaseConnection
{
    public class CustomAccountsContext : AccountsContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Setting.ConnectionString);
            }
        }
    }
}