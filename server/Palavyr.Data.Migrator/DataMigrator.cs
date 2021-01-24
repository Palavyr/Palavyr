using System;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Palavyr.Data.Migrator
{
    public class DataMigrator
    {
        private const string AccountConfigKey = "AccountsContextPostgres";
        private const string AccountFilterName = "accounts_migration";

        private const string ConvoConfigKey = "ConvoContextPostgres";
        private const string ConvoFilterName = "convo_migration";

        private const string ConfigConfigKey = "DashContextPostgres";
        private const string ConfigFilterName = "configuration_migration";

        private const string Environment = "Environment";
        
        private static ILogger _logger { get; set; }

        static int Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole()
                    .AddEventLog();
            });
            _logger = loggerFactory.CreateLogger<DataMigrator>();
            var assembly = Assembly.GetExecutingAssembly();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.migrator.json", true)
                .AddUserSecrets(assembly, true)
                .Build();
            _logger.LogInformation("This is a test of the logging system.");
            var env = configuration.GetValue<string>("Environment");
            _logger.LogDebug($"This is a debug test to print the env... printing: {env}");
            _logger.LogInformation($"Data Migrations being performed in {env}");

            var accountsRes = ApplyMigrations(env, AccountConfigKey, AccountFilterName, configuration);
            if (accountsRes == -1) return -1;
            
            var convoRes = ApplyMigrations(env, ConvoConfigKey, ConvoFilterName, configuration);
            if (convoRes == -1) return -1;
            
            var configRes = ApplyMigrations(env, ConfigConfigKey, ConfigFilterName, configuration);
            if (configRes == -1) return -1;
            _logger.LogInformation("Successfully updated the database!");
            return 0;
        }

        private static int ApplyMigrations(string env, string configKey, string filterName, IConfiguration config)
        {
            _logger.LogInformation($"Deploying migration for {configKey} in {env}.");
            var connection = config.GetConnectionString(configKey);
            _logger.LogInformation($"Connection String: {connection}");
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

            if (result.Successful) return 0;
            _logger.LogCritical($"Encountered error while running migration for {connectionString}. Error: {result.Error}");
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif                
            return -1;

        }
    }
}