using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.BackupAndRestore
{
    public interface IUpdateDatabaseLatest
    {
        Task UpdateLatestUserDataRecord(string latestUserDataBackup);
        Task UpdateLatestDatabaseRecord(string latestDatabaseBackup);
        Task UpdateLatestBackupRecords(string latestDatabaseBackup, string latestUserDataBackup);
    }

    public class UpdateDatabaseLatest : IUpdateDatabaseLatest
    {
        private readonly AccountsContext accountsContext;

        public UpdateDatabaseLatest(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }

        public async Task UpdateLatestUserDataRecord(string latestUserDataBackup)
        {
            await WriteAndSaveRecords("", latestUserDataBackup);
        }

        public async Task UpdateLatestDatabaseRecord(string latestDatabaseBackup)
        {
            await WriteAndSaveRecords(latestDatabaseBackup, "");
        }

        public async Task UpdateLatestBackupRecords(string latestDatabaseBackup, string latestUserDataBackup)
        {
            await WriteAndSaveRecords(latestDatabaseBackup, latestUserDataBackup);
        }

        async Task WriteAndSaveRecords(string latestDatabaseBackup, string latestUserDataBackup)
        {
            var currentRecords = await accountsContext.Backups.FirstOrDefaultAsync();
            if (currentRecords == null)
            {
                await accountsContext.Backups.AddAsync(Backup.Create(latestDatabaseBackup, latestUserDataBackup));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(latestDatabaseBackup))
                {
                    currentRecords.LatestFullDbBackup = latestDatabaseBackup;
                }

                if (!string.IsNullOrWhiteSpace(latestUserDataBackup))
                {
                    currentRecords.LatestUserDataBackup = latestUserDataBackup;
                }
            }

            await accountsContext.SaveChangesAsync();
        }
    }
}