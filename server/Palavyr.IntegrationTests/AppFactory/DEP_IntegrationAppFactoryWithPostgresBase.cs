// using System;
// using System.Linq;
// using AspNetCore.Testing.Authentication.ClaimInjector;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Palavyr.Data;
//
// #nullable enable
//
// namespace Palavyr.IntegrationTests.AppFactory
// {
//     public class CustomWebApplicationFactory<TStartup>
//         : ClaimInjectorWebApplicationFactory<TStartup> where TStartup : class
//     {
//         protected override void ConfigureWebHost(IWebHostBuilder builder)
//         {
//             builder.ConfigureServices(
//                 services =>
//                 {
//                     var descriptor = services.SingleOrDefault(
//                         d => d.ServiceType ==
//                              typeof(DbContextOptions<AccountsContext>));
//
//                     services.Remove(descriptor);
//
//                     services.AddDbContext<AccountsContext>(options => { options.UseNpgsql(IntegrationConstants.AccountDbConnString("34")); });
//
//                     var sp = services.BuildServiceProvider();
//
//                     using (var scope = sp.CreateScope())
//                     {
//                         var scopedServices = scope.ServiceProvider;
//                         var db = scopedServices.GetRequiredService<AccountsContext>();
//                         var logger = scopedServices
//                             .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
//
//                         db.Database.EnsureCreated();
//
//                         try
//                         {
//                             db.SeedTestAccount();
//                         }
//                         catch (Exception ex)
//                         {
//                             logger.LogError(
//                                 ex, "An error occurred seeding the " +
//                                     "database with test messages. Error: {Message}", ex.Message);
//                         }
//                     }
//                 });
//         }
//     }
// }