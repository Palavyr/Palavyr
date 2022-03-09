using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetEnquiryFileLinkHandler : IRequestHandler<GetEnquiryFileLinkRequest, GetEnquiryFileLinkResponse>
    {
        private readonly ILinkCreator linkCreator;

        public GetEnquiryFileLinkHandler(ILinkCreator linkCreator)
        {
            this.linkCreator = linkCreator;
        }

        public async Task<GetEnquiryFileLinkResponse> Handle(GetEnquiryFileLinkRequest request, CancellationToken cancellationToken)
        {
            var link = await linkCreator.CreateLink(request.FileId);
            return new GetEnquiryFileLinkResponse(link);
        }
    }

    public class GetEnquiryFileLinkResponse
    {
        public GetEnquiryFileLinkResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetEnquiryFileLinkRequest : IRequest<GetEnquiryFileLinkResponse>
    {
        public GetEnquiryFileLinkRequest(string fileId)
        {
            FileId = fileId;
        }

        public string FileId { get; set; }
    }
}