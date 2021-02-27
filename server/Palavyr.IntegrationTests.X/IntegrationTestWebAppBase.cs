using System;
using System.Net.Http;
using Palavyr.API;
using Xunit;
using AspNetCore.Testing.Authentication.ClaimInjector;

namespace Palavyr.IntegrationTests.X
{
    public class IntegrationTestWebAppBase : IClassFixture<ClaimInjectorWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient Client;

        public IntegrationTestWebAppBase(ClaimInjectorWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/");
            Client = factory.CreateClient();
        }
    }
}