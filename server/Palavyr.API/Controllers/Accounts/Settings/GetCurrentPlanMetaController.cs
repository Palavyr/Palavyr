using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.AccountServices.PlanTypes;

namespace Palavyr.API.Controllers.Accounts.Settings

{
    public class GetCurrentPlanMetaController : PalavyrBaseController
    {
        private const string Route = "account/settings/current-plan-meta";
        private readonly IBusinessRules businessRules;
        private ILogger<GetCurrentPlanMetaController> logger;

        public GetCurrentPlanMetaController(IBusinessRules businessRules, ILogger<GetCurrentPlanMetaController> logger)
        {
            this.businessRules = businessRules;
            this.logger = logger;
        }

        [HttpGet(Route)]
        public async Task<PlanTypeMeta> GetCurrentPlan(
            [FromHeader] string accountId,
            CancellationToken cancellationToken)
        {
            var currentPlan = await businessRules.GetPlanTypeMeta(accountId, cancellationToken);
            return currentPlan;
        }
    }
}