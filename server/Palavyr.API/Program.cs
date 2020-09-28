using System;
using System.Net;
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

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "NOT SET or NOT FOUND";
            Console.WriteLine($"PROGRAM-1: {env.ToString()}");
            var builder = Host
                .CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .ConfigureKestrel(options =>
                            {
                                if (env == Environments.Staging || env == Environments.Production)
                                {
                                    options.Listen(IPAddress.Loopback, 80);
                                    options.Listen(IPAddress.Loopback, 443,
                                        listenOptions =>
                                        {
                                            listenOptions.UseHttps("testCert.pfx",
                                                "testPassword");
                                        });
                                }
                                else
                                {
                                    options.Listen(IPAddress.Loopback, 5000);
                                    options.Listen(IPAddress.Loopback, 5001);
                                }
                            }
                        );
                })
                .ConfigureLogging((hostingContext, logging) =>
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

            Console.WriteLine($"PROGRAM-2: OS Platform: {Environment.OSVersion.Platform.ToString()}");
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                if (env == Environments.Staging || env == Environments.Production)
                    builder.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseIIS(); });
            }
            return builder;
        }
    }
}