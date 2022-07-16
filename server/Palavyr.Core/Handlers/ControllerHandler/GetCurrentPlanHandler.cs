using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Data.Entities;
using Palavyr.Core.Resources;
using Palavyr.Core.Services.AccountServices.PlanTypes;
using Palavyr.Core.Stores;
using Palavyr.Core.Stores.StoreExtensionMethods;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCurrentPlanHandler : IRequestHandler<GetCurrentPlanRequest, GetCurrentPlanResponse>
    {
        private readonly IPlanTypeRetriever planTypeRetriever;
        private readonly IEntityStore<Account> accountStore;

        public GetCurrentPlanHandler(
            IPlanTypeRetriever planTypeRetriever,
            IEntityStore<Account> accountStore
        )
        {
            this.planTypeRetriever = planTypeRetriever;
            this.accountStore = accountStore;
        }

        public async Task<GetCurrentPlanResponse> Handle(GetCurrentPlanRequest request, CancellationToken cancellationToken)
        {
            var account = await accountStore.GetAccount();

            var planStatus = await planTypeRetriever.GetCurrentPlanType();
            var currentPlan = new PlanStatusResource
            {
                HasUpgraded = account.HasUpgraded,
                Status = planStatus
            };
            return new GetCurrentPlanResponse(currentPlan);
        }
    }

    public class GetCurrentPlanResponse
    {
        public GetCurrentPlanResponse(PlanStatusResource response) => Response = response;
        public PlanStatusResource Response { get; set; }
    }

    public class GetCurrentPlanRequest : IRequest<GetCurrentPlanResponse>
    {
    }
}