using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Amazon.Lambda.Core;
using DbUp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Configuration;
using Palavyr.Core.Data;
using Polly;
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

            var configuration = ConfigurationGetter.GetConfigurationForMigrator();

            var connectionString = configuration.DbConnectionString;

            var result = -1;
            Policy.Handle<Exception>().WaitAndRetry(retryCount: 5, i => TimeSpan.FromSeconds(i * 5)).Execute(
                () =>
                {
                    Logger.LogInformation($"Attempting to apply migrations...");
                    result = ApplyMigrations(connectionString);
                });

            if (result == -1) return -1;

            Logger.LogInformation("Successfully updated the database!");
            return 0;
        }

        private static int ApplyMigrations(string connectionString)
        {
            Logger.LogInformation("Connection String: {ConnectionString}", connectionString);
            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgradeEngine = DeployChanges
                .To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .WithTransactionPerScript()
                .WithVariablesDisabled()
                .Build();

            var result = upgradeEngine.PerformUpgrade();

            if (result.Successful) return 0;
            Logger.LogCritical("Encountered error while running migration for {ConnectionString}. Error: {Error}", result.Error, connectionString);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
            return -1;
        }

        private static void EnsureDatabaseCustom(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDataContexts>();
            optionsBuilder.UseNpgsql(connectionString);

            using (var dbContext = new AppDataContexts(optionsBuilder.Options))
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}