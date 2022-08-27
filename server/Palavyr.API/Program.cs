using System;
using Amazon.Lambda.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Common.Environment;
using Palavyr.Core.Configuration;
using Serilog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

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
                            // logging.AddDebug();
                            logging.AddSerilog();
                            logging.SetMinimumLevel(LogLevel.Trace);
                            logging.AddConsole();
                        }

                        logging.AddSeq();
                    })
                .ConfigureWebHostDefaults(
                    webBuilder => webBuilder.UseStartup<Startup>()
                );
            return host;
        }
    }
    //For ASP.NET Core 3.1 the pattern shifted to use the more generic host builder, IHostBuilder
    //https://aws.amazon.com/blogs/developer/one-month-update-to-net-core-3-1-lambda/


    // On Lambda, Program.Main is **not** executed. Instead, Lambda loads this DLL
    // into its own app and uses the following class to translate from the Lambda
    // protocol to the standard ASP.Net Core web host and middleware pipeline.
    public class LambdaHandler : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IHostBuilder builder)
        {
            ConfigurationGetter.GetConfiguration();

            builder
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureLogging(
                    (hostingContext, logging) =>
                    {
                        logging.ClearProviders();
                        // logging.AddConfiguration(config.GetSection(ApplicationConstants.ConfigSections.LoggingSection));
                        logging.SetMinimumLevel(LogLevel.Trace);
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.AddEventSourceLogger();
                        logging.AddSeq();
                    });
        }

        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }
    }
}