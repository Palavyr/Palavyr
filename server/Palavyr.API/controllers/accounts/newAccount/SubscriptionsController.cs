using System.Linq;
using DashboardServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Palavyr.API.Controllers;

namespace Palavyr.API.controllers.accounts.newAccount
{        
    [Route("api/subscriptions")]
    [ApiController]
    public class Subscriptions : BaseController
    { 
        public Subscriptions(AccountsContext accountContext, ConvoContext convoContext, DashContext dashContext,
                IWebHostEnvironment env) : base(accountContext, convoContext, dashContext, env)
            { }

        [HttpGet("count")]
        public int GetNumAreasAllowed([FromHeader] string accountId)
        {
            var numAreasAllowed = AccountContext.Subscriptions.Single(row => row.AccountId == accountId).NumAreas;
            return numAreasAllowed;
        }
    }
}