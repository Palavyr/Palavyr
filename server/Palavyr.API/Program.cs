using System;
using System.Net;
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
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            Console.WriteLine("PROGRAM-1: {env}");
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
            
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                Console.WriteLine($"PROGRAM-2: OS Platform: {Environment.OSVersion.Platform.ToString()}");
                if (env == Environments.Staging || env == Environments.Production)
                {
                    Console.WriteLine($"PROGRAM-3: ENVIRONMENT = {env}");
                    builder
                        .UseIIS()
                        .UseStartup<Startup>();
                }
                else
                {
                    Console.WriteLine($"PROGRAM-4: ENVIRONMENT = {env}");
                    builder.UseStartup<Startup>();
                }
            }
            else
            {
                Console.WriteLine($"PROGRAM-5: OS Platform: {Environment.OSVersion.Platform.ToString()}");
                builder.UseStartup<Startup>();
            }

            return builder;
        }
    }
}