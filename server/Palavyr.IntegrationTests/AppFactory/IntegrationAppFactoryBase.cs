using System;
using AspNetCore.Testing.Authentication.ClaimInjector;
using Palavyr.API;
using Xunit;

namespace Palavyr.IntegrationTests.AppFactory
{
    public abstract class IntegrationAppFactoryBase : IClassFixture<ClaimInjectorWebApplicationFactory<Startup>>
    {
        protected readonly ClaimInjectorWebApplicationFactory<Startup> Factory;
        protected const string Localhost = "http://localhost/";
        protected const string BasePath = "api/";

        public IntegrationAppFactoryBase(ClaimInjectorWebApplicationFactory<Startup> factory)
        {
            factory.ClientOptions.BaseAddress = new Uri(Localhost + BasePath);
            Factory = factory;
        }
    }
}