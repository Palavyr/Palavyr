using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Obsolete
{
    [Obsolete("It seems this is no longer used by the frontend")]
    public class GetCurrentPlanController // : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "account/settings/current-plan";

        public GetCurrentPlanController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<PlanStatusResource> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetCurrentPlanRequest(), cancellationToken);
            return response.Response;
        }
    }
}