using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Models.Accounts.Schemas;
using Palavyr.Core.Repositories;

namespace Palavyr.Core.Services.AccountServices.PlanTypes
{
    public class PlanTypeRetriever : IPlanTypeRetriever
    {
        private readonly IAccountRepository repository;
        private readonly ILogger<PlanTypeRetriever> logger;

        public PlanTypeRetriever(IAccountRepository repository, ILogger<PlanTypeRetriever> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<string> GetCurrentPlanType()
        {
            var account = await repository.GetAccount();
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