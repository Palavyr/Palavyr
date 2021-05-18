// #nullable enable
// using System;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Autofac;
// using Microsoft.EntityFrameworkCore.Storage;
// using Palavyr.API;
// using Palavyr.Core.Data;
// using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Palavyr.IntegrationTests.AppFactory.DataBuilders;
// using Palavyr.IntegrationTests.AppFactory.ExtensionMethods;
//
//
// namespace Palavyr.IntegrationTests.AppFactory.ContextBuilder
// {
//     // public class DefaultTestContext
//     // {
//     //     public HttpClient Client { get; set; } = null!;
//     //     public DashContext DashContext { get; set; } = null!;
//     //     public AccountsContext AccountsContext { get; set; } = null!;
//     //     public ConvoContext ConvoContext { get; set; } = null!;
//     // }
//
//     public static class ContextBuilder
//     {
//         public static async Task<DefaultTestContext> CreateBareTestContext(
//             this InMemoryAutofacWebApplicationFactory factory,
//             Action<AccountsContext>? configureAccounts = null,
//             Action<DashContext>? configureDash = null,
//             Action<ConvoContext>? configureConvo = null
//         )
//         {
//             return await CreateBareTestContext(factory, configureAccounts, configureDash, configureConvo);
//         }
//
//         public static async Task<DefaultTestContext> CreateDefaultTestContext(
//             this InMemoryAutofacWebApplicationFactory factory,
//             Action<AccountsContext>? configureAccounts = null,
//             Action<DashContext>? configureDash = null,
//             Action<ConvoContext>? configureConvo = null
//         )
//         {
//             var defaultContext = CreateContext(factory, configureAccounts, configureDash, configureConvo);
//
//             await defaultContext
//                 .CreateDefaultAccountAndSessionBuilder()
//                 .WithDefaultPassword()
//                 .WithDefaultAccountId()
//                 .WithDefaultAccountType()
//                 .WithDefaultApiKey()
//                 .WithDefaultEmailAddress()
//                 .Build();
//             return defaultContext;
//         }
//
//         // private static DefaultTestContext CreateContext(
//         //     InMemoryAutofacWebApplicationFactory factory,
//         //     Action<AccountsContext>? configureAccounts,
//         //     Action<DashContext>? configureDash,
//         //     Action<ConvoContext>? configureConvo
//         // )
//         // {
//         //     factory.Server.BaseAddress = new Uri(IntegrationConstants.BaseUri);
//         //     var dbRoot = new InMemoryDatabaseRoot();
//         //     var webHost = factory
//         //         .WithWebHostBuilder(
//         //             builder =>
//         //             {
//         //                 builder
//         //                     .ConfigureTestContainer<ContainerBuilder>(builder => builder.CallStartupTestContainerConfiguration())
//         //                     .ConfigureTestServices(services => services.CallStartupServicesConfiguration().AddMvcCore().AddApplicationPart(typeof(Startup).Assembly))
//         //                     .ConfigureInMemoryDatabase(dbRoot)
//         //                     .EnsureAndConfigureDbs(configureAccounts, configureDash, configureConvo)
//         //                     .UseTestServer();
//         //             });
//         //
//         //     var dashContext = (DashContext) webHost.Services.GetService(typeof(DashContext));
//         //     var accountContext = (AccountsContext) webHost.Services.GetService(typeof(AccountsContext));
//         //     var conversationContext = (ConvoContext) webHost.Services.GetService(typeof(ConvoContext));
//
//             return new DefaultTestContext
//             {
//                 Client = webHost.ConfigureInMemoryClient(),
//                 DashContext = dashContext,
//                 AccountsContext = accountContext,
//                 ConvoContext = conversationContext
//             };
//         }
//     }
// }