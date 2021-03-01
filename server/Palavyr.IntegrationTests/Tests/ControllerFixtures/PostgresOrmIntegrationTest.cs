﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Palavyr.API;
using Palavyr.Domain.Configuration.Schemas;
using Palavyr.IntegrationTests.AppFactory;
using Xunit;

namespace Palavyr.IntegrationTests.Tests.ControllerFixtures
{
    public class PostgresOrmIntegrationTest : IClassFixture<PostgresOrmWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<Startup> factory;
        private HttpClient client;
        private const string Route = "configure-conversations/ensure-db-valid";

        public PostgresOrmIntegrationTest(PostgresOrmWebApplicationFactory<Startup> factory)
        {
            var configured = factory.ConfigureAppFactory(
                DbSetupAndTeardown.SeedTestAccount,
                db =>
                {
                    db.WidgetPreferences.Add(WidgetPreference.CreateEmpty(IntegrationConstants.AccountId));
                    db.SaveChanges();
                });
            this.client = configured.CreateClient();
            this.factory = configured;
        }

        [Fact]
        public async Task EnsuresDbIsValid()
        {
            var response = await client.PostAsyncWithoutContent(Route);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            factory.DisposeByDelete();
        }
    }
}