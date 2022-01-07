using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Palavyr.API;
using Test.Common;

namespace Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory
{
    public class IntegrationTestAutofacWebApplicationFactory : AutofacWebApplicationFactory, IIntegrationTestFixture
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(x => x.UseStartup<Startup>());
        }
    }
}