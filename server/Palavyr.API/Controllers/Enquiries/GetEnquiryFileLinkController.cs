using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Handlers;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.API.Controllers.Enquiries
{
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