using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;

namespace Palavyr.API
{
    /// <summary>
    /// TO get initial migrations working:
    ///     dotnet ef migrations add init -s .\Palavyr.API\ --project .\Palavyr.Data\
    ///     dotnet ef database update -s ..\Palavyr.API\
    /// </summary>
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

            if (env == EnvironmentName.Staging || env == EnvironmentName.Production)
                builder.UseIIS();
            
            builder.UseStartup<Startup>();
            return builder;
        }
    }
}