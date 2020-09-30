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

        private ILogger _logger { get; set; }

        int Main(string[] args)
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

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.User);

            _logger.LogInformation($"Data Migrations being performed in {env}");

            var assembly = Assembly.GetExecutingAssembly();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.migrator.json", true)
                .AddUserSecrets(assembly, true)
                .Build();
            
            var accountsRes = ApplyMigrations(env, AccountConfigKey, AccountFilterName, configuration);
            if (accountsRes == -1) return -1;
            
            var convoRes = ApplyMigrations(env, ConvoConfigKey, ConvoFilterName, configuration);
            if (convoRes == -1) return -1;
            
            var configRes = ApplyMigrations(env, ConfigConfigKey, ConfigFilterName, configuration);
            if (configRes == -1) return -1;
            _logger.LogInformation("Successfully updated the database!");
            return 0;
        }

        private int ApplyMigrations(string env, string configKey, string filterName, IConfiguration config)
        {
            _logger.LogInformation($"Deploying migration for {configKey} in {env}.");
            var connection = config.GetConnectionString(configKey);
            EnsureDatabase.For.PostgresqlDatabase(connection);
            var accountsRes = DeployMigration(connection, filterName);
            return accountsRes;
        }
        
        private int DeployMigration(string connectionString, string filterName)
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