using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Component.AppFactory.AutofacWebApplicationFactory
{
    public class CustomServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private AutofacServiceProviderFactory _wrapped;
        private IServiceCollection services;

        public CustomServiceProviderFactory()
        {
            _wrapped = new AutofacServiceProviderFactory();
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            // Store the services for later.
            this.services = services;

            return _wrapped.CreateBuilder(services);
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            var sp = services.BuildServiceProvider();
#pragma warning disable CS0612
            var filters = sp.GetRequiredService<IEnumerable<IStartupConfigureContainerFilter<ContainerBuilder>>>();
#pragma warning restore CS0612

            foreach (var filter in filters)
            {
                filter.ConfigureContainer(b => { })(containerBuilder);
            }

            return _wrapped.CreateServiceProvider(containerBuilder);
        }
    }
}