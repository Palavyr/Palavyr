using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Enquiries
{
    public class GetEnquiriesController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries";

        public GetEnquiriesController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<Enquiry[]> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetEnquiriesRequest(), cancellationToken);
            return response.Response;
        }
    }
}