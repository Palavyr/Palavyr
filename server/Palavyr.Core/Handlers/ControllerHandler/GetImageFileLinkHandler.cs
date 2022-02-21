using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetImageFileLinkHandler : IRequestHandler<GetImageFileLinkRequest, GetImageFileLinkResponse>
    {
        private readonly IConfiguration configuration;
        private readonly ILinkCreator linkCreator;

        public GetImageFileLinkHandler(
            IConfiguration configuration,
            ILinkCreator linkCreator)
        {
            this.configuration = configuration;
            this.linkCreator = linkCreator;
        }

        public async Task<GetImageFileLinkResponse> Handle(GetImageFileLinkRequest request, CancellationToken cancellationToken)
        {
            var previewBucket = configuration.GetUserDataBucket();
            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(request.S3Key, previewBucket);
            return new GetImageFileLinkResponse(preSignedUrl);
        }
    }

    public class GetImageFileLinkResponse
    {
        public GetImageFileLinkResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetImageFileLinkRequest : IRequest<GetImageFileLinkResponse>
    {
        public string S3Key { get; set; }
    }
}