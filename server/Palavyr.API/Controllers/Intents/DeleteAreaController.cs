using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Areas
{
    public class DeleteAreaController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "intents/delete/{intentId}";

        public DeleteAreaController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpDelete(Route)]
        public async Task Delete(
            [FromRoute]
            DeleteAreaRequest request,
            CancellationToken cancellationToken
        )
        {
            await mediator.Publish(request, cancellationToken);
        }
    }
}