using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Palavyr.Common.GlobalConstants;
using Palavyr.Data;
using Palavyr.Domain.Accounts.Schemas;

namespace Palavyr.API.Controllers.Accounts.Subscriptions
{

    public class GetNumberOfAreasAllowedBySubscriptionController : PalavyrBaseController
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
                case Account.PlanTypeEnum.Free:
                    numAreasAllowed = SubscriptionConstants.DefaultNumAreas;
                    break;
                case Account.PlanTypeEnum.Premium:
                    numAreasAllowed = SubscriptionConstants.PremiumNumAreas;
                    break;
                case Account.PlanTypeEnum.Pro:
                    numAreasAllowed = SubscriptionConstants.ProNumAreas;
                    break;
                default:
                    throw new Exception("PlanType Enum does not coallesce with the current values in the database. Investigate.");
            }

            return numAreasAllowed;
        }
    }
}