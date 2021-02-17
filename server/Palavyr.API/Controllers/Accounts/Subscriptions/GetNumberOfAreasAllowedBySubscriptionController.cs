using System;
using System.Threading.Tasks;
using DashboardServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Common.Constants;
using Server.Domain.Accounts;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{
    [Route("api")]
    [ApiController]
    public class GetNumberOfAreasAllowedBySubscriptionController : ControllerBase
    {
        private readonly AccountsContext accountsContext;

        public GetNumberOfAreasAllowedBySubscriptionController(AccountsContext accountsContext)
        {
            this.accountsContext = accountsContext;
        }

        [HttpGet("subscriptions/count")]
        public async Task<int> Get([FromHeader] string accountId)
        {
            var account = await accountsContext
                .Accounts
                .SingleAsync(row => row.AccountId == accountId);
            var planType = account.PlanType;
            int numAreasAllowed;
            switch (planType)
            {
                case UserAccount.PlanTypeEnum.Free:
                    numAreasAllowed = SubscriptionConstants.DefaultNumAreas;
                    break;
                case UserAccount.PlanTypeEnum.Premium:
                    numAreasAllowed = SubscriptionConstants.PremiumNumAreas;
                    break;
                case UserAccount.PlanTypeEnum.Pro:
                    numAreasAllowed = SubscriptionConstants.ProNumAreas;
                    break;
                default:
                    throw new Exception("PlanType Enum does not coallesce with the current values in the database. Investigate.");
            }

            return numAreasAllowed;
        }
    }
}