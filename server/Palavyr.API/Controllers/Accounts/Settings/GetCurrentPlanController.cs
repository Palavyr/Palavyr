using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Accounts.Settings
{
    public class GetCurrentPlanController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/current-plan";

        public GetCurrentPlanController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<PlanStatus> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCurrentPlanRequest(), cancellationToken);
            return response.Response;
        }
    }
}