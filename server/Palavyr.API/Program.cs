using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Palavyr.Common.Constants;

namespace Palavyr.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            // in case we need it
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "NOT SET or NOT FOUND";
            var host = Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureLogging(
                    (hostingContext, logging) =>
                    {
                        logging.ClearProviders();
                        logging.AddConfiguration(hostingContext.Configuration.GetSection(ConfigSections.LoggingSection));
                        logging.SetMinimumLevel(LogLevel.Trace);
                        logging.AddConsole();
                        if (env != "Production")
                        {
                            logging.AddDebug();
                        }
                        logging.AddEventSourceLogger();
                        logging.AddNLog();
                        logging.AddSeq();
                    })
                .UseNLog();
            return host;
        }
    }
}