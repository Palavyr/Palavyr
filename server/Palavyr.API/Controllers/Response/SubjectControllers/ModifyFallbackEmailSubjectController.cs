using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    public class ModifyFallbackEmailSubjectController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "email/fallback/default-subject";

        public ModifyFallbackEmailSubjectController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<string> Modify([FromBody] ModifyFallbackEmailSubjectRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}