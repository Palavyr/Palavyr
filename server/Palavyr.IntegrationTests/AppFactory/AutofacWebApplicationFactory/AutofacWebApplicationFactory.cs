using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Palavyr.API;
using Palavyr.Core.Common.RequestsTools;

//https://github.com/autofac/Autofac/issues/1207#issuecomment-701961371

namespace Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory
{
    public class AutofacWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseServiceProviderFactory(new CustomServiceProviderFactory());
            return base.CreateHost(builder);
        }

        protected override void ConfigureClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add(MagicUrlStrings.Action, MagicUrlStrings.SessionAction);
            client.DefaultRequestHeaders.Add(MagicUrlStrings.SessionId, IntegrationConstants.SessionId);
            client.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            base.ConfigureClient(client);
        }
    }
}