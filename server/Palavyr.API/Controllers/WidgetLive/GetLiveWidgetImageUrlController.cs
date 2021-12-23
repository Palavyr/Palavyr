using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Palavyr.Core.Common.ExtensionMethods;
using Palavyr.Core.Exceptions;
using Palavyr.Core.Repositories;
using Palavyr.Core.Services.AmazonServices;

namespace Palavyr.API.Controllers.WidgetLive
{
    public class GetLiveWidgetImageUrlController : PalavyrBaseController
    {
        private readonly IConfigurationRepository repository;
        private readonly ILinkCreator linkCreator;
        private readonly IConfiguration configuration;
        private const string Route = "widget/node-image/{nodeId}";

        public GetLiveWidgetImageUrlController(IConfigurationRepository repository, ILinkCreator linkCreator, IConfiguration configuration)
        {
            this.repository = repository;
            this.linkCreator = linkCreator;
            this.configuration = configuration;
        }

        [HttpGet(Route)]
        public async Task<string> GetImageUrl(

            [FromRoute]
            string nodeId,
            CancellationToken cancellationToken
        )
        {
            var convoNode = await repository.GetConversationNodeById(nodeId);
            var image = await repository.GetImageById(convoNode.ImageId);
            if (image.S3Key == null)
            {
                throw new DomainException("Failed to set the file key for this image.");
            }

            var preSignedUrl = linkCreator.GenericCreatePreSignedUrl(image.S3Key, configuration.GetUserDataBucket(), DateTime.Now.AddDays(3));
            return preSignedUrl;
        }
    }
}