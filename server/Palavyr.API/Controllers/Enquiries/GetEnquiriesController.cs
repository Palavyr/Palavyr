using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources;

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
        public async Task<IEnumerable<EnquiryResource>> Get([FromQuery] GetEnquiriesRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }

    public class SkipTake
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}