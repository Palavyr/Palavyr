#nullable enable
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.API;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class WebHostBuilderExtensions
    {
        public static HttpClient ConfigureInMemoryClient(this WebApplicationFactory<Startup> builder)
        {
            var client = builder.CreateClient();
            client.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            return client;
        }
    }
}