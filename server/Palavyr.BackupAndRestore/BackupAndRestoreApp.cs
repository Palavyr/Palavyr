using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.BackupAndRestore
{
    public class BackupAndRestoreApp
    {
        private readonly Executor executor;
        private readonly PostgresRestorer postgresRestorer;
        private readonly IConfiguration configuration;
        private readonly IPostgresBackup postgresBackup;
        private readonly IUserDataBackup userDataBackup;
        private readonly UpdateDatabaseLatest updateDatabaseLatest;
        private const string BackupsSection = "Backups";
        private const string PostgresHost = "Postgres:host";
        private const string PostgresPort = "Postgres:port";
        private const string PostgresPassword = "Postgres:password";
        public BackupAndRestoreApp(
            Executor executor, 
            PostgresRestorer postgresRestorer, 
            IConfiguration configuration,
            IPostgresBackup postgresBackup,
            IUserDataBackup userDataBackup,
            UpdateDatabaseLatest updateDatabaseLatest
            )
        {
            this.executor = executor;
            this.postgresRestorer = postgresRestorer;
            this.configuration = configuration;
            this.postgresBackup = postgresBackup;
            this.userDataBackup = userDataBackup;
            this.updateDatabaseLatest = updateDatabaseLatest;
        }

        public async Task Execute(string[] args)
        {
            var bucket = configuration.GetSection(BackupsSection).Value;
            var host = configuration.GetSection(PostgresHost).Value;
            var port = configuration.GetSection(PostgresPort).Value;
            var pass = configuration.GetSection(PostgresPassword).Value;

            if (args.Length == 0 || args.Length > 1)
            {
                Console.WriteLine();
                Console.WriteLine("Please provide only one of the following arguments: --check, --restore, --backup");
            }
            
            if (args[0] == "--check")
            {
                await executor.RestoreAndCheckAndCleanup(bucket, host, port, pass);
            }
            else if (args[0] == "--restore")
            {
                await postgresRestorer.PerformStandardRestore( host, port, pass);
                Console.WriteLine("Completed restoring your database!");
            }
            else if (args[0] == "--backup")
            {
                
                var timeStamp = TimeUtils.CreateTimeStamp();
                var latestDatabaseBackup = await postgresBackup.CreateFullDatabaseBackup(host, port, pass, timeStamp, bucket);
                var latestUserDataBackup = await userDataBackup.CreateFullUserDataBackup(timeStamp, bucket);
                await updateDatabaseLatest.UpdateLatestBackupRecords(latestDatabaseBackup, latestUserDataBackup);
                Console.WriteLine();
                Console.WriteLine("Completed creating a new backup!");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Please provide only one of the following arguments: --check, --restore, --backup");
            }
        }
    }
}