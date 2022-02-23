using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    public class GetAreaEmailSubjectController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "email/subject/{intentId}";

        public GetAreaEmailSubjectController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get([FromRoute] string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetAreaEmailSubjectRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}