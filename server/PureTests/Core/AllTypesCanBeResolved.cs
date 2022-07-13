using Autofac;
using Palavyr.API.Registration.Container;
using Test.Common;
using TestStack.ConventionTests;
using TestStack.ConventionTests.Autofac;
using Xunit;

namespace Pure.Core
{
    public class AllTypesCanBeResolved : IUnitTestFixture
    {
        [Fact]
        public void TestResolution()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new GeneralModule());
            var iContainer = containerBuilder.Build();
            var data = new AutofacRegistrations(iContainer.ComponentRegistry);

            Assert.Throws<ConventionFailedException>(() => Convention.Is(new CanResolveAllRegisteredServices(iContainer), data));
        }
    }
}