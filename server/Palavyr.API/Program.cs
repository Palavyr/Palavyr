using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Configuration;
using Serilog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Palavyr.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            ConfigurationGetter.GetConfiguration();

            // The initial "bootstrap" logger is able to log errors during start-up. It's completely replaced by the
            // logger configured in `UseSerilog()` below, once configuration and dependency-injection have both been
            // set up successfully.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            Log.Information("Starting Palavyr Server!");

            try
            {
                CreateHostBuilder(args).Build().Run();
                Log.Information("Stopped cleanly");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured while starting up Palavyr...");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = ConfigurationGetter.GetConfiguration();
            var host = Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(
                    (hostingContext, logging) =>
                    {
                        var envGetter = new DetermineCurrentEnvironment(config);

                        logging.ClearProviders();
                        if (envGetter.IsDevelopment())
                        {
                            logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
                            logging.AddDebug();
                            logging.AddSerilog();
                            logging.SetMinimumLevel(LogLevel.Trace);
                            logging.AddConsole();
                        }

                        logging.AddSeq(serverUrl: config.SeqUrl);
                    })
                .ConfigureWebHostDefaults(
                    webBuilder => webBuilder.UseStartup<Startup>()
                );
            return host;
        }
    }
}