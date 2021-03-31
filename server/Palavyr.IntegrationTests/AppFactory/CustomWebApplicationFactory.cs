using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Palavyr.Core.Common.RequestsTools;

namespace Palavyr.IntegrationTests.AppFactory
{
    //https://stebet.net/mocking-jwt-tokens-in-asp-net-core-integration-tests/
    public class InMemoryWebApplicationFactory : CustomWebApplicationFactoryBase<InMemoryTestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var dbRoot = new InMemoryDatabaseRoot();
            var builder = Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    x =>
                    {
                        x.UseStartup<OrmTestStartup>()
                            .ConfigureInMemoryDatabase(dbRoot)
                            .EnsureAndConfigureDbs()
                            .UseTestServer();
                    });
            return builder;
        }
    }

    public class PostgresOrmWebApplicationFactory : CustomWebApplicationFactoryBase<OrmTestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var dbRoot = new InMemoryDatabaseRoot();
            var builder = Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    x =>
                    {
                        x.UseStartup<OrmTestStartup>()
                            .ConfigurePostgresOrmDatabase()
                            .EnsureAndConfigureDbs()
                            .UseTestServer();
                    });
            return builder;
        }
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