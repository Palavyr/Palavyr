using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Enquiries
{
    public class ToggleShowSeenEnquiriesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/toggle-show";

        public ToggleShowSeenEnquiriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<bool> Put(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new ToggleShowSeenEnquiriesRequest(), cancellationToken);
            return response.Response;
        }
    }
}