using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.Core.Common.UIDUtils;

namespace Palavyr.BackupAndRestore
{
    public class CreatePalavyrSnapshot : ICreatePalavyrSnapshot
    {
        private readonly ILogger<CreatePalavyrSnapshot> logger;
        private readonly IPostgresBackup postgresBackup;
        private readonly IConfiguration configuration;
        private readonly IUpdateDatabaseLatest updateDatabaseLatest;

        private const string PostgresHost = "Postgres:host";
        private const string PostgresPort = "Postgres:port";
        private const string PostgresPassword = "Postgres:password";
        private const string BackupBucket = "Backups";

        public CreatePalavyrSnapshot(
            ILogger<CreatePalavyrSnapshot> logger,
            IPostgresBackup postgresBackup,
            IConfiguration configuration,
            IUpdateDatabaseLatest updateDatabaseLatest
        )
        {
            this.logger = logger;
            this.postgresBackup = postgresBackup;
            this.configuration = configuration;
            this.updateDatabaseLatest = updateDatabaseLatest;
        }

        public async Task CreateAndTransferCompleteBackup() // TODO: Deprecate once moved to RDP
        {
            var snapshotTimeStamp = TimeUtils.CreateTimeStamp();
            var host = configuration.GetSection(PostgresHost).Value;
            var port = configuration.GetSection(PostgresPort).Value;
            var pass = configuration.GetSection(PostgresPassword).Value;
            var bucket = configuration.GetSection(BackupBucket).Value;

            logger.LogDebug("Creating a new backup of the Palavyr user data and database");
            var latestDatabaseBackup = await postgresBackup.CreateFullDatabaseBackup(host, port, pass, snapshotTimeStamp, bucket);
            
            logger.LogDebug("Updating the database records now!");
            await updateDatabaseLatest.WriteAndSaveRecords(latestDatabaseBackup);
        }
    }
}