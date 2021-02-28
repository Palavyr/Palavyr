using AspNetCore.Testing.Authentication.ClaimInjector;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.API;

namespace Palavyr.IntegrationTests.AppFactory
{
    public class IntegrationTestFixtureFactory : WebApplicationFactory<Startup>
    {
    }
}