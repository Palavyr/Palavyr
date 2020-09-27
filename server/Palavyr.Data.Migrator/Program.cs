using System;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;


namespace Palavyr.Data.Migrator
{
    class Program
    {
        static int Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            Console.WriteLine($"Current env: {env}");
            var appsettings = $"appsettings.{env}.migrator.json";

            var assembly = Assembly.GetExecutingAssembly();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.migrator.json", true)
                .AddJsonFile(appsettings, false)
                .AddUserSecrets(assembly, true)
                .Build();
            
            var accountsRes = ApplyMigrations(env, "AccountsContextPostgres", "accounts_migration", configuration);
            if (accountsRes == -1) return -1;
            
            var convoRes = ApplyMigrations(env, "ConvoContextPostgres", "convo_migration", configuration);
            if (convoRes == -1) return -1;
            
            var configRes = ApplyMigrations(env, "DashContextPostgres", "configuration_migration", configuration);
            if (configRes == -1) return -1;
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }

        private static int ApplyMigrations(string env, string configKey, string filterName, IConfiguration config)
        {
            var connection = config.GetConnectionString(configKey);
            EnsureDatabase.For.PostgresqlDatabase(connection);
            var accountsRes = DeployMigration(connection, filterName);
            return accountsRes;
        }
        
        private static int DeployMigration(string connectionString, string filterName)
        {
            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), name => name.Contains(filterName)) // reflection used to get the db context
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