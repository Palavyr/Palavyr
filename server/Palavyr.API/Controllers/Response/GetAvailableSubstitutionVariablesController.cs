using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Response
{
    public class GetAvailableSubstitutionVariablesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/variables";

        public GetAvailableSubstitutionVariablesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<ResponseVariableResource> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAvailableSubstitutionVariablesRequest(), cancellationToken);
            return response.Resource;
        }
    }
}