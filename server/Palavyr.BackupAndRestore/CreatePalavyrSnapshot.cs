using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.Core.Common.UIDUtils;

namespace Palavyr.BackupAndRestore
{
    public class CreatePalavyrSnapshot : ICreatePalavyrSnapshot
    {
        private readonly ILogger<CreatePalavyrSnapshot> logger;
        private readonly IPostgresBackup postgresBackup;
        private readonly IUserDataBackup userDataBackup;
        private readonly IConfiguration configuration;
        private readonly IUpdateDatabaseLatest updateDatabaseLatest;

        private const string PostgresHost = "Postgres:host";
        private const string PostgresPort = "Postgres:port";
        private const string PostgresPassword = "Postgres:password";
        private const string BackupBucket = "Backups";

        public CreatePalavyrSnapshot(
            ILogger<CreatePalavyrSnapshot> logger,
            IPostgresBackup postgresBackup,
            IUserDataBackup userDataBackup,
            IConfiguration configuration,
            IUpdateDatabaseLatest updateDatabaseLatest
        )
        {
            this.logger = logger;
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

            logger.LogDebug("Creating a new backup of the Palavyr user data and database");
            var latestDatabaseBackup = await postgresBackup.CreateFullDatabaseBackup(host, port, pass, snapshotTimeStamp, bucket);
            var latestUserDataBackup = await userDataBackup.CreateFullUserDataBackup(snapshotTimeStamp, bucket);
            
            logger.LogDebug("Updating the database records now!");
            await updateDatabaseLatest.UpdateLatestBackupRecords(latestDatabaseBackup, latestUserDataBackup);
        }
    }
}