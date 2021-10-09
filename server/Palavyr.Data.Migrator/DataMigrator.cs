using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Amazon.Lambda.Core;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]


namespace Palavyr.Data.Migrator
{
    // On Lambda, Program.Main is **not** executed. Instead, Lambda loads this DLL
    // into its own app and uses the following class to translate from the Lambda
    // protocol to the standard ASP.Net Core web host and middleware pipeline.
    public class MigratorLambdaHandler
    {
        public int MigratorHandler(object input, ILambdaContext context)
        {
            return DataMigrator.Main(new string[] { });
        }
    }


    public class DataMigrator
    {
        private const string AccountConfigKey = "AccountsContextPostgres";
        private const string AccountFilterName = "accounts_migration";

        private const string ConvoConfigKey = "ConvoContextPostgres";
        private const string ConvoFilterName = "convo_migration";

        private const string ConfigConfigKey = "DashContextPostgres";
        private const string ConfigFilterName = "configuration_migration";

        private const string Environment = "Environment";

        private static ILogger<DataMigrator> Logger { get; set; }

        public static int Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(
                builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("Palavyr.Data.Migrator.DataMigrator", LogLevel.Debug)
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        builder.AddConsole().AddEventLog();
                    }
                });
            Logger = loggerFactory.CreateLogger<DataMigrator>();
            Logger.LogDebug("This is the first thing that happens. A TEST.");
            var assembly = Assembly.GetExecutingAssembly();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.migrator.json", true)
                .AddUserSecrets(assembly, true)
                .Build();

            Logger.LogInformation("This is a test of the logging system.");
            var env = configuration.GetValue<string>(Environment);

            Logger.LogDebug($"This is a debug test to print the env... printing: {env}");
            Logger.LogInformation($"Data Migrations being performed in {env}");

            var accountsRes = ApplyMigrations(env, AccountConfigKey, AccountFilterName, configuration);
            if (accountsRes == -1) return -1;

            var convoRes = ApplyMigrations(env, ConvoConfigKey, ConvoFilterName, configuration);
            if (convoRes == -1) return -1;

            var configRes = ApplyMigrations(env, ConfigConfigKey, ConfigFilterName, configuration);
            if (configRes == -1) return -1;
            Logger.LogInformation("Successfully updated the database!");
            return 0;
        }

        private static int ApplyMigrations(string env, string configKey, string filterName, IConfiguration config)
        {
            Logger.LogInformation($"Deploying migration for {configKey} in {env}.");
            var connection = config.GetConnectionString(configKey);
            Logger.LogInformation($"Connection String: {connection}");
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
            Logger.LogCritical($"Encountered error while running migration for {connectionString}. Error: {result.Error}");
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
            return -1;
        }
    }
}