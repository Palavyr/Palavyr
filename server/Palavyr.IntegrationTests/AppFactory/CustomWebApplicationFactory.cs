using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.Common.RequestsTools;

namespace Palavyr.IntegrationTests.AppFactory
{
    public class InMemoryWebApplicationFactory : CustomWebApplicationFactoryBase<InMemoryTestStartup>
    {
    }

    public class PostgresOrmWebApplicationFactory : CustomWebApplicationFactoryBase<OrmTestStartup>
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