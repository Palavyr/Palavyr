using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices.PlanTypes;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetCurrentPlanController : PalavyrBaseController
    {
        private readonly IPlanTypeRetriever planTypeRetriever;
        private readonly IAccountRepository accountRepository;
        private ILogger<GetCurrentPlanController> logger;

        public GetCurrentPlanController(IPlanTypeRetriever planTypeRetriever, IAccountRepository accountRepository, ILogger<GetCurrentPlanController> logger)
        {
            this.planTypeRetriever = planTypeRetriever;
            this.accountRepository = accountRepository;
            this.logger = logger;
        }

        [HttpGet("account/settings/current-plan")]
        public async Task<PlanStatus> GetCurrentPlan(
            [FromHeader]
            string accountId,
            CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount(accountId, cancellationToken);
            var planStatus = await planTypeRetriever.GetCurrentPlanType(accountId, cancellationToken);
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