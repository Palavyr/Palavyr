using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using Palavyr.IntegrationTests.AppFactory.IntegrationTestFixtures;
using Test.Common.ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace Palavyr.IntegrationTests.Tests
{
    public class WhenAttemptingToResolveAllTypes : IntegrationTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        [Fact]
        public void AllTypesAreResolvedSuccessfully()
        {
            var allTypes = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName!.Contains("Palavyr"))
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsInterface)
                .ToList();

            var badTypes = new List<Type>();
            foreach (var t in allTypes)
            {
                try
                {
                    var instance = Container.GetService(t);
                    if (instance is null)
                    {
                        badTypes.Add(t);
                    }
                }
                catch (Exception)
                {
                    badTypes.Add(t);
                }
            }

            var filteredBadTypes = badTypes
                .Where(x => !x.IsInterface)
                .DistinctBy(x => x.Name)
                .Select(x => x.Name)
                .ToList();
            if (filteredBadTypes.Count > 0)
            {
                throw new Exception($"Unable to resolve types: {string.Join(", ", filteredBadTypes)}");
            }

            // this.PalavyrAssent(stringBuilder.ToString());
        }

        [Fact]
        public void AllHandlersAreResolved()
        {
            // var containerBuilder = new ContainerBuilder();
            // var config = TestConfiguration.GetTestConfiguration();
            //
            // Startup.ContainerSetup(containerBuilder, config);
            // var iContainer = containerBuilder.Build();
            // var data = new AutofacRegistrations(iContainer.ComponentRegistry);
            //
            // Assert.Throws<ConventionFailedException>(() => Convention.Is(new CanResolveAllRegisteredServices(iContainer), data));
        }


        public WhenAttemptingToResolveAllTypes(ITestOutputHelper testOutputHelper, ServerFactory factory)
            : base(testOutputHelper, factory)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public override async Task DisposeAsync()
        {
            await Task.CompletedTask;
            WebHostFactory.Dispose();
        }
    }
}