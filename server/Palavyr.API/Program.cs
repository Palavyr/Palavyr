using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Palavyr.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var builder = WebHost.CreateDefaultBuilder(args);
            builder.ConfigureLogging((hostingContext, logging) =>
            {
                logging.ClearProviders();

                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.SetMinimumLevel(LogLevel.Trace);

                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
                logging.AddNLog();
            })
                .UseNLog();
            
            Console.WriteLine("Server is running: " + env);
            
            OperatingSystem osVersion = Environment.OSVersion;
            Console.WriteLine($"PROGRAM OS Platform: {osVersion.Platform.ToString()}");
            if (osVersion.Platform != PlatformID.Unix)
            {
                if (env == Environments.Staging || env == Environments.Production)
                    builder.UseIIS();
            }

            builder.UseStartup<Startup>();
            return builder;
        }
    }
}