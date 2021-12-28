// #nullable enable
// using System;
// using System.Threading;
// using Autofac;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using Palavyr.API;
// using Palavyr.API.CustomMiddleware;
// using Palavyr.API.Registration.Configuration;
// using Palavyr.API.Registration.Container;
// using Palavyr.Core.Services.AccountServices;
// using Palavyr.Core.Sessions;
// using Palavyr.IntegrationTests.AppFactory.AutofacWebApplicationFactory;
//
// // How to inherit from startup
// // https://stackoverflow.com/questions/53269815/webapplicationfactory-throws-error-that-contentrootpath-does-not-exist-in-asp-ne
//
// namespace Palavyr.IntegrationTests.AppFactory.TestStartup
// {
//     public class IntegrationTestStartup : Startup
//     {
//         private readonly IWebHostEnvironment env;
//         private readonly IConfiguration configuration;
//
//         public IntegrationTestStartup()
//         {
//             
//         }
//         public IntegrationTestStartup(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration)
//         {
//             this.env = env;
//             this.configuration = configuration;
//         }
//
//         public override void ContainerSetup(ContainerBuilder builder, IConfiguration configuration)
//         {
//             ;
//         }
//
//         public override void SetServices(IServiceCollection services, IConfiguration config, IWebHostEnvironment environ)
//         {
//             var conf = TestConfiguration.GetTestConfiguration();
//             var env = new FakeEnvironmentConfiguration
//             {
//                 EnvironmentName = "Development"
//             };
//             services.AddHttpContextAccessor();
//             services.AddAuthorization();
//             services.AddAuthentication();
//             services.AddControllers().AddControllersAsServices();
//             CorsConfiguration.ConfigureCorsService(services, env);
//             Configurations.ConfigureStripe(conf);
//             RegisterStores(services, conf);
//             ServiceRegistry.RegisterHealthChecks(services);
//             ServiceRegistry.RegisterIisConfiguration(services, env);
//             ServiceRegistry.RegisterMediator(services);
//
//         }
//
//         public override void DoConfigure(IApplicationBuilder app, ILoggerFactory? factory = null)
//         {
//             AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//             PalavyrAccessChecker.AssertEnvironmentsDoNoOverlap();
//             app.UseHttpsRedirection();
//             app.UseRouting();
//             app.UseCors();
//             app.UseMiddleware<ErrorHandlingMiddleware>();
//             app.UseMiddleware<SetCancellationTokenTransportMiddleware>();
//             app.UseMiddleware<SetHeadersMiddleware>(); // MUST come after UseAuthentication to ensure we are setting these headers on authenticated requests
//
//             app.UseEndpoints(
//                 endpoints =>
//                 {
//                     endpoints.MapControllers();
//                     endpoints.MapHealthChecks("/healthcheck");
//                 });
//         }
//     }
// }