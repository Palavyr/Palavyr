using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public class PlanTypeRetriever : IPlanTypeRetriever
    {
        private readonly IEntityStore<Account> accountStore;
        private readonly ILogger<PlanTypeRetriever> logger;

        public PlanTypeRetriever(IEntityStore<Account> accountStore, ILogger<PlanTypeRetriever> logger)
        {
            this.accountStore = accountStore;
            this.logger = logger;
        }

        public async Task<string> GetCurrentPlanType()
        {
            var account = await accountStore.GetAccount();
            string planStatus;
            switch (account.PlanType)
            {
                case (Account.PlanTypeEnum.Free):
                    planStatus = Account.PlanTypeEnum.Free.ToString();
                    break;
                case (Account.PlanTypeEnum.Lyte):
                    planStatus = Account.PlanTypeEnum.Lyte.ToString();
                    break;
                case (Account.PlanTypeEnum.Premium):
                    planStatus = Account.PlanTypeEnum.Premium.ToString();
                    break;    
                case (Account.PlanTypeEnum.Pro):
                    planStatus = Account.PlanTypeEnum.Pro.ToString();
                    break;
                default:
                    logger.LogDebug("Plan type was not able to be determined.");
                    throw new DomainException("Plan Type not able to be determined.");
            }

            return planStatus;
        }
    }
}