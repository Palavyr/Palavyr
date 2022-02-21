using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AccountServices.PlanTypes;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCurrentPlanHandler : IRequestHandler<GetCurrentPlanRequest, GetCurrentPlanResponse>
    {
        private readonly IPlanTypeRetriever planTypeRetriever;
        private readonly IAccountRepository accountRepository;

        public GetCurrentPlanHandler(
            IPlanTypeRetriever planTypeRetriever,
            IAccountRepository accountRepository
        )
        {
            this.planTypeRetriever = planTypeRetriever;
            this.accountRepository = accountRepository;
        }

        public async Task<GetCurrentPlanResponse> Handle(GetCurrentPlanRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAccount();

            var planStatus = await planTypeRetriever.GetCurrentPlanType();
            var currentPlan = new PlanStatus
            {
                HasUpgraded = account.HasUpgraded,
                Status = planStatus
            };
            return new GetCurrentPlanResponse(currentPlan);
        }
    }

    public class GetCurrentPlanResponse
    {
        public GetCurrentPlanResponse(PlanStatus response) => Response = response;
        public PlanStatus Response { get; set; }
    }

    public class GetCurrentPlanRequest : IRequest<GetCurrentPlanResponse>
    {
    }

    public class PlanStatus
    {
        public string Status { get; set; }
        public bool HasUpgraded { get; set; }
    }
}