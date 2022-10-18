using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Palavyr.API.Registration.Container;
using Palavyr.Core.Configuration;
using Serilog;

namespace Palavyr.API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var config = ConfigurationGetter.GetConfiguration();
            LoggingModule.BootstrapLogger(config);
            
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
            var host = Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
            return host;
        }
    }
}