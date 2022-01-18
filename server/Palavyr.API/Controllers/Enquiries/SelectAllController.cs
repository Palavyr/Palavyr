using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Enquiries
{
    public class SelectAllController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/selectall";

        public SelectAllController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPost(Route)]
        public async Task SelectAll(CancellationToken cancellationToken)
        {
            await mediator.Publish(new SelectAllRequest(), cancellationToken);
        }
    }
}