using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Domain.Accounts;

namespace Palavyr.API.controllers.accounts.newAccount
{
    [Route("api")]
    [ApiController]
    public class GetCurrentPlanController : ControllerBase
    {
        private AccountsContext accountsContext;
        private ILogger<GetCurrentPlanController> logger;
        public GetCurrentPlanController(AccountsContext accountsContext, ILogger<GetCurrentPlanController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpGet("account/settings/current-plan")]
        public async Task<IActionResult> GetCurrentPlan([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            string planStatus;
            switch (account.PlanType)
            {
                case (UserAccount.PlanTypeEnum.Free):
                    planStatus = UserAccount.PlanTypeEnum.Free.ToString();
                    break;
                case (UserAccount.PlanTypeEnum.Premium):
                    planStatus = UserAccount.PlanTypeEnum.Premium.ToString();
                    break;    
                case (UserAccount.PlanTypeEnum.Pro):
                    planStatus = UserAccount.PlanTypeEnum.Pro.ToString();
                    break;
                default:
                    logger.LogDebug("Plan type was not able to be determined.");
                    throw new Exception("Plan Type not able to be determined.");
            }
            return Ok(planStatus);
        }
    }
}