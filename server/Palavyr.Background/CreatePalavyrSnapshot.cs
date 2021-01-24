using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.FileSystem.UIDUtils;
using Server.Domain.Accounts;

namespace Palavyr.Background
{
    public class CreatePalavyrSnapshot : ICreatePalavyrSnapshot
    {
        private readonly IPostgresBackup postgresBackup;
        private readonly IUserDataBackup userDataBackup;
        private readonly IConfiguration configuration;
        private readonly AccountsContext accountsContext;

        private const string PostgresHost = "Postgres:host";
        private const string PostgresPort = "Postgres:port";
        private const string PostgresPassword = "Postgres:password";
        private const string BackupBucket = "Backups";

        public CreatePalavyrSnapshot(IPostgresBackup postgresBackup, IUserDataBackup userDataBackup, IConfiguration configuration, AccountsContext accountsContext)
        {
            this.postgresBackup = postgresBackup;
            this.userDataBackup = userDataBackup;
            this.configuration = configuration;
            this.accountsContext = accountsContext;
        }

        public async Task CreateAndTransferCompleteBackup()
        {
            var snapshotTimeStamp = TimeUtils.CreateTimeStamp();
            var host = configuration.GetSection(PostgresHost).Value;
            var port = configuration.GetSection(PostgresPort).Value;
            var pass = configuration.GetSection(PostgresPassword).Value;
            var bucket = configuration.GetSection(BackupBucket).Value;

            var latestDatabaseBackup = await postgresBackup.CreateFullDatabaseBackup(host, port, pass, snapshotTimeStamp, bucket);
            var latestUserDataBackup = await userDataBackup.CreateFullUserDataBackup(snapshotTimeStamp, bucket);

            var currentRecords = await accountsContext.Backups.FirstOrDefaultAsync();
            if (currentRecords == null)
            {
                await accountsContext.Backups.AddAsync(Backup.Create(latestDatabaseBackup, latestUserDataBackup));
            }
            else
            {
                currentRecords.LatestFullDbBackup = latestDatabaseBackup;
                currentRecords.LatestUserDataBackup = latestUserDataBackup;
            }
            await accountsContext.SaveChangesAsync();
        }
    }
}