using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.API;
using Palavyr.Core.GlobalConstants;

namespace Palavyr.IntegrationTests.AppFactory.ExtensionMethods
{
    public static class WebHostBuilderExtensions
    {
        public static HttpClient ConfigureInMemoryClient(this WebApplicationFactory<Startup> builder, string sessionId)
        {
            var client = builder.CreateClient();
            client.BaseAddress = new Uri(BaseUriBuilder.BuildBaseUri());
            client.DefaultRequestHeaders.Add(ApplicationConstants.MagicUrlStrings.SessionId, sessionId);
            return client;
        }

        public static HttpClient ConfigureInMemoryApiKeyClient(this WebApplicationFactory<Startup> builder, string apikey)
        {
            var client = builder.CreateClient();
            client.BaseAddress = new Uri(BaseUriBuilder.BuildBaseUri());

            client.DefaultRequestHeaders.Remove("action");
            client.DefaultRequestHeaders.Add("action", "apiKeyAccess");

            // if we can provide the apikey inside the client, that makes setup easier since we have extension methods around the client currently.
            client.DefaultRequestHeaders.Add("test-only-apikey", apikey);

            return client;
        }
    }
}