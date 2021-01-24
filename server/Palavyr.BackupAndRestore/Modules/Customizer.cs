using System.Reflection;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Palavyr.BackupAndRestore.Modules
{
    public static class Customizer
    {
        public static ContainerBuilder CustomizeContainer(ContainerBuilder builder)
        {
            var config = GetConfiguration();
            builder.RegisterModule(new Configuration(config));
            builder.RegisterModule(new AmazonModule(config));
            builder.RegisterModule(new DbContextModule(config));
            builder.RegisterModule(new DatabaseAndUserDataModule());
            return builder;
        }
        
        static IConfiguration GetConfiguration()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.BackupAndRestore.json", true)
                .AddUserSecrets(assembly, true)
                .Build();

        }

    }
}