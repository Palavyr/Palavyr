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