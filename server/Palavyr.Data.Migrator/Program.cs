using System;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using static Microsoft.Extensions.Hosting.Environments;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Palavyr.Data.Migrator
{
    class Program
    {
        static int Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            Console.WriteLine($"Current env: {env}");
            var appsettings = $"appsettings.{env}.json";

            var assembly = Assembly.GetExecutingAssembly();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile(appsettings, false)
                .AddUserSecrets(assembly, true)
                .Build();


            string accountsConnection;
            string convoConnection;
            string dashConnection;
            if (env == Development)
            {
                Console.WriteLine("USING SECRETS ON DEV");
                accountsConnection = configuration.GetConnectionString("DevAccountsContextPostgres");
                convoConnection = configuration.GetConnectionString("DevConvoContextPostgres");
                dashConnection = configuration.GetConnectionString("DevDashContextPostgres");
            }
            else
            {
                Console.WriteLine("USING SECRETS ON PROD/STAGING");
                accountsConnection = configuration.GetConnectionString("AccountsContextPostgres");
                convoConnection = configuration.GetConnectionString("ConvoContextPostgres");
                dashConnection = configuration.GetConnectionString("DashContextPostgres");
                Console.WriteLine($"CONNECTIONS: {accountsConnection}");
            }
            
            EnsureDatabase.For.PostgresqlDatabase(accountsConnection);
            EnsureDatabase.For.PostgresqlDatabase(convoConnection);
            EnsureDatabase.For.PostgresqlDatabase(dashConnection);

            var accountsRes = ApplyMigration(accountsConnection);
            if (accountsRes == -1) return -1;
            
            var convoRes = ApplyMigration(convoConnection);
            if (convoRes == -1) return -1;
            
            var configRes = ApplyMigration(dashConnection);
            if (configRes == -1) return -1;
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private static int ApplyMigration(string connectionString)
        {
            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()) // reflection used to get the db context
                    .LogToConsole()
                    .WithTransactionPerScript()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif                
                return -1;
            }

            return 0;
        }
    }
}