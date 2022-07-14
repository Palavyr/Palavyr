using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTests.AppFactory.AutofacWebApplicationFactory;
using IntegrationTests.AppFactory.IntegrationTestFixtures;
using Test.Common.ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Tests
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