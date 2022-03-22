using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Enquiries
{
    public class GetEnquiryCountController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/count";

        public GetEnquiryCountController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<int> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetEnquiryCountRequest(), cancellationToken);
            return response.Response;
        }
    }
}