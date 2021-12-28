// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Hosting;
// using Palavyr.API;
// using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
// using Xunit;
//
// namespace Palavyr.IntegrationTests.TestBase
// {
//     public abstract class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
//     {
//
//
//         protected override IHost CreateHost(IHostBuilder builder)
//         {
//             builder.UseServiceProviderFactory(new CustomServiceProviderFactory());
//         }
//         
//         public virtual void SetUpDatabases()
//         {
//         }
//     }
//
//     public class InMemoryIntegrationTest : CustomWebApplicationFactory<Startup>
//     {
//         public CustomWebApplicationFactory<Startup> Factory { get; }
//
//         public InMemoryIntegrationTest(CustomWebApplicationFactory<Startup> factory)
//         {
//             Factory = factory;
//         }
//     }
//
//     public class MyTestTest : IClassFixture<InMemoryIntegrationTest>
//     {
//         public MyTestTest(CustomWebApplicationFactory<Startup> factory)
//         {
//         }
//
//         [Fact]
//         public async Task SomeTest()
//         {
//         }
//     }
// }