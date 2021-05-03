using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.BackupAndRestore
{
    public interface IUpdateDatabaseLatest
    {
        Task WriteAndSaveRecords(string latestDatabaseBackup);
    }

    public class UpdateDatabaseLatest : IUpdateDatabaseLatest
    {
        private readonly AccountsContext accountsContext;

        public UpdateDatabaseLatest(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }

        public async Task WriteAndSaveRecords(string latestDatabaseBackup)
        {
            var currentRecords = await accountsContext.Backups.FirstOrDefaultAsync();
            if (currentRecords == null)
            {
                await accountsContext.Backups.AddAsync(Backup.Create(latestDatabaseBackup));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(latestDatabaseBackup))
                {
                    currentRecords.LatestFullDbBackup = latestDatabaseBackup;
                }
            }

            await accountsContext.SaveChangesAsync();
        }
    }
}