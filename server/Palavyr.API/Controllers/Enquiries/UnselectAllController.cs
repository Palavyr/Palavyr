using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Enquiries
{
    public class UnselectAllController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public UnselectAllController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public const string Route = "enquiries/unselectall";

        [HttpPost(Route)]
        public async Task UnSelectAll(CancellationToken cancellationToken)
        {
            await mediator.Publish(new UnselectAllRequest(), cancellationToken);
        }
    }
}