using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data;
using Palavyr.Core.Models.Accounts.Schemas;

namespace Palavyr.API.Controllers.Accounts.Settings
{

    public class GetCurrentPlanController : PalavyrBaseController
    {
        private AccountsContext accountsContext;
        private ILogger<GetCurrentPlanController> logger;
        public GetCurrentPlanController(AccountsContext accountsContext, ILogger<GetCurrentPlanController> logger)
        {
            this.logger = logger;
            this.accountsContext = accountsContext;
        }
        
        [HttpGet("account/settings/current-plan")]
        public async Task<PlanStatus> GetCurrentPlan([FromHeader] string accountId)
        {
            var account = await accountsContext.Accounts.SingleOrDefaultAsync(row => row.AccountId == accountId);
            string planStatus;
            switch (account.PlanType)
            {
                case (Account.PlanTypeEnum.Free):
                    planStatus = Account.PlanTypeEnum.Free.ToString();
                    break;
                case (Account.PlanTypeEnum.Premium):
                    planStatus = Account.PlanTypeEnum.Premium.ToString();
                    break;    
                case (Account.PlanTypeEnum.Pro):
                    planStatus = Account.PlanTypeEnum.Pro.ToString();
                    break;
                default:
                    logger.LogDebug("Plan type was not able to be determined.");
                    throw new Exception("Plan Type not able to be determined.");
            }

            return new PlanStatus()
            {
                HasUpgraded = account.HasUpgraded,
                Status = planStatus
            };
        }

        public class PlanStatus
        {
            public string Status { get; set; }
            public bool HasUpgraded { get; set; }
        }
    }
}