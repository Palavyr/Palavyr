using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Palavyr.Core.Services.AccountServices.PlanTypes;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetCurrentPlanMetaHandler : IRequestHandler<GetCurrentPlanMetaRequest, GetCurrentPlanMetaResponse>
    {
        private readonly IBusinessRules businessRules;
        private readonly ILogger<GetCurrentPlanMetaHandler> logger;

        public GetCurrentPlanMetaHandler(
            IBusinessRules businessRules,
            ILogger<GetCurrentPlanMetaHandler> logger)
        {
            this.businessRules = businessRules;
            this.logger = logger;
        }

        public async Task<GetCurrentPlanMetaResponse> Handle(GetCurrentPlanMetaRequest request, CancellationToken cancellationToken)
        {
            var currentPlan = await businessRules.GetPlanTypeMeta();
            return new GetCurrentPlanMetaResponse(currentPlan);
        }
    }

    public class GetCurrentPlanMetaController
    {
    }

    public class GetCurrentPlanMetaResponse
    {
        public GetCurrentPlanMetaResponse(PlanTypeMetaResource response) => Response = response;
        public PlanTypeMetaResource Response { get; set; }
    }

    public class GetCurrentPlanMetaRequest : IRequest<GetCurrentPlanMetaResponse>
    {
    }
}