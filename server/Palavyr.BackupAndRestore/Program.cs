﻿using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleEmail;
using DashboardServer.Data;
using EmailService.ResponseEmail;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Palavyr.Amazon.S3Services;
using Palavyr.BackupAndRestore.Postgres;
using Palavyr.Common.FileSystem;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.FileSystem.UIDUtils;

namespace Palavyr.BackupAndRestore
{
    class BackupAndRestore
    {
        private static readonly string tempRestoreDirectory = "TempRestoreDirectory";
        private const string AccountDbStringKey = "AccountsContextPostgres";
        private const string AccessKeySection = "AWS:AccessKey";
        private const string SecretKeySection = "AWS:SecretKey";
        private const string PostgresHost = "Postgres:host";
        private const string PostgresPort = "Postgres:port";
        private const string PostgresPassword = "Postgres:password";

        static async Task Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(
                builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                        .AddConsole()
                        .AddEventLog();
                });
            var logger = loggerFactory.CreateLogger<BackupAndRestore>();

            var assembly = Assembly.GetExecutingAssembly();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.BackupAndRestore.json", true)
                .AddUserSecrets(assembly, true)
                .Build();

            var accessKey = configuration.GetSection(AccessKeySection).Value;
            var secretKey = configuration.GetSection(SecretKeySection).Value;
            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var host = configuration.GetSection(PostgresHost).Value;
            var port = configuration.GetSection(PostgresPort).Value;
            var pass = configuration.GetSection(PostgresPassword).Value;

            if (args.Length == 0)
            {
                Console.WriteLine("Running in Restore Check mode");

                // 1. create a temp directory
                var tempBackupDirectory = Path.Combine(MagicPathStrings.InstallationRoot, tempRestoreDirectory);
                DiskUtils.CreateDir(tempBackupDirectory);

                // 2. download the latest db archive from S3
                var accountsContext = GetAccountsContext(configuration);
                var backups = await accountsContext.Backups.SingleAsync();
                var latestDbs3Key = backups.LatestFullDbBackup;

                var s3Retriever = new S3Retriever(new AmazonS3Client(credentials), logger);

                var saveToPath = Path.Combine(tempBackupDirectory, "latest.zip");
                await s3Retriever.GetLatestDatabaseBackup(latestDbs3Key, saveToPath);

                // 3. unzip the archive to create 3 .sql files
                ZipFile.ExtractToDirectory(saveToPath, tempRestoreDirectory);

                // 4. restore databases (using the alternate name)
                var emailClient = new AmazonSimpleEmailServiceClient(credentials);
                var restorer = new PostgresRestore(new SesEmail(new Logger<SesEmail>(new LoggerFactory()), emailClient), new Logger<PostgresBackup>(new LoggerFactory()));

                // 5. If any errors are encounters, email paul.e.gradie
                await restorer.GenerateFullRestore(tempBackupDirectory, host, port, pass);

                // 6. Delete all alternate databases just created
                // 7. Delete temp directory
                await restorer.Cleanup(tempBackupDirectory, host, port, pass);
            }
            else if (args[0] == "--restore")
            {
                // TODO: if arg[0] == "Restore to new machine type arg" then use the real table names.
                // for use when we want to move the database to a different VM
                Console.WriteLine("We must move mountains!!");
            }
            else if (args[0] == "--backup")
            {
                var timeStamp = TimeUtils.CreateTimeStamp();
                var backupCreate = new PostgresBackup(new SesEmail(new Logger<SesEmail>(new NLogLoggerFactory()), new AmazonSimpleEmailServiceClient(credentials)), new Logger<PostgresBackup>(new NLogLoggerFactory()), new S3Saver(new AmazonS3Client(credentials), new Logger<S3Saver>(new NLogLoggerFactory())));
                await backupCreate.CreateFullDatabaseBackup(host, port, pass, timeStamp);
            }
        }

        public static AccountsContext GetAccountsContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(AccountDbStringKey);
            var optionsBuilder = new DbContextOptionsBuilder<AccountsContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new AccountsContext(optionsBuilder.Options);
        }
    }
}