using System;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Xunit;

namespace Palavyr.IntegrationTests.AppFactory.FixtureBase
{
    public class InMemoryIntegrationFixture : IClassFixture<InMemoryAutofacWebApplicationFactory>, IDisposable
    {
        public virtual void Dispose()
        {
        }
    }
}