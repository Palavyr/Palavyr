using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Palavyr.IntegrationTests.AppFactory.TestStartup;

namespace Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory
{
    public class InMemoryAutofacWebApplicationFactory : AutofacWebApplicationFactory
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(x => x.UseStartup<InMemoryTestStartup>());
        }
    }
}