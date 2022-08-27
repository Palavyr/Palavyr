using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

namespace Palavyr.API.Controllers.Intents
{
    public class CreateNewIntentController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public CreateNewIntentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(CreateIntentRequest.Route)]
        public async Task Create(
            [FromBody]
            CreateIntentRequest request,
            CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
        }
    }
}