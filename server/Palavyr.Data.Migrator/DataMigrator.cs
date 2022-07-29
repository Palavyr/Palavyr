using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Amazon.Lambda.Core;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Configuration;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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
        private const string ConnectionStringAppSettingsKey = "ConnectionString";
        private const string Environment = "Environment";

        private static ILogger<DataMigrator> Logger { get; set; }

        public static int Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(
                builder =>
                {
                    builder
                        .AddFilter("Microsoft", LogLevel.Debug)
                        .AddFilter("System", LogLevel.Debug)
                        .AddFilter("Palavyr.Data.Migrator.DataMigrator", LogLevel.Debug);
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        builder.AddConsole().AddEventLog();
                    }
                });
            Logger = loggerFactory.CreateLogger<DataMigrator>();
            Logger.LogDebug("{Message}", "This is the first thing that happens. A TEST");

            var configuration = ConfigurationGetter.GetConfiguration();


            Logger.LogInformation("This is a test of the logging system");
            var env = configuration.GetCurrentEnvironment();
            var connectionString = configuration.CorrectConnectionString();

            Logger.LogDebug("This is a debug test to print the env... printing: {Environment}", env);
            Logger.LogInformation("Data Migrations being performed in {Environment}", env);

            var result = ApplyMigrations(env, connectionString, configuration);
            if (result == -1) return -1;

            Logger.LogInformation("Successfully updated the database!");
            return 0;
        }

        private static int ApplyMigrations(string env, string connectionString, IConfiguration config)
        {
            Logger.LogInformation("Deploying migration for in {Environment}", env);
            Logger.LogInformation("Connection String: {ConnectionString}", connectionString);
            EnsureDatabase.For.PostgresqlDatabase(connectionString);
            return DeployMigration(connectionString);
        }

        private static int DeployMigration(string connectionString)
        {
            var upgrader =
                DeployChanges.To
                    .PostgresqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()) // reflection used to get the db context
                    .LogToConsole()
                    .WithTransactionPerScript()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (result.Successful) return 0;
            Logger.LogCritical("Encountered error while running migration for {ConnectionString}. Error: {Error}", result.Error, connectionString);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
            return -1;
        }
    }
}