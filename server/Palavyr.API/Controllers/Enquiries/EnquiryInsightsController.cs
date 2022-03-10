using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;
using Palavyr.Core.Services.EnquiryServices;

namespace Palavyr.API.Controllers.Enquiries
{
    public class EnquiryInsightsController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiry-insights";


        public EnquiryInsightsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<EnquiryInsightsResource[]> Get(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new EnquiryInsightsRequest(), cancellationToken);
            return response.Response;
        }
    }
}