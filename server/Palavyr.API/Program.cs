using Amazon.Lambda.Core;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Palavyr.Core.GlobalConstants;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

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
                .ConfigureWebHostDefaults(webBuilder => webBuilder
                    .UseIIS()
                    .UseStartup<Startup>())
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
    //For ASP.NET Core 3.1 the pattern shifted to use the more generic host builder, IHostBuilder
    //https://aws.amazon.com/blogs/developer/one-month-update-to-net-core-3-1-lambda/


    // On Lambda, Program.Main is **not** executed. Instead, Lambda loads this DLL
    // into its own app and uses the following class to translate from the Lambda
    // protocol to the standard ASP.Net Core web host and middleware pipeline.
    public class LambdaHandler : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
    {
        protected override void Init(IHostBuilder builder)
        {
            builder
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
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
        }

        protected override void Init(IWebHostBuilder builder)
        {
            builder.UseStartup<Startup>();
        }
    }
}