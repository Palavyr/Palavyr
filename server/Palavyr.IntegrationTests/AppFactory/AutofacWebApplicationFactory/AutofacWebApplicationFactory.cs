using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Palavyr.API;
using Palavyr.Core.GlobalConstants;

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
            client.DefaultRequestHeaders.Add(ApplicationConstants.MagicUrlStrings.Action, ApplicationConstants.MagicUrlStrings.SessionAction);
            client.DefaultRequestHeaders.Add(ApplicationConstants.MagicUrlStrings.SessionId, IntegrationConstants.SessionId);
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            client.BaseAddress = new Uri(IntegrationConstants.BaseUri);
            base.ConfigureClient(client);
        }
    }
}