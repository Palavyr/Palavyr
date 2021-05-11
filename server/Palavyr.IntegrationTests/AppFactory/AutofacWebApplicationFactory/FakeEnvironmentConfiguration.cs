using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory
{
    public class FakeEnvironmentConfiguration : IWebHostEnvironment
    {
        public string EnvironmentName { get; set; }
        public string ApplicationName { get; set; }
        public string ContentRootPath { get; set; }
        public IFileProvider ContentRootFileProvider { get; set; }
        public IFileProvider WebRootFileProvider { get; set; }
        public string WebRootPath { get; set; }

        public bool IsDevelopment()
        {
            return true;
        }
    }
}