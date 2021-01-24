using System;
using System.IO;
using System.Threading.Tasks;
using EmailService.ResponseEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem.FormPaths;

namespace Palavyr.BackupAndRestore.Postgres
{
    public class PostgresRestorer : PostgresBase
    {
        private readonly ILogger<PostgresRestorer> logger;

        private const string FailMessage = "Database restore check failure. Investigate now.";

        public PostgresRestorer(
            ISesEmail emailClient,
            ILogger<PostgresRestorer> logger
        ) : base(emailClient, logger)
        {
            this.logger = logger;
        }

        public async Task PerformStandardRestore(string host, string port, string password)
        {
            var tempRestoreDirectory = FormDirectoryPaths.FormLocalRestoreDirectory();

            foreach (var database in DatabaseConstants.Databases)
            {
                logger.LogDebug($"Restoring to database: {database}");
                var inputFile = Path.Combine(tempRestoreDirectory, database + ".sql");
                await RestoreFromPostgreSqlBackup(inputFile, host, port, database, password);
            }
        }

        public async Task PerformRestoreCheck(string host, string port, string password)
        {
            var tempRestoreDirectory = FormDirectoryPaths.FormLocalRestoreDirectory();
            foreach (var database in DatabaseConstants.Databases)
            {
                logger.LogDebug($"Restoring to database: {database}");
                var inputFile = Path.Combine(tempRestoreDirectory, database + ".sql");
                var altDatabaseName = DatabaseConstants.FormCheckTableName(database);
                await RestoreFromPostgreSqlBackup(inputFile, host, port, altDatabaseName, password);
            }
        }

        public async Task RestoreCheckCleanup(string host, string port, string password)
        {
            var tempRestoreDirectory = FormDirectoryPaths.FormLocalRestoreDirectory();

            foreach (var database in DatabaseConstants.Databases)
            {
                var altDatabaseName = DatabaseConstants.FormCheckTableName(database);

                logger.LogDebug($"Cleaning up database after restore: {altDatabaseName}");
                Console.WriteLine($"Cleaning up : {altDatabaseName}");
                var cleanupCommand = $"{GetSetPassword(password)}{Newline}"
                                     + GetTerminateProcessCommand(host, port, altDatabaseName)
                                     + GetDropDbCommand(host, port, altDatabaseName);
                var failMessage = $"Database cleanup failed on {altDatabaseName}. Investigate Now.";
                await Execute(cleanupCommand, failMessage);
            }

            logger.LogDebug($"Deleting TempDirection {tempRestoreDirectory}");
            DiskUtils.DeleteTempDirectory(tempRestoreDirectory);
        }

        private async Task RestoreFromPostgreSqlBackup(
            string inputFile,
            string host,
            string port,
            string database,
            string password
        )
        {
            var restoreCommand = $"{GetSetPassword(password)}{Newline}"
                                 + GetTerminateProcessCommand(host, port, database)
                                 + GetDropDbCommand(host, port, database)
                                 + CreateDbCommand(host, port, database)
                                 + CreateRestoreCommand(host, port, database);

            var command = $"{restoreCommand} {inputFile}";
            await Execute(command, FailMessage);
        }

        private string CreateRestoreCommand(string host, string port, string database)
        {
            var restoreCommand = $"pg_restore{Space}";
            return $"{restoreCommand}"
                   + GetHost(host)
                   + GetPort(port)
                   + GetDatabase(database)
                   + GetUser();
        }

        private string CreateDbCommand(string host, string port, string database)
        {
            var createDbCommand = $"createdb{Space}";
            return $"{createDbCommand}"
                   + GetHost(host)
                   + GetPort(port)
                   + GetUser()
                   + $" {database}"
                   + $"{Newline}";
        }

        private string GetDropDbCommand(string host, string port, string database)
        {
            var dropCommand = $"dropdb{Space}";
            return $"{dropCommand}" + GetHost(host) + GetPort(port) + GetUser() + $" {database}" + $"{Newline}";
        }

        private string GetTerminateProcessCommand(string host, string port, string database)
        {
            var psqlCommand = $"psql{Space}";
            var terminateProcess = $"\"select pg_terminate_backend(pid) from pg_stat_activity where datname = '{database}'\"";
            return $"{psqlCommand}"
                   + GetHost(host)
                   + GetPort(port)
                   + GetUser()
                   + GetDatabase(database)
                   + $"-c "
                   + $"{terminateProcess}"
                   + $"{Newline}";
        }
    }
}