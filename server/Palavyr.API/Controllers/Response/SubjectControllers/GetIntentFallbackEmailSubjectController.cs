using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    public class GetIntentFallbackEmailSubjectController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "email/fallback/subject/{intentId}";


        public GetIntentFallbackEmailSubjectController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Modify([FromRoute] string intentId, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetIntentFallbackEmailSubjectRequest(intentId), cancellationToken);
            return response.Response;
        }
    }
}