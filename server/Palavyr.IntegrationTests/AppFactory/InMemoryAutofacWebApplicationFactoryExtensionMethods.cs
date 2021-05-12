#nullable enable
using System;
using System.Net.Http;
using Autofac;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Palavyr.API;
using Palavyr.Core.Data;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;

// https: //stackoverflow.com/questions/59680121/unable-to-cast-object-of-type-servicecollection-to-type-autofac-containerbuilde

namespace Palavyr.IntegrationTests.AppFactory
{
    public static class InMemoryAutofacWebApplicationFactoryExtensionMethods
    {
        public static HttpClient CreateInMemAuthedClient(
            this InMemoryAutofacWebApplicationFactory factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
        )
        {
            factory.Server.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            var dbRoot = new InMemoryDatabaseRoot();
            var client = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureTestContainer<ContainerBuilder>(builder => builder.CallStartupTestContainerConfiguration())
                            .ConfigureTestServices(services => services.CallStartupServicesConfiguration().AddMvcCore().AddApplicationPart(typeof(Startup).Assembly))
                            .ConfigureInMemoryDatabase(dbRoot)
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo)
                            .UseTestServer();
                    })
                .CreateClient();
            client.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            return client;
        }

        public static HttpClient ConfigureUnauthenticatedClientWithInMemContext(
            this InMemoryAutofacWebApplicationFactory factory,
            Action<AccountsContext>? configureAccounts = null,
            Action<DashContext>? configureDash = null,
            Action<ConvoContext>? configureConvo = null
        )
        {
            var dbRoot = new InMemoryDatabaseRoot();
            factory.ClientOptions.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            var client = factory
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureTestContainer<ContainerBuilder>(b => b.CallStartupTestContainerConfiguration())
                            .ConfigureTestServices(s => s.CallStartupServicesConfiguration().AddMvcCore().AddApplicationPart(typeof(Startup).Assembly))
                            .ConfigureInMemoryDatabase(dbRoot)
                            .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo)
                            .UseTestServer();
                    })
                .CreateClient();
            client.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            return client;
        }
    }
}