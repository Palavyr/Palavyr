﻿using System.Threading;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.Core.Sessions;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class StartupTestConfigurationExtensionMethods
    {
        public static ContainerBuilder CallStartupTestContainerConfiguration(this ContainerBuilder builder)
        {
            builder.Register(
                c =>
                {
                    var holder = new AccountIdTransport();
                    holder.Assign(IntegrationConstants.AccountId);
                    return holder;
                }).As<IHoldAnAccountId>().InstancePerLifetimeScope();
            
            builder.Register(
                c =>
                {
                    var ctx = new CancellationTokenSource();
                    var holder = new CancellationTokenTransport();
                    holder.Assign(ctx.Token);
                    return holder;
                }).As<ITransportACancellationToken>().InstancePerLifetimeScope();
            return builder;
        }

        public static IServiceCollection CallStartupServicesConfiguration(this IServiceCollection services)
        {
            var config = TestConfiguration.GetTestConfiguration();
            var env = new FakeEnvironmentConfiguration
            {
                EnvironmentName = "Development"
            };
            // new IntegrationTestStartup().SetServices(services, config, env);
            return services;
        }
    }
}