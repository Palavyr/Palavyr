using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;


// How to inherit from startup
// https://stackoverflow.com/questions/53269815/webapplicationfactory-throws-error-that-contentrootpath-does-not-exist-in-asp-ne

namespace Palavyr.IntegrationTests.AppFactory.TestStartup
{
    public class OrmTestStartup : Startup
    {
        public OrmTestStartup(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
        }

        // public override void RegisterStores(IServiceCollection services)
        // {
        //     services.ConfigureRealPostgresTestDatabases();
        // }
    }
}