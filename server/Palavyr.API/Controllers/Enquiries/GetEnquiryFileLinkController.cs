using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Palavyr.Core.Handlers.ControllerHandler;

namespace Palavyr.API.Controllers.Enquiries
{
    [Obsolete("It seems this is no longer used by the frontend")]
    public class GetEnquiryFileLinkController : PalavyrBaseController
    {
        private readonly IMediator mediator;
        public const string Route = "enquiries/link/{fileId}";


        public GetEnquiryFileLinkController(
            IMediator mediator
        )
        {
            this.mediator = mediator;
        }

        [HttpGet(Route)]
        public async Task<string> Get(
            [FromRoute]
            string fileId,
            CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new GetEnquiryFileLinkRequest(fileId), cancellationToken);
            return response.Response;
        }
    }
}