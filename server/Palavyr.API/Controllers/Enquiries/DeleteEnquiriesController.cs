using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Resources.Responses;

namespace Palavyr.API.Controllers.Enquiries
{
    public class DeleteEnquiriesController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "enquiries/delete";

        public DeleteEnquiriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<IEnumerable<EnquiryResource>> DeleteSelected(
            DeleteEnquiryRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}