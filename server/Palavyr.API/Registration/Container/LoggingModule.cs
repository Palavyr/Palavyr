using Autofac;
using Palavyr.Core.Configuration;
using Serilog;

namespace Palavyr.API.Registration.Container;

internal sealed class LoggingModule : Module
{
    private const string LogApplicationProperty = "Application";
    private const string LogApplicationValue = "Palavyr";

    public static void BootstrapLogger(ConfigContainerServer config)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .Enrich.WithProperty(LogApplicationProperty, LogApplicationValue)
            .WriteTo.Seq(config.SeqUrl) // TODO: prod key?
            .WriteTo.Console()
            .CreateLogger();
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(Log.Logger).As<ILogger>().SingleInstance();
        base.Load(builder);
    }
}