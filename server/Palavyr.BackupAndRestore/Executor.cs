using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.EntityFrameworkCore;
using Palavyr.Amazon.S3Services;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.BackupAndRestore.UserData;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.BackupAndRestore
{
    public class Executor
    {
        private readonly AccountsContext accountsContext;
        private readonly IS3Retriever s3Retriever;
        private readonly PostgresRestorer postgresRestorer;
        private readonly IUserDataBackup userDataBackup;
        private readonly IPostgresBackup postgresBackup;

        public Executor(
            AccountsContext accountsContext,
            IS3Retriever s3Retriever,
            PostgresRestorer postgresRestorer,
            IUserDataBackup userDataBackup,
            IPostgresBackup postgresBackup
        )
        {
            this.accountsContext = accountsContext;
            this.s3Retriever = s3Retriever;
            this.postgresRestorer = postgresRestorer;
            this.userDataBackup = userDataBackup;
            this.postgresBackup = postgresBackup;
            
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public async Task RestoreAndCheckAndCleanup(string bucket, string host, string port, string password)
        {
            Console.WriteLine("Running in Restore Check mode");

            // 1. create a temp directory
            var tempRestoreDirectory = FormDirectoryPaths.FormLocalRestoreDirectory();

            // 2. download the latest db archive from S3
            var backups = await accountsContext.Backups.SingleAsync();
            var latestDbs3Key = backups.LatestFullDbBackup;

            // var s3Retriever = new S3Retriever(new AmazonS3Client(credentials), logger);
            var saveToPath = Path.Combine(tempRestoreDirectory, "latest.zip");
            await s3Retriever.GetLatestDatabaseBackup(bucket, latestDbs3Key, saveToPath);

            // 3. unzip the archive to create 3 .sql files
            ZipFile.ExtractToDirectory(saveToPath, tempRestoreDirectory, true);
            
            // 5. If any errors are encounters, email paul.e.gradie
            // 6. Delete all alternate databases just created
            // 7. Delete temp directory
            await postgresRestorer.PerformRestoreCheck(host, port, password);
            await postgresRestorer.RestoreCheckCleanup(host, port, password);
        }

        public async Task<string> CreateDatabaseBackup(string bucket, string host, string port, string password)
        {
            var timeStamp = TimeUtils.CreateTimeStamp();
            var latestDatabaseBackup = await postgresBackup.CreateFullDatabaseBackup(host, port, password, timeStamp, bucket);
            Console.WriteLine();
            Console.WriteLine("Successfully created a backup and write to S3. S3 keys saved to the database.");
            return latestDatabaseBackup;
        }

        public async Task<string> CreateUserDataBackup(TimeUtils timeStamp, string bucket)
        {
            var latestUserDataBackup = await userDataBackup.CreateFullUserDataBackup(timeStamp, bucket);
            return latestUserDataBackup;
        }
    }
}