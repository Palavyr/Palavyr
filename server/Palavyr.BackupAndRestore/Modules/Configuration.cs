using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Palavyr.BackupAndRestore.Modules
{
    public class Configuration : Module
    {
        private readonly IConfiguration configuration;

        public Configuration(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(
                    context => { return configuration; })
                .As<IConfiguration>();

            
            builder.RegisterType<LoggerFactory>().As<ILoggerFactory>();//.SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));//.SingleInstance();
            // builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).WithParameter("loggerFactory", new LoggerFactory());
        }
    }
}