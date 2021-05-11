using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class StartupTestConfigurationExtensionMethods
    {
        public static ContainerBuilder CallStartupTestContainerConfiguration(this ContainerBuilder containerBuilder)
        {
            var config = TestConfiguration.GetTestConfiguration();
            Startup.ContainerSetup(containerBuilder, config);
            return containerBuilder;
        }

        public static IServiceCollection CallStartupServicesConfiguration(this IServiceCollection services)
        {
            var config = TestConfiguration.GetTestConfiguration();
            var env = new FakeEnvironmentConfiguration
            {
                EnvironmentName = "Development"
            };
            Startup.SetServices(services, config, env);
            return services;
        }
    }
}