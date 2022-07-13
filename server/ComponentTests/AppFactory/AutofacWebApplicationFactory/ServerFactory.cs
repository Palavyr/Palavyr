using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Palavyr.API;
using Test.Common;

namespace Component.AppFactory.AutofacWebApplicationFactory
{

    
    public interface IComponentTestFixture : ITestBase
    {
        
    }
    
    public class ServerFactory : AutofacWebApplicationFactory, IIntegrationTestFixture
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(x => x.UseStartup<Startup>());
        }
    }
}