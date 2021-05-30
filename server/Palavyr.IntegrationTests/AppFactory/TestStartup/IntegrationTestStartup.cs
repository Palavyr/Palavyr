using System;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;

// How to inherit from startup
// https://stackoverflow.com/questions/53269815/webapplicationfactory-throws-error-that-contentrootpath-does-not-exist-in-asp-ne

namespace Palavyr.IntegrationTests.AppFactory.TestStartup
{
    public class IntegrationTestStartup : Startup
    {
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;

        public IntegrationTestStartup(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }

        public override void ConfigureContainer(ContainerBuilder builder)
        {
            Console.WriteLine("CONFIGURE CONTAINER IN MEMORY STARTUP");
            ContainerSetup(builder, configuration);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Configure Services IN MEMORY");
            SetServices(services, configuration, env);
        }
    }
}