using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.AccountServices.PlanTypes;

namespace Palavyr.API.Controllers.Accounts.Settings

{
    public class GetCurrentPlanMetaController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        private const string Route = "account/settings/current-plan-meta";


        public GetCurrentPlanMetaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<PlanTypeMetaBase> GetCurrentPlan(
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCurrentPlanMetaRequest(), cancellationToken);
            return response.Response;
        }
    }
}