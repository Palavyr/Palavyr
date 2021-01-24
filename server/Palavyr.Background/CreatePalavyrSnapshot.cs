using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.Extensions.Configuration;
using Palavyr.BackupAndRestore;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.Background
{
    public class CreatePalavyrSnapshot : ICreatePalavyrSnapshot
    {
        private readonly IPostgresBackup postgresBackup;
        private readonly IUserDataBackup userDataBackup;
        private readonly IConfiguration configuration;
        private readonly IUpdateDatabaseLatest updateDatabaseLatest;

        private const string PostgresHost = "Postgres:host";
        private const string PostgresPort = "Postgres:port";
        private const string PostgresPassword = "Postgres:password";
        private const string BackupBucket = "Backups";

        public CreatePalavyrSnapshot(
            IPostgresBackup postgresBackup,
            IUserDataBackup userDataBackup,
            IConfiguration configuration,
            IUpdateDatabaseLatest updateDatabaseLatest
        )
        {
            this.postgresBackup = postgresBackup;
            this.userDataBackup = userDataBackup;
            this.configuration = configuration;
            this.updateDatabaseLatest = updateDatabaseLatest;
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

            await updateDatabaseLatest.UpdateLatestBackupRecords(latestDatabaseBackup, latestUserDataBackup);
        }
    }
}