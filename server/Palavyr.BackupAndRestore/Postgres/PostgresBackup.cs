using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using EmailService.ResponseEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Amazon;
using Palavyr.Amazon.S3Services;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.BackupAndRestore.Postgres
{
    public interface IPostgresBackup
    {
        Task CreateFullDatabaseBackup(string host, string port, string password, TimeUtils timeStamp);
    }

    public class PostgresBackup : PostgresBase, IPostgresBackup
    {
        private readonly ILogger<PostgresBackup> logger;
        private readonly IS3Saver s3Saver;
        private const string FailMessage = "Database backup and restore check failure. Investigate now.";

        public PostgresBackup(ISesEmail emailClient, ILogger<PostgresBackup> logger, IS3Saver s3Saver) : base(emailClient, logger)
        {
            this.logger = logger;
            this.s3Saver = s3Saver;
        }

        public async Task CreateFullDatabaseBackup(string host, string port, string password, TimeUtils timeStamp)
        {
            // this double the database size on disk.
            foreach (var databaseName in DatabaseConstants.Databases)
            {
                var outfilePath = LocalPathUtils.FormLocalTempDbExportPath(databaseName, timeStamp);
                await CreatePostgreSqlBackup(outfilePath, host, port, databaseName, password);
            }
            
            // this then triples the size on disk. Max DB size is then 
            var directoryToZip = FormDirectoryPaths.FormZippableDbBackupDirectory();
            var outputZipPath = LocalPathUtils.FormLocalTempDbZipFilePath(timeStamp);
            ZipFile.CreateFromDirectory(directoryToZip, outputZipPath);

            // send to S3, and then 
            var dbSnapshotNameKey = AmazonPathUtils.FormS3DatabaseBackupKey(timeStamp);
            await s3Saver.SaveZipToS3(AmazonConstants.ArchivesBucket, outputZipPath, dbSnapshotNameKey);

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