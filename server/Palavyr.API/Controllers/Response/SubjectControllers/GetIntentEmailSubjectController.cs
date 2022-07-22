using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    public class GetIntentEmailSubjectController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "email/subject/{intentId}";

        public GetIntentEmailSubjectController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get([FromRoute] string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetIntentEmailSubjectRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}