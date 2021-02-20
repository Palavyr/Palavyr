using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Services.EmailService.ResponseEmailTools;

namespace Palavyr.BackupAndRestore.Postgres
{
    public abstract class PostgresBase
    {
        protected readonly ISesEmail emailClient;
        protected readonly ILogger logger;
        protected const string PGPASSWORD = "PGPASSWORD";
        protected const string Newline = "\n";
        protected const string Space = " ";

        public PostgresBase(ISesEmail emailClient, ILogger logger)
        {
            this.emailClient = emailClient;
            this.logger = logger;
        }

        protected string SetEnvVarCommand => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "set " : "export ";
        protected string GetHost(string host) => $"-h{Space}{host}{Space}";
        protected string GetPort(string port) => $"-p{Space}{port}{Space}";
        protected string GetUser() => $"-U{Space}postgres{Space}";
        protected string GetDatabase(string database) => $"-d{Space}{database}{Space}";
        protected string GetSetPassword(string password) => $"{SetEnvVarCommand}{Space}{PGPASSWORD}={password}";

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
            info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory ?? ""; // Set a working directory here
            info.RedirectStandardError = true;
            return info;
        }

        protected Task Execute(string dumpCommand, string failMessage)
        {
            logger.LogDebug("Executing postgres command...");
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
                        Console.WriteLine($"Failed to perform backup or restore command: {dumpCommand}");
                        Console.WriteLine($"Error: {e.Message}");
                        logger.LogDebug($"Exception encountered when executing backup and restore. {e.Message} - Sending email.");
                        logger.LogDebug($"Dump Command: {dumpCommand}");
                        emailClient.SendEmail(
                            "palavyr@gmail.com",
                            "paul.e.gradie@gmail.com",
                            "DATABASE RESTORE FAILURE",
                            $"<h3>{failMessage}</h3>",
                            "Database backup and restore check failure. Investigate now.");
                    }
                    finally
                    {
                        logger.LogDebug("Executing Finally block - deleting batfile.");
                        if (File.Exists(batFilePath))
                        {
                            File.Delete(batFilePath);
                        }
                    }
                });
        }
    }
}