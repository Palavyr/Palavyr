using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.AmazonServices;
using Palavyr.Core.Services.AmazonServices.S3Service;

namespace Palavyr.Core.Handlers
{
    public class GetEnquiryFileLinkHandler : IRequestHandler<GetEnquiryFileLinkRequest, GetEnquiryFileLinkResponse>
    {
        private readonly IConfiguration configuration;
        private readonly ILinkCreator linkCreator;
        private readonly IS3KeyResolver s3KeyResolver;

        public GetEnquiryFileLinkHandler(
            IConfiguration configuration,
            ILinkCreator linkCreator,
            IS3KeyResolver s3KeyResolver)
        {
            this.configuration = configuration;
            this.linkCreator = linkCreator;
            this.s3KeyResolver = s3KeyResolver;
        }

        public async Task<GetEnquiryFileLinkResponse> Handle(GetEnquiryFileLinkRequest request, CancellationToken cancellationToken)
        {
            var s3Key = s3KeyResolver.ResolveResponsePdfKey(request.FileId);
            var previewBucket = configuration.GetUserDataBucket();
            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(s3Key, previewBucket);
            return new GetEnquiryFileLinkResponse(preSignedUrl);
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