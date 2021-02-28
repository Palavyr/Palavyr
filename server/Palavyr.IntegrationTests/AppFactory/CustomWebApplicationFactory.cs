using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.Common.RequestsTools;

namespace Palavyr.IntegrationTests.AppFactory
{
    public class InMemoryWebApplicationFactory<TStartup> : CustomWebApplicationFactoryBase<TStartup> where TStartup : class
    {
    }

    public class PostgresOrmWebApplicationFactory<TStartup> : CustomWebApplicationFactoryBase<TStartup> where TStartup : class
    {

    }

    public class CustomWebApplicationFactoryBase<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add(MagicUrlStrings.Action, MagicUrlStrings.SessionAction);
            client.DefaultRequestHeaders.Add(MagicUrlStrings.SessionId, IntegrationConstants.SessionId);
            base.ConfigureClient(client);
        }
    }
}