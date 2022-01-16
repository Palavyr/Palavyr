using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;

namespace Palavyr.API.Controllers.Enquiries
{
    public class GetShowSeenEnquiriesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/show";

        public GetShowSeenEnquiriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<bool> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetShowSeenEnquiriesRequest(), cancellationToken);
            return response.Response;
        }
    }
}