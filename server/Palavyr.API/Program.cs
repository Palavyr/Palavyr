using Amazon.Lambda.AspNetCoreServer;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Palavyr.Core.GlobalConstants;

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
            var host = Host
                .CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureLogging(
                    (hostingContext, logging) =>
                    {
                        logging.ClearProviders();
                        logging.AddConfiguration(hostingContext.Configuration.GetSection(ApplicationConstants.ConfigSections.LoggingSection));
                        logging.SetMinimumLevel(LogLevel.Trace);
                        logging.AddConsole();
                        logging.AddDebug();
                        logging.AddEventSourceLogger();
                        logging.AddNLog();
                        logging.AddSeq();
                    })
                .UseNLog();
            return host;
        }
    }


    // On Lambda, Program.Main is **not** executed. Instead, Lambda loads this DLL
    // into its own app and uses the following class to translate from the Lambda
    // protocol to the standard ASP.Net Core web host and middleware pipeline.
    public class LambdaHandler : APIGatewayHttpApiV2ProxyFunction<Startup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Program.CreateHostBuilder(null);
        }
    }
}