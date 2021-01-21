using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using EmailService.ResponseEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Amazon.S3Services;
using Palavyr.API.Services.AmazonServices;
using Palavyr.Common.FileSystem.FormPaths;

namespace Palavyr.BackupAndRestore.Postgres
{
    public interface IPostgresBackup
    {
        Task CreateFullDatabaseBackup(string host, string port, string password, string snapshotTimeStamp);
    }

    public class PostgresBackup : PostgresBase, IPostgresBackup
    {
        private readonly ILogger<PostgresBackup> logger;
        private readonly S3Saver s3Saver;
        private const string FailMessage = "Database backup and restore check failure. Investigate now.";

        public PostgresBackup(ISesEmail emailClient, ILogger<PostgresBackup> logger, S3Saver s3Saver) : base(emailClient, logger)
        {
            this.logger = logger;
            this.s3Saver = s3Saver;
        }

        public async Task CreateFullDatabaseBackup(string host, string port, string password, string snapshotTimeStamp)
        {
            var backupTempDirectory = FormDirectoryPaths.FormTempDbBackupDirectory();

            // this double the database size on disk.
            foreach (var databaseName in DatabaseConstants.Databases)
            {
                var outfileName = string.Join("-", new[] {databaseName, snapshotTimeStamp});
                var outfilePath = FormDatabaseBackupPaths.FormDbBackupPath(outfileName);
                await CreatePostgreSqlBackup(outfilePath, host, port, databaseName, password);
            }

            // this then triples the size on disk. Max DB size is then 
            var directoryToZip = FormDirectoryPaths.FormTempDbBackupDirectory();
            var zipFilePath = Path.Combine(backupTempDirectory, $"PalavyrData-{snapshotTimeStamp}");
            ZipFile.CreateFromDirectory(directoryToZip, zipFilePath);

            // send to S3, and then 
            var snapshotName = $"{AmazonConstants.Databases}.{snapshotTimeStamp}.zip";
            await s3Saver.SaveZipToS3(zipFilePath, snapshotTimeStamp, snapshotName);

            // delete the temp directory if all okay
            DiskUtils.DeleteDbBackupFolder();
        }

        private async Task CreatePostgreSqlBackup(string outFile, string host, string port, string database, string password)
        {
            var dumpCommand = $"pg_dump{Space}";
            var formatClean = $"-Fc{Space}";

            var pgDumpCommand = $"{GetSetPassword(password)}{Newline}"
                                + dumpCommand
                                + formatClean
                                + GetHost(host)
                                + GetPort(port)
                                + GetDatabase(database)
                                + GetUser();
            var command = "" + pgDumpCommand + "  > " + "\"" + outFile + "\"" + $"{Newline}";

            if (File.Exists(outFile))
            {
                File.Delete(outFile);
            }

            await Execute(command, FailMessage);
        }
    }
}