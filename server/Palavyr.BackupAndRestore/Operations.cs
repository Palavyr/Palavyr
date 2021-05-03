using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.Core.Common.FileSystemTools.FormPaths;
using Palavyr.Core.Common.UIDUtils;
using Palavyr.Core.Data;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.BackupAndRestore
{
    public class Operations
    {
        private readonly AccountsContext accountsContext;
        private readonly IS3Retriever s3Retriever;
        private readonly PostgresRestorer postgresRestorer;
        private readonly IPostgresBackup postgresBackup;

        public Operations(
            AccountsContext accountsContext,
            IS3Retriever s3Retriever,
            PostgresRestorer postgresRestorer,
            IPostgresBackup postgresBackup
        )
        {
            this.accountsContext = accountsContext;
            this.s3Retriever = s3Retriever;
            this.postgresRestorer = postgresRestorer;
            this.postgresBackup = postgresBackup;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public async Task RestoreAndCheckAndCleanup(string bucket, string host, string port, string password)
        {
            Console.WriteLine("Running in Restore Check mode");

            var tempRestoreDirectory = FormDirectoryPaths.FormLocalRestoreDirectory();

            var backups = await accountsContext.Backups.SingleAsync();
            var latestDbs3Key = backups.LatestFullDbBackup;

            var saveToPath = Path.Combine(tempRestoreDirectory, "latest.zip");
            await s3Retriever.GetLatestDatabaseBackup(bucket, latestDbs3Key, saveToPath);

            ZipFile.ExtractToDirectory(saveToPath, tempRestoreDirectory, true);

            await postgresRestorer.PerformRestoreCheck(host, port, password);
            await postgresRestorer.RestoreCheckCleanup(host, port, password);
        }

        public async Task<string> CreateDatabaseBackup(string bucket, string host, string port, string password)
        {
            var timeStamp = TimeUtils.CreateTimeStamp();
            var latestDatabaseBackup = await postgresBackup.CreateFullDatabaseBackup(host, port, password, timeStamp, bucket);
            return latestDatabaseBackup;
        }
    }
}