using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Intents
{
    public class PutUseIntentFallbackEmailController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public PutUseIntentFallbackEmailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut("intents/use-fallback-email-toggle")]
        public async Task<bool> Put(ModifyUseIntentFallbackEmailRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}