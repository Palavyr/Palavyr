using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EmailService.ResponseEmail;
using Microsoft.Extensions.Logging;
using Palavyr.Common.FileSystem;

namespace Palavyr.BackupAndRestore.Postgres
{
    public abstract class PostgresBase
    {
        protected readonly ISesEmail EmailClient;
        protected readonly ILogger Logger;
        protected const string PGPASSWORD = "PGPASSWORD";
        protected const string Newline = "\n";
        protected const string Space = " ";

        public PostgresBase(ISesEmail emailClient, ILogger logger)
        {
            this.EmailClient = emailClient;
            this.Logger = logger;
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
            info.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory ?? ""; // Set a working direcotyr here
            info.RedirectStandardError = true;
            return info;
        }

        protected Task Execute(string dumpCommand, string failMessage)
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
                        EmailClient.SendEmail(
                            "palavyr@gmail.com",
                            "paul.e.gradie@gmail.com",
                            "DATABASE RESTORE FAILURE",
                            $"<h3>{failMessage}</h3>",
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