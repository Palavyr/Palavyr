using System;
using System.IO;
using System.Threading.Tasks;
using EmailService.ResponseEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem.FormPaths;

namespace Palavyr.BackupAndRestore.Postgres
{
    public class PostgresRestore : PostgresBase
    {
        private readonly ILogger<PostgresBackup> logger;

        private const string FailMessage = "Database restore check failure. Investigate now.";

        public PostgresRestore(ISesEmail emailClient, ILogger<PostgresBackup> logger) : base(emailClient, logger)
        {
            this.logger = logger;
        }

        public async Task GenerateFullRestore(string tempDirectory, string host, string port, string password)
        {
            foreach (var database in DatabaseConstants.Databases)
            {
                var inputFile = Path.Combine(tempDirectory, database + ".sql");
                await RestoreFromPostgreSqlBackup(inputFile, host, port, database, password);
            }
        }

        public async Task Cleanup(string tempDirectory, string host, string port, string password)
        {
            foreach (var database in DatabaseConstants.Databases)
            {
                logger.LogDebug($"Cleaning up database after restore: {database}");
                var cleanupCommand = $"{GetSetPassword(password)}{Newline}"
                                     + GetTerminateProcessCommand(host, port, database)
                                     + GetDropDbCommand(host, port, database);
                var failMessage = $"Database cleanup failed on {database}. Investigate Now.";
                await Execute(cleanupCommand, failMessage);
            }

            logger.LogDebug($"Deleting TempDirection {tempDirectory}");
            DiskUtils.DeleteTempDirectory(tempDirectory);
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
            var restoreCommand = "pg_restore";
            return $"{restoreCommand}"
                   + GetHost(host)
                   + GetPort(port)
                   + GetDatabase(database)
                   + GetUser();
        }

        private string CreateDbCommand(string host, string port, string database)
        {
            var createDbCommand = "createdb ";
            return $"{createDbCommand}"
                   + GetHost(host)
                   + GetPort(port)
                   + GetUser()
                   + $" {database}"
                   + $"{Newline}";
        }

        private string GetDropDbCommand(string host, string port, string database)
        {
            var dropCommand = "dropdb ";
            return $"{dropCommand}" + GetHost(host) + GetPort(port) + GetUser() + $" {database}" + $"{Newline}";
        }

        private string GetTerminateProcessCommand(string host, string port, string database)
        {
            var psqlCommand = $"psql{Space}";
            var terminateProcess = $"\"select pg_terminate_backend(pid) from pg_stat_activity where datename = '{database}'\"";
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