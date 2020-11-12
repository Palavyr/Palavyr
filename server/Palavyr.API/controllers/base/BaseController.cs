using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace Palavyr.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly AccountsContext AccountContext;
        protected readonly DashContext DashContext;
        protected readonly ConvoContext ConvoContext;
        public IWebHostEnvironment Env;
        
        protected BaseController(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext, IWebHostEnvironment env)
        {
            DashContext = dashContext;
            AccountContext = accountContext;
            ConvoContext = convoContext;
            Env = env;
        }
    }
}
//----------------------------------------------------------------------------------------------------------------------
/// Keeping the following commented to show how I was able to get a DB per user.

// public ServerDbContext GetUserDb()
// {
//     // Must call in each controller... :/ WTF
//     var server = Env.EnvironmentName == "production" ? null : "RegEx";
//     var settingsJson = Env.EnvironmentName == "production"
//         ? "appsettings.json"
//         : "appsettings.Development.json";
//     
//     // Middleware has already performed the userId lookup for us and placed it int he header
//     var userId = Request.Headers["userId"];
//     
//     IConfigurationRoot config = new ConfigurationBuilder()
//         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//         .AddJsonFile(settingsJson)
//         .Build();
//     
//     var newOptions = new DbContextOptionsBuilder<ServerDbContext>();
//     var connectString =
//         $"Server={server};Database={userId};Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//
//     newOptions.UseSqlServer(connectString);
//     var dbContext = new ServerDbContext(newOptions.Options);
//     dbContext.Database.EnsureCreated();
//     Context = dbContext;
//     return dbContext;
// }
//
// public ServerDbContext GetUserDb(string userId)
// {
//     // Must call in each controller... :/ WTF
//     var server = Env.EnvironmentName == "production" ? null : "RegEx";
//     var settingsJson = Env.EnvironmentName == "production"
//         ? "appsettings.json"
//         : "appsettings.Development.json";
//
//     IConfigurationRoot config = new ConfigurationBuilder()
//         .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//         .AddJsonFile(settingsJson)
//         .Build();
//     
//     var newOptions = new DbContextOptionsBuilder<ServerDbContext>();
//     var connectString =
//         $"Server={server};Database={userId};Integrated Security=True;Connect Timeout=5;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
//
//     newOptions.UseSqlServer(connectString);
//     var dbContext = new ServerDbContext(newOptions.Options);
//     dbContext.Database.EnsureCreated();
//     Context = dbContext;
//     return dbContext;
// }
