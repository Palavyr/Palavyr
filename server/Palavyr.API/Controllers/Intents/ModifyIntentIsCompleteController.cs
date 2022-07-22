using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Intents
{
    public class ModifyIntentIsCompleteController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "intents/intent-toggle";

        public ModifyIntentIsCompleteController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<bool> Put(ModifyIntentIsCompleteRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}