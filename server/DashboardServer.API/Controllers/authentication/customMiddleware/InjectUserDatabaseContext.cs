// using System;
// using System.Threading.Tasks;
// using DashboardServer.Data;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Http;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace DashboardServer.API.CustomMiddleware
// {
//     public class InjectUserDatabaseContext
//     {
//
//         private readonly RequestDelegate _next;
//
//         public InjectUserDatabaseContext(RequestDelegate next)
//         {
//             _next = next;
//         }
//
//         public async Task InvokeAsync(HttpContext httpContext, IWebHostEnvironment env)
//         {
//             var server = env.EnvironmentName == "production" ? null : "RegEx";
//             var settingsJson = env.EnvironmentName == "production"
//                 ? "appsettings.json"
//                 : "appsettings.Development.json";
//             
//             // Middleware has already performed the userId lookup for us and placed it int he header
//             httpContext.Request.Headers
//             
//             var userId = Request.Headers["userId"];
//             
//             IConfigurationRoot config = new ConfigurationBuilder()
//                 .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//                 .AddJsonFile(settingsJson)
//                 .Build();
//             
//             var newOptions = new DbContextOptionsBuilder<ServerDbContext>();
//             var connectString =
//                 $"Server={server};Database={userId};Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//
//             newOptions.UseSqlServer(connectString);
//             var dbContext = new ServerDbContext(newOptions.Options);
//             dbContext.Database.EnsureCreated();
//             Context = dbContext;
//             return dbContext;           
//         }
//         
//
//     }
// }