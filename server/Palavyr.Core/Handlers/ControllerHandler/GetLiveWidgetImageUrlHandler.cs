using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.Core.Handlers.ControllerHandler
{
    public class GetLiveWidgetImageUrlHandler : IRequestHandler<GetLiveWidgetImageUrlRequest, GetLiveWidgetImageUrlResponse>
    {
        private readonly IConfigurationRepository repository;
        private readonly ILinkCreator linkCreator;
        private readonly IConfiguration configuration;

        public GetLiveWidgetImageUrlHandler(IConfigurationRepository repository, ILinkCreator linkCreator, IConfiguration configuration)
        {
            this.repository = repository;
            this.linkCreator = linkCreator;
            this.configuration = configuration;
        }

        public async Task<GetLiveWidgetImageUrlResponse> Handle(GetLiveWidgetImageUrlRequest request, CancellationToken cancellationToken)
        {
            var convoNode = await repository.GetConversationNodeById(request.NodeId);
            var image = await repository.GetImageById(convoNode.ImageId);
            if (image.S3Key == null)
            {
                throw new DomainException("Failed to set the file key for this image.");
            }

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(image.S3Key, configuration.GetUserDataBucket(), DateTime.Now.AddDays(3));
            return new GetLiveWidgetImageUrlResponse(preSignedUrl);
        }
    }

    public class GetLiveWidgetImageUrlResponse
    {
        public GetLiveWidgetImageUrlResponse(string response) => Response = response;
        public string Response { get; set; }
    }

    public class GetLiveWidgetImageUrlRequest : IRequest<GetLiveWidgetImageUrlResponse>
    {
        public string NodeId { get; set; }
    }
}