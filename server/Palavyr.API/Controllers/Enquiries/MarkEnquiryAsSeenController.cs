using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Enquiries
{
    public class MarkEnquiriesAsSeenController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/seen";

        public MarkEnquiriesAsSeenController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task Put([FromBody] MarkEnquiryAsSeenRequest request, CancellationToken cancellationToken)
        {
            await mediator.Publish(request, cancellationToken);
        }
    }
}