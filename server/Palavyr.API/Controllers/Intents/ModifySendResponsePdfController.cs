using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Intents
{
    public class ModifySendResponsePdfController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public ModifySendResponsePdfController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost(ModifySendResponseRequest.Route)]
        public async Task<bool> Post(
            [FromRoute]
            string intentId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ModifySendResponseRequest(intentId), cancellationToken);
            return response.NewState;
        }
    }
}