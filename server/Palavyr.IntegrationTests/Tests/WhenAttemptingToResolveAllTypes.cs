using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assent;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Test.Common.ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests
{
    public class WhenAttemptingToResolveAllTypes : InMemoryIntegrationFixture
    {
        [Fact]
        public void AllTypesAreResolvedSuccessfully()
        {
            var registrations = Container.LifetimeScope.ComponentRegistry.Registrations.Select(x => x.Activator.LimitType);
            var palavyrRegos = registrations.Where(x => x.Assembly.FullName.Contains("Palavyr")).Select(x => x.Name).ToList();

            var allTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName.Contains("Palavyr"))
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsInterface)
                .ToList();

            var stringBuilder = new StringBuilder();

            foreach (var t in allTypes)
            {
                try
                {
                    var instance = Container.GetService(t);
                    if (instance is null)
                        stringBuilder.Append($"{t.Name}\n");
                }
                catch (Exception)
                {
                    stringBuilder.Append($"{t.Name}\n");
                }
            }

            this.PalavyrAssent(stringBuilder.ToString());
        }

        public WhenAttemptingToResolveAllTypes(ITestOutputHelper testOutputHelper, IntegrationTestAutofacWebApplicationFactory factory)
            : base(testOutputHelper, factory)
        {
        }
    }
}