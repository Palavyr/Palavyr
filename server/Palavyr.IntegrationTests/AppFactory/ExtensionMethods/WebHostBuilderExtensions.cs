#nullable enable
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.Core.Data;
using Palavyr.IntegrationTests.AppFactory.TestAuthentication;

// We using 3.1 but...
//https://www.thinktecture.com/en/entity-framework-core/isolation-of-integration-tests-in-2-1/

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureAuthentication(this IWebHostBuilder builder)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        services
                            .AddAuthentication("Test")
                            .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>(
                                "Test",
                                opt => { }
                            );
                    });
        }

        public static IWebHostBuilder ConfigureInMemoryDatabase(this IWebHostBuilder builder, InMemoryDatabaseRoot dbRoot)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        services.ClearDescriptors();
                        services.ConfigureInMemoryDatabases(dbRoot);
                        services.CreateDatabases();
                    });
        }

        public static IWebHostBuilder ConfigureAndCreateRealTestDatabase(this IWebHostBuilder builder)
        {
            return builder
                .ConfigureTestServices(
                    services =>
                    {
                        services.ClearDescriptors();
                        services.AddRealDatabaseContexts();
                        services.CreateDatabases();
                    });
        }

        public static HttpClient ConfigureInMemoryClient(this WebApplicationFactory<Startup> builder)
        {
            var client = builder.CreateClient();
            client.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            return client;
        }
    }
}