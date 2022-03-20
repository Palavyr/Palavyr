using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Intents
{
    public class DeleteIntentController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "intents/delete/{intentId}";

        public DeleteIntentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete(Route)]
        public async Task Delete(
            [FromRoute]
            DeleteIntentRequest request,
            CancellationToken cancellationToken
        )
        {
            await mediator.Publish(request, cancellationToken);
        }
    }
}