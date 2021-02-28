// using System;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Palavyr.Domain.Configuration.Schemas;
// using Palavyr.IntegrationTests.AppFactory;
// using Xunit;
//
// namespace Palavyr.IntegrationTests.Tests.ControllerFixtures
// {
//     public class EnsureDbIsValidControllerFixture : IClassFixture<IntegrationTestFixtureFactory>, IDisposable
//     {
//         private readonly IntegrationTestFixtureFactory factory;
//         private const string Route = "configure-conversations/ensure-db-valid";
//
//         public EnsureDbIsValidControllerFixture(IntegrationTestFixtureFactory factory)
//         {
//             factory.(
//                 DbSetupAndTeardown.SeedTestAccount,
//                 db =>
//                 {
//                     db.WidgetPreferences.Add(WidgetPreference.CreateEmpty(IntegrationConstants.AccountId));
//                     db.SaveChanges();
//                 });
//             this.factory = factory;
//
//         }
//
//         [Fact]
//         public async Task EnsuresDbIsValid()
//         {
//             var response = await Client.PostAsync(Route, new StringContent(""));
//             response.EnsureSuccessStatusCode();
//         }
//
//         public void Dispose()
//         {
//             factory.DisposeDbsByReset();
//         }
//     }
// }