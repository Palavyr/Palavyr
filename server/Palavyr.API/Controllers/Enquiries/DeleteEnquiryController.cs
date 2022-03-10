using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Models.Resources.Responses;

namespace Palavyr.API.Controllers.Enquiries
{
    public class DeleteEnquiryController : PalavyrBaseController
    {
        private readonly IMediator mediator;

        public const string Route = "enquiries/selected";

        public DeleteEnquiryController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpPut(Route)]
        public async Task<Enquiry[]> DeleteSelected(
            DeleteEnquiryRequest request,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return response.Response;
        }
    }
}