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
        private readonly Operations operations;
        private readonly PostgresRestorer postgresRestorer;
        private readonly IConfiguration configuration;
        private readonly IPostgresBackup postgresBackup;
        private readonly IUserDataBackup userDataBackup;
        private readonly UpdateDatabaseLatest updateDatabaseLatest;

        public BackupAndRestoreApp(
            Operations operations, 
            PostgresRestorer postgresRestorer, 
            IConfiguration configuration,
            IPostgresBackup postgresBackup,
            IUserDataBackup userDataBackup,
            UpdateDatabaseLatest updateDatabaseLatest
            )
        {
            this.operations = operations;
            this.postgresRestorer = postgresRestorer;
            this.configuration = configuration;
            this.postgresBackup = postgresBackup;
            this.userDataBackup = userDataBackup;
            this.updateDatabaseLatest = updateDatabaseLatest;
        }

        public async Task Execute(string[] args)
        {
            var bucket = configuration.GetSection(DatabaseConstants.BackupsSection).Value;
            var host = configuration.GetSection(DatabaseConstants.PostgresHost).Value;
            var port = configuration.GetSection(DatabaseConstants.PostgresPort).Value;
            var pass = configuration.GetSection(DatabaseConstants.PostgresPassword).Value;

            if (args.Length == 0 || args.Length > 1)
            {
                Console.WriteLine();
                Console.WriteLine("Please provide only one of the following arguments: --check, --restore, --backup");
            }
            
            if (args[0] == "--check")
            {
                await operations.RestoreAndCheckAndCleanup(bucket, host, port, pass);
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