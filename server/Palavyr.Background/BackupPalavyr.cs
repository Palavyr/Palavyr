using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EmailService.ResponseEmail;
using Palavyr.Common.FileSystem.FormPaths;
using Palavyr.FileSystem.UIDUtils;
using System.IO.Compression;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem;


namespace Palavyr.Background
{
    public interface IBackupPalavyr
    {
        Task CreatePostgreSqlBackup(string outFile, string host, string port, string database, string password);
        Task RestoreFromPostgreSqlBackup(string inputFile, string host, string port, string database, string password);
        Task GenerateFullBackup(string host, string port, string password);
    }

    public class BackupPalavyr : IBackupPalavyr
    {
        private readonly ISesEmail emailClient;
        private readonly IAmazonS3 s3Client;
        private readonly ILogger<BackupPalavyr> logger;
        private const string PGPASSWORD = "PGPASSWORD";
        private const string Newline = "\n";
        private const string Space = " ";

        public BackupPalavyr(ISesEmail emailClient, IAmazonS3 s3Client, ILogger<BackupPalavyr> logger)
        {
            this.emailClient = emailClient;
            this.s3Client = s3Client;
            this.logger = logger;
        }
        
        private string setEnvVarCommand => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";

        private string GetHost(string host) => $"-h{Space}{host}{Space}";
        private string GetPort(string port) => $"-p{Space}{port}{Space}";
        private string GetUser() => $"-U{Space}postgres{Space}";
        private string GetDatabase(string database) => $"-d{Space}{database}{Space}";
        private string GetSetPassword(string password) => $"{setEnvVarCommand}{Space}{PGPASSWORD}={password}";
        private string UserDataDirectory => Path.Combine(MagicPathStrings.InstallationRoot, MagicPathStrings.UserData);

        public async Task GenerateFullBackup(string host, string port, string password)
        {
            var accounts = "Accounts";
            var configuration = "Configuration";
            var conversation = "Conversations";
            var snapshotTimeStamp = DateTime.Now.ToString(TimeUtils.DateTimeFormat);
            var backupTempDirectory = FormDirectoryPaths.FormTempDbBackupDirectory();
            
            // this double the database size on disk.
            var databases = new[] {accounts, configuration, conversation};
            foreach (var databaseName in databases)
            {
                var outfileName = string.Join("-", new[]{databaseName, snapshotTimeStamp});
                var outfilePath = FormDatabaseBackupPaths.FormDbBackupPath(outfileName);
                await CreatePostgreSqlBackup(outfilePath, host, port, databaseName, password);
            }

            var userDataZipFilePath = $"Palavyr-UserData-{snapshotTimeStamp}";
            ZipFile.CreateFromDirectory(UserDataDirectory, Path.Combine(backupTempDirectory, userDataZipFilePath));
            
            // this then triples the size on disk. Max DB size is then 
            var directoryToZip = FormDirectoryPaths.FormTempDbBackupDirectory();
            var zipFilePath = Path.Combine(backupTempDirectory, $"PalavyrData-{snapshotTimeStamp}");
            ZipFile.CreateFromDirectory(directoryToZip, zipFilePath);


            // send to S3, and then 
            var snapshotName = $"{Utils.Databases}.{snapshotTimeStamp}.zip";
            await SaveZipToS3(zipFilePath, snapshotTimeStamp, snapshotName);
            
            // delete the temp directory if all okay
            DiskUtils.DeleteDbBackupFolder();
        }

        private async Task SaveZipToS3(string zipPath, string snapshotTimeStamp, string snapshotName)
        {
            
            var fileKey = Path.Combine(Utils.SnapshotsDir, snapshotTimeStamp, snapshotName).Replace("\\", "/");
            var putRequest = new PutObjectRequest()
            {
                BucketName = Utils.ArchivesBucket,
                FilePath = zipPath,
                Key = fileKey
            };
            try
            {
                var response = await s3Client.PutObjectAsync(putRequest);
                logger.LogInformation($"Saved {zipPath} to {fileKey} in {Utils.ArchivesBucket}");
            }
            catch (Exception ex)
            {
                logger.LogInformation("Failed to write snapshot files: " + ex.Message);
                Console.WriteLine(ex);
            }
        }
        
        
        public async Task CreatePostgreSqlBackup(string outFile, string host, string port, string database, string password)
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

            await Execute(command);
        }


        //psql command disconnect database
        //dropdb and createdb  remove database and create.
        //pg_restore restore database with file create with pg_dump command
        public async Task RestoreFromPostgreSqlBackup(
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
            await Execute(command);
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


        private static ProcessStartInfo ProcessInfoByOs(string batFilePath)
        {
            ProcessStartInfo info;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                info = new ProcessStartInfo(batFilePath) { };
            }
            else
            {
                info = new ProcessStartInfo("sh")
                {
                    Arguments = $"{batFilePath}"
                };
            }

            info.CreateNoWindow = true;
            info.UseShellExecute = false;
            info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory ?? ""; // Set a working direcotyr here
            info.RedirectStandardError = true;
            return info;
        }

        private Task Execute(string dumpCommand)
        {
            return Task.Run(
                () =>
                {
                    var batFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}." + (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "bat" : "sh"));
                    var info = ProcessInfoByOs(batFilePath);
                    try
                    {
                        var batchContent = "";
                        batchContent += $"{dumpCommand}";
                        File.WriteAllText(batFilePath, batchContent, Encoding.ASCII);
                        using var proc = Process.Start(info);
                        if (proc != null)
                        {
                            proc.WaitForExit();
                            var exit = proc.ExitCode;

                            proc.Close();
                        }
                        else
                        {
                            throw new AccessViolationException($"Could not retrieve process when attempting to execute the backup/restore command: {info.ArgumentList}");
                        }
                    }
                    catch (Exception e)
                    {
                        emailClient.SendEmail(
                            "palavyr@gmail.com",
                            "paul.e.gradie@gmail.com",
                            "DATABASE RESTORE FAILURE",
                            "<h3>Database backup and restore check failure. Investigate now.</h3>",
                            "Database backup and restore check failure. Investigate now.");
                    }
                    finally
                    {
                        if (File.Exists(batFilePath))
                        {
                            File.Delete(batFilePath);
                        }
                    }
                });
        }
    }
}