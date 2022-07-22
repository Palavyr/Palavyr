using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Response.SubjectControllers
{
    public class ModifyIntentEmailSubjectController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public ModifyIntentEmailSubjectController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(ModifyIntentEmailSubjectRequest.Route)]
        public async Task<string> Modify(
            [FromBody]
            ModifyIntentEmailSubjectRequest request,
            CancellationToken cancellationToken
        )
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}